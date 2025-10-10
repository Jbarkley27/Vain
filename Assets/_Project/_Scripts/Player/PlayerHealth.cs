using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    public int MaxHealth;
    public int CurrentHealth;
    public int HealthBonus;
    public GameObject _heartIcon;
    public Slider _healthSlider;
    public TMP_Text _healthText;
    public float _healthSliderSpeed;
    public Color _heartHitColor;
    public Color _originalHeartIconColor;
    



    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a Player Health object, destroying new one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        MaxHealth = StatManager.Instance.GetHealthValue();
        CurrentHealth = MaxHealth;
        HealthBonus = 0;
        _healthSlider.maxValue = MaxHealth;
        _healthSlider.value = MaxHealth;
    }

    void Update()
    {
        if (!Mathf.Approximately(_healthSlider.value, CurrentHealth))
        {
            _healthSlider.value = Mathf.Lerp(_healthSlider.value, CurrentHealth, _healthSliderSpeed);
        }

        _healthText.text = $"{CurrentHealth} | {MaxHealth}";
    }

    public void TakeDamage(int damage)
    {
        _heartIcon.GetComponent<Image>().DOColor(_heartHitColor, .3f)
        .OnComplete(() =>
        {
            _heartIcon.GetComponent<Image>().color = _originalHeartIconColor;
        });

        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth + HealthBonus);
        _healthText.text = $"{CurrentHealth} | {MaxHealth}";
    }
}