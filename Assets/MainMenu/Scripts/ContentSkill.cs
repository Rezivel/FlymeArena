using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ContentSkill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public SkillInfo SkillInfo;
    public Color color;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            AddSkill();
            ChangeColor();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SkillInfo.gameObject.SetActive(true);
        SkillInfo.transform.position = Input.mousePosition;
        List<Skill> skills = FindObjectOfType<Skills>().skills;
        int number = int.Parse(gameObject.name);
        SkillInfo.SetProperties(skills[number].Name, skills[number].Name, skills[number].Damage, skills[number].Range);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SkillInfo.gameObject.SetActive(false);
    }

    public void AddSkill()
    {
        SelectedSkills ss = FindObjectOfType<SelectedSkills>();
        bool canAddSkill = false;
        int id = -1;
        for (int i = 0; i < ss.SelSkills.Length; i++)
        {
            if (ss.SelSkills[i] == int.Parse(gameObject.name))
            {
                canAddSkill = false;
                break;
            }else if (ss.SelSkills[i] == -1 && id == -1)
            {
                id = i;
            }
            if(id != -1)
            {
                canAddSkill = true;
            }
        }

        if (canAddSkill)
        {
            ss.SelSkills[id] = int.Parse(gameObject.name);
            ss.selected[id].transform.GetChild(0).GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<Image>().sprite;
        }

    }

    public void ChangeColor()
    {
        transform.GetChild(0).GetComponent<Image>().color = color;
    }
}
