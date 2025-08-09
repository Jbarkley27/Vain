using UnityEngine;
using UnityEngine.UI;

public class BoostUI : MonoBehaviour
{
    public Slider slider;
    public float maxBoost;
    public float boostRegenRate;
    public InputManager inputManager;
    public float drainRate;
    public bool isDraining;
    private float targetValue;
    public bool CanBoost;

    // Augment Idea : Can Shoot while boosting
    // Augment IDea : Greatly reduce boost's drain rate
    // Augment Idea : Greatly increase boost regen rate

    void Start()
    {
        // Start fully filled
        targetValue = maxBoost;
        slider.value = targetValue;
    }


    void Update()
    {
        CanBoost = EnoughEnergyToBoost();
        isDraining = inputManager.IsBoosting;
        float rate = isDraining ? drainRate : boostRegenRate;

        // Decide what the target value should be
        if (isDraining)
            targetValue = Mathf.Max(slider.minValue, targetValue - drainRate * Time.deltaTime);
        else
            targetValue = Mathf.Min(slider.maxValue, targetValue + boostRegenRate * Time.deltaTime);

        // Smoothly move the actual slider toward the target
        slider.value = Mathf.MoveTowards(slider.value, targetValue, rate * Time.deltaTime);
    }


    public bool EnoughEnergyToBoost()
    {
        return slider.value > 1f;
    }
}