using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HoldToConfirm : MonoBehaviour
{
    [Header("Settings")]
    public string inputButton = "Fire1";  // or a key name like "space"
    public float holdDuration = 2f;       // seconds to trigger action
    public GameObject targetPanel;        // panel that must be active

    [Header("UI")]
    public Slider progressBar;             // fillable UI Image (type = Filled)

    [Header("Action")]
    public UnityEvent onHoldComplete;     // what happens when filled

    private float holdTime = 0f;
    private bool isHolding = false;
    private CanvasGroupFlasher canvasGroupFlasher;

    void Start()
    {
        progressBar.value = 0;
        canvasGroupFlasher = GetComponent<CanvasGroupFlasher>();
    }

    void Update()
    {
        if (targetPanel != null && !targetPanel.activeInHierarchy)
        {
            ResetBar();
            return;
        }

        if (GlobalDataStore.Instance.InputManager.ConfirmPromptHeld)
        {
            if (!canvasGroupFlasher.IsFlashing) canvasGroupFlasher.StartFlashing();
            isHolding = true;
            holdTime += Time.deltaTime;
            progressBar.value = holdTime / holdDuration;

            if (holdTime >= holdDuration)
            {
                onHoldComplete.Invoke();
                ResetBar();
            }
        }
        else if (isHolding) // released early
        {
            if (canvasGroupFlasher.IsFlashing) canvasGroupFlasher.StopFlashing();
            ResetBar();
        }
    }

    void ResetBar()
    {
        isHolding = false;
        holdTime = 0f;
        if (progressBar != null)
            progressBar.value = 0f;
    }
}
