using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectedSkillUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public SkillInfo SkillInfo;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            MinusSkill();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (FindObjectOfType<SelectedSkills>().SelSkills[int.Parse(gameObject.name)] != -1)
        {
            SkillInfo.gameObject.SetActive(true);
            SkillInfo.transform.position = Input.mousePosition;
            List<Skill> skills = FindObjectOfType<Skills>().skills;
            int number = FindObjectOfType<SelectedSkills>().SelSkills[int.Parse(gameObject.name)];
            SkillInfo.SetProperties(skills[number].Name, skills[number].Name, skills[number].Damage, skills[number].Range);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SkillInfo.gameObject.SetActive(false);
    }

    public void MinusSkill()
    {
        SelectedSkills ss = FindObjectOfType<SelectedSkills>();
        if(ss.SelSkills[int.Parse(gameObject.name)] != -1)
        {
            transform.GetChild(0).GetComponent<Image>().sprite = null;
            FindObjectOfType<ContentSkillsController>().ClearState(ss.SelSkills[int.Parse(gameObject.name)]);
            ss.SelSkills[int.Parse(gameObject.name)] = -1;
            SkillInfo.gameObject.SetActive(false);
        }
    }
}
