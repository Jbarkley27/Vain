using System.Collections.Generic;
using UnityEngine;

// Inputs triggers this class
// This class then communicates with the skillUI class
// then the skillUI executes the skill
public class PlayerSkillManager : MonoBehaviour
{
    public List<SkillUI> skillUIs = new List<SkillUI>();
    public List<Skill> equippedSkills = new List<Skill>();

    public static PlayerSkillManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found a PlayerSkillManager object, destroying new one");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        InitializeSkills();
        BeginRegeneration();
    }

    public void UseSkill(int skillIndex)
    {
        // Checks for making sure we can use the skill
        if (skillIndex < 0 || skillIndex >= skillUIs.Count)
        {
            Debug.LogError($"Skill index {skillIndex} is out of range.");
            return;
        }

        SkillUI skillUI = skillUIs[skillIndex];

        if (skillUI == null || skillUI.insertedSkill == null || !skillUI.isReady)
        {
            Debug.LogWarning($"SkillUI at index {skillIndex} is not ready or does not have a skill assigned.");
            return;
        }

        // Use the skill
        Debug.Log($"Using skill at index {skillIndex}: {equippedSkills[skillIndex].name}");
        StartCoroutine(skillUI.UseSkill());
    }


    private void InitializeSkills()
    {
        // Assign skills to their respective UI elements
        for (int i = 0; i < skillUIs.Count; i++)
        {
            if (i < equippedSkills.Count)
            {
                skillUIs[i].SetSkill(equippedSkills[i]);
                skillUIs[i].SetActive(true); // Initially set to active
            }
            else
            {
                skillUIs[i].SetActive(false); // Disable UI if no skill is assigned
            }
        }
    }

    public void BeginRegeneration()
    {
        foreach (var skillUI in skillUIs)
        {
            if (skillUI != null)
            {
                skillUI.BeginRegeneration();
            }
            else
            {
                Debug.LogWarning("SkillUI doesnt have a skill.");
            }
        }
    }

    public void SetAllSkillsActive(bool isActive)
    {
        foreach (var skillUI in skillUIs)
        {
            if (skillUI != null)
            {
                skillUI.SetActive(isActive);
            }
            else
            {
                Debug.LogWarning("SkillUI is null in SetAllSkillsActive.");
            }
        }
    }
}