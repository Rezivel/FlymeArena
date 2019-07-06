using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameHistoryPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animator;
    public GameObject win;
    public GameObject defeat;
    public GameObject draw;
    public Text Tx;
    public Text Enemy;

    public int[] Skills;
    public int[] SkillsEnemy;

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("ShowBtn", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("ShowBtn", false);
    }

    public void SetStats(string tx, string enemy, int[] skills, int[] skillsEnemy, int txChoose)
    {
        Tx.text = "ID: " + tx;
        Enemy.text = "Enemy: " + enemy;

        Skills = skills;
        SkillsEnemy = skillsEnemy;
        int result;
        Debug.Log(txChoose);
        if (txChoose == 1)
        {
            result = Fight.CalculateFight(skills, skillsEnemy, int.Parse(AttachmentBase58.HexToDecimal(AttachmentBase58.Base58NumericDecode(tx.Substring(0, 4)))));
        }
        else
        {
            result = Fight.CalculateFight(skillsEnemy, skills, int.Parse(AttachmentBase58.HexToDecimal(AttachmentBase58.Base58NumericDecode(tx.Substring(0, 4)))));
        }

        if(txChoose == 1 && result == 0)
        {
            win.SetActive(true);
        }
        else if(txChoose == 1 && result == 1)
        {
            defeat.SetActive(true);
        }
        else if(txChoose == 0 && result == 0)
        {
            defeat.SetActive(true);
        }
        else if (txChoose == 0 && result == 1)
        {
            win.SetActive(true);
        }
        else
        {
            draw.SetActive(true);
        }

        for (int i = 0; i < SkillsEnemy.Length; i++)
        {
            transform.GetChild(i).GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("SkillPreview")[SkillsEnemy[i]];
        }
        

    }
}
