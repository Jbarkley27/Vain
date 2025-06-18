using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditorInternal;

public class ExtractionManager : MonoBehaviour
{
    public static ExtractionManager Instance { get; private set; }
    public BastionPortal BastionExtractionPortal;
    public enum PortalType
    {
        Bastion,
        Veil,
        Random_Veil
    }

    [Header("Extraction Settings")]
    public bool isInVeil = true;
    public bool isInBastion = false;
    public bool isExtracting = false;
    public int howManyTimesExtractedFromVeil = 0;
    public int baseExtractCost;
    public List<ExtractionPortal> extractionPortals = new List<ExtractionPortal>();
    public bool firstTimeExtractingToVeil = true;
    public int howManyTimeExtractedFromBastion = 0;
    public ExtractionPortal VeilExtractionPortal;

    [Header("Leaving Veil")]
    public List<GameObject> veilOnlyGameObjects;
    public List<GameObject> bastionOnlyGameObjects;
    public Vector3 portalBeamFinalSize = new Vector3(1f, 1f, 1f);
    public float portalBeamDuration;
    public float beamScaleSpeed = 1f;
    public CanvasGroup transitionScreenToBastion_Basic;
    public float transitionScreenFadeDuration = 1f;
    public float transitionScreenDuration;
    public bool firstTimeExtractingToBastion = true;




    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a ExtractionManager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        transitionScreenToBastion_Basic.gameObject.SetActive(false);
        VeilExtractionPortal.gameObject.SetActive(false);
    }

    public void AddPortal(ExtractionPortal portal)
    {
        if (!extractionPortals.Contains(portal))
        {
            extractionPortals.Add(portal);
            Debug.Log($"Added portal: {portal.name}");
        }
        else
        {
            Debug.LogWarning($"Portal {portal.name} is already in the list.");
        }
    }







    // FROM RANDOM VEIL TO BASTION

    public IEnumerator StartExtractionFromVeilToBastion(ExtractionPortal portal)
    {
        Debug.Log("Starting extraction from Veil...");

        // move player onto portal
        GlobalDataStore.Instance.Player.transform.DOMove(portal.extractPoint.transform.position, 1f)
        .SetEase(Ease.Linear);

        yield return new WaitForSeconds(1f);

        isExtracting = true;

        // Activate the portal beam visual
        yield return StartCoroutine(ActivatePortalBeam(PortalType.Random_Veil, portal));

        // yield return new WaitForSeconds(2);

        yield return StartCoroutine(StartUITranstion());

        // Shrink Player
        GlobalDataStore.Instance.Player.transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(2f);

        // Deactivate first portal player is leaving from
        portal.ResetExtractionState();

        // turn off old location and activate new location
        yield return StartCoroutine(HandleEnvironmentChanges(PortalType.Bastion, portal));

        StartCoroutine(ToBastion());

    }


    public IEnumerator ToBastion()
    {
        GlobalDataStore.Instance.Player.GetComponent<Rigidbody>().position = BastionExtractionPortal.extractPoint.transform.position;

        // Hide all extraction portals in veil
        HideAllExtractionPortals();

        yield return StartCoroutine(HideUITransition());

        // BREAKING HERE
        yield return new WaitForSeconds(1f);

        // show new portal beam visual
        StartCoroutine(ActivatePortalBeam(PortalType.Bastion));

        // Shrink Player
        GlobalDataStore.Instance.Player.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(1f);

        // // Deactivate the portal beam visual
        yield return StartCoroutine(DeactivatePortalVisually(null, PortalType.Bastion));



        howManyTimesExtractedFromVeil++;
        howManyTimeExtractedFromBastion++;
        isExtracting = false;
        isInVeil = false;
        isInBastion = true;
        firstTimeExtractingToBastion = false;
        Debug.Log("Extraction to Bastion completed from Veil.");
    }





    public IEnumerator ActivatePortalBeam(PortalType portalType, ExtractionPortal portal = null)
    {
        switch (portalType)
        {
            case PortalType.Veil:
                // VeilExtractionPortal.gameObject.SetActive(false);
                if (VeilExtractionPortal.portalBeamVisual != null)
                {
                    VeilExtractionPortal.portalBeamVisual.SetActive(true);
                    VeilExtractionPortal.portalBeamVisual.transform.DOScale(portalBeamFinalSize, beamScaleSpeed).SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        // Optionally, you can add any additional logic here after the beam is fully scaled
                        VeilExtractionPortal.extractionBeamVFX.SetActive(true);
                        ScreenShakeManager.Instance.DoShake(new ScreenShakeManager.ShakeProfile(0.5f, 0.5f, 20f));
                    });
                }
                else
                {
                    Debug.LogWarning("Portal Beam Visual is not assigned.");
                }
                break;

            case PortalType.Bastion:
                // BastionExtractionPortal.gameObject.SetActive(true);
                if (BastionExtractionPortal.portalBeamVisual != null)
                {
                    BastionExtractionPortal.portalBeamVisual.SetActive(true);
                    BastionExtractionPortal.portalBeamVisual.transform.DOScale(portalBeamFinalSize, beamScaleSpeed).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        // Optionally, you can add any additional logic here after the beam is fully scaled
                        BastionExtractionPortal.extractionBeamVFX.SetActive(true);
                        ScreenShakeManager.Instance.DoShake(new ScreenShakeManager.ShakeProfile(0.5f, 0.5f, 20f));
                    });
                }
                else
                {
                    Debug.LogWarning("Portal Beam Visual is not assigned.");
                }
                break;
            case PortalType.Random_Veil:
                if (portal.portalBeamVisual != null)
                {
                    portal.portalBeamVisual.SetActive(true);
                    portal.portalBeamVisual.transform.DOScale(portalBeamFinalSize, beamScaleSpeed).SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        // Optionally, you can add any additional logic here after the beam is fully scaled
                        portal.extractionBeamVFX.SetActive(true);
                        ScreenShakeManager.Instance.DoShake(new ScreenShakeManager.ShakeProfile(0.5f, 0.5f, 20f));
                    });
                }
                else
                {
                    Debug.LogWarning("Portal Beam Visual is not assigned.");
                }
                break;

            default:
                Debug.LogWarning("Portal Beam Visual is not assigned.");
                break;
        }

        yield return new WaitForSeconds(1f); // Wait for a short duration before activating the beam

    }









    public IEnumerator StartUITranstion()
    {
        yield return new WaitForSeconds(0.5f); // Wait for a short duration before starting the transition

        // Fade in the transition screen to Bastion
        transitionScreenToBastion_Basic.alpha = 0;
        transitionScreenToBastion_Basic.gameObject.SetActive(true);


        // Enable the transition screen
        transitionScreenToBastion_Basic.DOFade(1, transitionScreenFadeDuration).SetEase(Ease.InOutSine);
    }
    



    public void HideAllExtractionPortals()
    {
        foreach (var portal in extractionPortals)
        {
            portal.gameObject.SetActive(false);
        }
    }




    public IEnumerator HideUITransition()
    {
        Debug.Log("Hiding UI transition...");
        // Disable the transition screen
        transitionScreenToBastion_Basic.DOFade(0, transitionScreenFadeDuration).SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                transitionScreenToBastion_Basic.gameObject.SetActive(false);
                Debug.Log("Transition hidden.");
            });

        yield return new WaitForSeconds(.5f);
    }










    public IEnumerator HandleEnvironmentChanges(PortalType portalExtractedTo, ExtractionPortal portal = null)
    {
        switch (portalExtractedTo)
        {
            case PortalType.Veil:
                // BastionExtractionPortal.ResetExtractionState();

                // Disable game objects when leaving Bastion
                foreach (var obj in bastionOnlyGameObjects)
                {
                    obj.SetActive(false);
                }

                // Enable game objects when entering Veil
                foreach (var obj in veilOnlyGameObjects)
                {
                    obj.SetActive(true);
                }
                break;

            case PortalType.Bastion:
                // VeilExtractionPortal.ResetExtractionState();

                // Disable game objects when leaving Veil
                foreach (var obj in veilOnlyGameObjects)
                {
                    obj.SetActive(false);
                }

                // Enable game objects when entering Bastion
                foreach (var obj in bastionOnlyGameObjects)
                {
                    obj.SetActive(true);

                }
                break;

            case PortalType.Random_Veil:
                // portal.ResetExtractionState();

                // Disable game objects when leaving Veil
                foreach (var obj in veilOnlyGameObjects)
                {
                    obj.SetActive(false);
                }

                // Enable game objects when entering Bastion
                foreach (var obj in bastionOnlyGameObjects)
                {
                    obj.SetActive(true);
                }
                break;

            default:
                Debug.LogError("Invalid portal type specified.");
                break;
        }

        yield return new WaitForSeconds(2); // Wait for a short duration before scaling the player

    }









    public IEnumerator DeactivatePortalVisually(ExtractionPortal portal, PortalType portalTypeToDeactivate)
    {
        yield return new WaitForSeconds(.5f);
        switch (portalTypeToDeactivate)
        {
            case PortalType.Veil:
                VeilExtractionPortal.portalBeamVisual.transform.DOScale(0, .2f).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        // Optionally, you can add any additional logic here after the beam is fully scaled
                        VeilExtractionPortal.extractionBeamVFX.SetActive(false);
                    });
                break;

            case PortalType.Bastion:
                BastionExtractionPortal.portalBeamVisual.transform.DOScale(0, .2f).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        // Optionally, you can add any additional logic here after the beam is fully scaled
                        BastionExtractionPortal.extractionBeamVFX.SetActive(false);
                    });
                break;

            case PortalType.Random_Veil:
                portal.portalBeamVisual.transform.DOScale(0, .2f).SetEase(Ease.Linear)
                     .OnComplete(() =>
                     {
                         // Optionally, you can add any additional logic here after the beam is fully scaled
                         portal.extractionBeamVFX.SetActive(false);
                     });
                break;

            default:
                Debug.LogError("Invalid portal type specified.");
                break;
        }

        // yield return new WaitForSeconds(.4f);
    }




    

    



    public int GetExtractionCost()
    {
        // Calculate the extraction cost based on how many times extracted from Veil
        return baseExtractCost * (howManyTimesExtractedFromVeil + 1); // Example cost calculation
    }

    

    public bool IsExtracting()
    {
        foreach (var portal in extractionPortals)
        {
            if (portal.startedExtraction)
            {
                return true;
            }
        }

        return false;
    }



    public void DisableVeil()
    {
        // Disable all game objects when leaving Veil
        foreach (var obj in veilOnlyGameObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Attempted to disable a null GameObject in DisableVeil.");
            }
        }

        Debug.Log("Veil disabled.");
    }

    public void EnableVeil()
    {
        // Enable all game objects when entering Veil
        foreach (var obj in veilOnlyGameObjects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Attempted to enable a null GameObject in EnableVeil.");
            }
        }

        Debug.Log("Veil enabled.");
    }
}
