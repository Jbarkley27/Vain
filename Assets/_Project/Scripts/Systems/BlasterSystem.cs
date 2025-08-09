using System.Collections;
using UnityEngine;
using TMPro;
using FMODUnity;
using UnityEngine.UI;
using Random=UnityEngine.Random;

// LEGEND
// COOLDOWN - mean how long it takes before a weapon begins to regenerate
// REGEN or REGENERATE TIME - means how fast a weapon regains ammo

public class WeaponSystems : MonoBehaviour
{
    public static WeaponSystems instance { get; private set; }

    [Header("General")]
    public GameObject _playerFireSource;
    public InputManager _inputManager;
    public GameObject _player;

    [Header("Cooldown")]
    public bool isCoolingDown = false;

    [Header("Regeneration")]
    public bool isRegeneratingAmmo = false;
    public bool canRegenerateAmmo = true;

    [Header("Firing Status")]
    public bool isShooting = false;

    Coroutine RegenCo;

    [Header("Equipped Blaster")]
    public Blaster EquippedBlaster;


    // this allows us to control when the player can and can't shoot
    // could be useful for status effects or when swapping and equipping
    // weapons new weapons
    public bool CanShoot = true;


    [Header("UI")]
    public TMP_Text currentAmmo;
    public TMP_Text maxAmmo;
    public Image blasterIcon;
    public CanvasGroup inputCG;

    

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found a Weapon System object, destroying new one.");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        InitiateUI();
    }


    public void InitiateUI()
    {
        if (!EquippedBlaster) return;
        EquippedBlaster.CurrentAmmo = EquippedBlaster.MaxAmmo;
        currentAmmo.text = EquippedBlaster.CurrentAmmo + "";
        maxAmmo.text = EquippedBlaster.MaxAmmo + "";
        blasterIcon.sprite = EquippedBlaster.BlasterIcon;
    }



    void Update()
    {
        ListenForShooting();
        inputCG.alpha = _inputManager.IsShooting ? .2f : .9f; 
    }




    // Shooting
    public void ListenForShooting()
    {
        if (_inputManager.IsShooting)
        {
            // making sure that the player canshoot - this variable will be used for stopping shooting in the game
            // making sure their not already shooting
            // making sure they still have enought bullets given their burst amount = burstAmount * burstAmountPerShot

            if (EquippedBlaster == null) return;
            if (!CanShoot || isShooting) return;

            InitiateShootingMode();
        }
    }



    public void InitiateShootingMode()
    {
        CanShoot = false;
        isShooting = true;
        isCoolingDown = false;
        canRegenerateAmmo = false;
        isRegeneratingAmmo = false;
        if (RegenCo != null) StopCoroutine(RegenCo);
        StartCoroutine(ShootBlaster());
    }

    public bool HasAmmo(int amount = 1)
    {
        if (!EquippedBlaster) return false;
        return EquippedBlaster.CurrentAmmo - amount >= 0;
    }

    public void UpdateUI()
    {
        if (!EquippedBlaster) return;
        currentAmmo.text = EquippedBlaster.CurrentAmmo + "";
        maxAmmo.text = EquippedBlaster.MaxAmmo + "";
    }


    public IEnumerator ShootBlaster()
    {
        for (int i = 0; i < EquippedBlaster.BurstAmount; i++)
        {
            for (int j = 0; j < EquippedBlaster.BurstAmountPerShot; j++)
            {
                if (!HasAmmo()) break;

                GameObject projectile = Instantiate(
                    EquippedBlaster.ProjectilePrefab,
                    _playerFireSource.transform.position,
                    Quaternion.LookRotation(transform.forward));

                ScreenShakeManager.Instance.DoShake(EquippedBlaster.ScreenShakeProfile);

                projectile.GetComponent<PlayerProjectile>().Initialize(GetProjectileDirection(EquippedBlaster.Accuracy), EquippedBlaster.ProjectileSpeed, EquippedBlaster.animationCurve, EquippedBlaster.Range, EquippedBlaster.BaseDamage);

                EquippedBlaster.CurrentAmmo--;
                UpdateUI();
                yield return new WaitForSeconds(EquippedBlaster.BurstAmountPerShotFireRate);

            }
            yield return new WaitForSeconds(EquippedBlaster.FireRate);
        }

        // Cooldown - NOT REGEN
        isCoolingDown = true;
        yield return new WaitForSeconds(EquippedBlaster.CooldownRate);
        EndBlasterCooldown();
    }



    public void EndBlasterCooldown()
    {
        CanShoot = true;
        isShooting = false;
        isCoolingDown = false;
        isRegeneratingAmmo = false;
        canRegenerateAmmo = true;

        if (!isShooting && canRegenerateAmmo)
        {
            RegenCo = StartCoroutine(RegenBlasterAmmo());
        }
    }



    // Cooldown Stuff
    public IEnumerator RegenBlasterAmmo()
    {
        yield return new WaitForSeconds(2f);

        if (EquippedBlaster.CurrentAmmo >= EquippedBlaster.MaxAmmo) yield break;
        if (isShooting) yield break;
        if (isRegeneratingAmmo) yield break;

        while (EquippedBlaster.CurrentAmmo < EquippedBlaster.MaxAmmo && !isShooting)
        {
            EquippedBlaster.CurrentAmmo += 1;
            UpdateUI();
            yield return new WaitForSeconds(EquippedBlaster.RegenRate);
        }
    }




    #region Helpers

    public Vector3 GetProjectileDirection(float spread)
    {
        Vector3 directionWithoutSpread = _player.transform.forward;

        float yFactor = Random.Range(0, 1f);

        //Calculate spread
        float y = yFactor * spread * GetRandomNegOrPositive();

        //calc new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(y, 0, 0); // add spread to last digit

        return directionWithSpread.normalized;
    }


    public int GetRandomNegOrPositive()
    {
        float positiveOrNeg = Random.Range(0, 1);
        int negFactor;

        if (positiveOrNeg > 0.5f)
        {
            negFactor = -1;
        }
        else
        {
            negFactor = 1;
        }

        return negFactor;
    }


    #endregion
}
