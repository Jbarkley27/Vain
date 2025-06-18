using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BastionPortal : MonoBehaviour
{
   [Header("Portal Settings")]
    public bool isActive = false;
    public bool startedExtraction = false;
    public GameObject activeGlowVisual;
    public Transform extractPoint;
    public GameObject portalBeamVisual;
    public GameObject extractionBeamVFX;

    [Header("UI")]
    public GameObject extractionMenuRoot;
    public CanvasGroup promptCG;
    public CanvasGroup extractionButtonCG;
    public CanvasGroup inputCG;
    public string beforeExtractionText = "Return to Veil";
    public string afterExtractionText = "Returning to Veil...";
    public string buttonBeforeExtractionText = "Initiate";
    public string buttonAfterExtractionText = "Initiateing...";
    public TMP_Text extractionButtonText;
    public TMP_Text extractionPromptText;
    public Button extractionButton;
    public CanvasGroupFlasher canvasGroupFlasher;


    void Start()
    {
        isActive = false;
        startedExtraction = false;
        extractionBeamVFX.SetActive(false);
        portalBeamVisual.SetActive(false);
        HideExtractionMenu();
    }


    void Update()
    {
        if (!isActive) return;
        if (startedExtraction) return;

        // check if player is pressing confirm
        if (GlobalDataStore.Instance.InputManager.ConfirmPromptPressed)
        {
            startedExtraction = true;


            extractionButtonCG.DOFade(.3f, 0.2f).SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                inputCG.DOFade(.3f, 0.2f).SetEase(Ease.InOutSine);
                extractionButtonText.text = buttonAfterExtractionText;
                extractionPromptText.text = afterExtractionText;
                extractionButtonCG.DOFade(1f, 0.2f).SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    extractionButton.interactable = false;
                });
            });

            // Disable the input prompt and button
            GlobalDataStore.Instance.InputManager.ConfirmPromptPressed = false;

            // hand it over to the ExtractionManageR
            // StartCoroutine(ExtractionManager.Instance.StartExtractionFromBastionToVeil(this));
        }
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActive = true;
            ShowExtractionMenu();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (startedExtraction) return; // Don't hide if extraction is in progress
        if (other.CompareTag("Player"))
        {
            isActive = false;
            HideExtractionMenu();
        }
    }

    public void ShowExtractionMenu()
    {
        if (!isActive) return;

        extractionMenuRoot.SetActive(true);

        bool enoughResource = EnoughResources();
        // Check if the player has enough resources to extract
        if (!enoughResource)
        {
            extractionButton.interactable = false;
            canvasGroupFlasher.StopFlashing();
        }
        else
        {
            extractionButton.interactable = true;
            canvasGroupFlasher.StartFlashing();
        }

        extractionPromptText.text = beforeExtractionText;
        extractionButtonText.text = buttonBeforeExtractionText;

        promptCG.DOFade(1f, 0.7f).SetEase(Ease.InOutSine);

        // we need to make this visual slowly flash and glow until extraction is started or player leaves the portal
        if (activeGlowVisual.activeSelf == false)
        {
            activeGlowVisual.SetActive(true);
        }

        // Start the glowing effect
        if (activeGlowVisual.GetComponent<Renderer>() != null)
        {
            activeGlowVisual.GetComponent<Renderer>().material.DOColor(Color.yellow, "_EmissionColor", .6f).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            Debug.LogWarning("Active Glow Visual does not have a Renderer component.");
        }

    }

    public void HideExtractionMenu()
    {
        extractionMenuRoot.SetActive(false);
        promptCG.DOFade(0f, 0.2f).SetEase(Ease.InOutSine);
        extractionButtonCG.DOFade(0f, 0.3f).SetEase(Ease.InOutSine);



        // Stop the glowing effect
        if (activeGlowVisual.activeSelf == true)
        {
            activeGlowVisual.SetActive(false);
        }
        if (activeGlowVisual.GetComponent<Renderer>() != null)
        {
            activeGlowVisual.GetComponent<Renderer>().material.DOKill();
            activeGlowVisual.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
        }
        else
        {
            Debug.LogWarning("Active Glow Visual does not have a Renderer component.");
        }
    }


    public bool EnoughResources()
    {
        if (ExtractionManager.Instance.GetExtractionCost() <= Inventory.Instance.GetResourceAmount(Resource.ResourceType.ION))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetExtractionState()
    {
        startedExtraction = false;
        extractionButton.interactable = true;
        extractionButtonText.text = buttonBeforeExtractionText;
        extractionPromptText.text = beforeExtractionText;

        // Reset the UI elements
        promptCG.alpha = 0f;
        extractionButtonCG.alpha = 0f;

        // Reset the portal beam visual
        if (portalBeamVisual.activeSelf)
        {
            portalBeamVisual.SetActive(false);
        }
    }
}