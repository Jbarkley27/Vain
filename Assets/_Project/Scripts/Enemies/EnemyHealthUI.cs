using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthUI : MonoBehaviour
{
    public EnemyBase target; // The enemy to follow
    public Vector3 screenOffset = new Vector3(0, 50, 0); // Pixels above the enemy
    private Camera cam;
    private RectTransform rectTransform;
    public Slider _healthSlider;
    public TMP_Text healthText;
    public float _healthSliderSpeed;
    public GameObject statusEffectRoot;

    void Start()
    {
        cam = Camera.main;
        rectTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenPos = cam.WorldToScreenPoint(target.gameObject.transform.position);
        screenPos += screenOffset;
        rectTransform.position = screenPos;



        if (!Mathf.Approximately(_healthSlider.value, target.CurrentHealth))
        {
            _healthSlider.value = Mathf.Lerp(_healthSlider.value, target.CurrentHealth, _healthSliderSpeed);
        }
    }

    public void UpdateHealthText()
    {
        healthText.text = target.CurrentHealth + "";
    }

    public void SetTarget(EnemyBase newTarget)
    {
        target = newTarget;
        _healthSlider.maxValue = newTarget.MaxHealth;
        UpdateHealthText();
    }

    
}
