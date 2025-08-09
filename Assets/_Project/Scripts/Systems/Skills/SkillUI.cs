using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkillUI : MonoBehaviour
{
    public Slider slider;
    public Image skillIcon;
    public Image inputIcon;
    public CanvasGroup inputIconCanvasGroup;
    public GameObject activeSkillSlottedRoot;
    public GameObject inactiveSkillSlottedRoot;
    public Skill insertedSkill;
    public bool isRecharging;
    public bool isReady;
    public CanvasGroup canvasGroup;


    public void SetSkill(Skill skill)
    {
        if (skill == null)
        {
            Debug.LogWarning("Skill is null.");
            return;
        }
        insertedSkill = skill;
        SetActive(true);
        SetSkillIcon(skill.icon);
        SetSliderColor(ColorManager.Instance.GetPrimaryColor(skill.elementType), ColorManager.Instance.GetSecondaryColor(skill.elementType));
    }



    public void SetSkillIcon(Sprite icon)
    {
        if (icon == null || insertedSkill == null)
        {
            Debug.LogWarning("Skill icon or inserted skill is null.");
            return;
        }
        skillIcon.sprite = icon;
    }



    public void SetInputIcon(Sprite icon)
    {
        if (icon == null || insertedSkill == null)
        {
            Debug.LogWarning("Input icon or inserted skill is null.");
            return;
        }
        inputIcon.sprite = icon;
    }



    public void SetSliderValue(float value)
    {
        if (slider == null)
        {
            Debug.LogWarning("Slider is not assigned.");
            return;
        }
        slider.value = value;
    }



    public void SetSliderMaxValue(float maxValue)
    {
        if (slider == null)
        {
            Debug.LogWarning("Slider is not assigned.");
            return;
        }
        slider.maxValue = maxValue;
    }



    public void SetActive(bool isActive)
    {
        if (activeSkillSlottedRoot == null || inactiveSkillSlottedRoot == null)
        {
            Debug.LogWarning("Active or inactive skill slotted root is not assigned.");
            return;
        }

        activeSkillSlottedRoot.SetActive(isActive);
        inactiveSkillSlottedRoot.SetActive(!isActive);
    }

    public void SetSliderColor(Color fillColor, Color backgroundColor)
    {
        if (slider == null)
        {
            Debug.LogWarning("Slider is not assigned.");
            return;
        }
        // Set slider background and fill color
        slider.transform.Find("Background").GetComponent<Image>().color = backgroundColor;
        slider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = fillColor;
    }


    // Use Skill
    public IEnumerator UseSkill()
    {
        // show the input icon was pressed we want this to be a visual feedback
        // we want it to fire even if the skill is not ready for visual feedback
        inputIconCanvasGroup.DOFade(.2f, .1f).OnComplete(() =>
        {
            inputIconCanvasGroup.DOFade(1f, .1f);
        });

        // null check
        if (insertedSkill == null)
        {
            Debug.LogWarning("Inserted skill is null.");
            yield break;
        }

        // check if the skill is ready or recharging
        if (isRecharging || !isReady)
        {
            Debug.Log("Skill is recharging or not ready.");
            yield break;
        }

        isReady = false;
        canvasGroup.alpha = .4f;

        // simulate skill execution
        insertedSkill.Execute(this);

        // set the skill slider to 0
        SetSliderValue(0);

        yield return new WaitForSeconds(insertedSkill.castTime);

        BeginRegeneration();
    }

    public void BeginRegeneration()
    {
        if (insertedSkill == null)
        {
            Debug.LogWarning("Inserted skill is null.");
            return;
        }

        isRecharging = true;
        isReady = false;

        slider.DOValue(1, insertedSkill.rechargeTime)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                canvasGroup.alpha = .9f;
                isRecharging = false;
                isReady = true;
            });
    }
}