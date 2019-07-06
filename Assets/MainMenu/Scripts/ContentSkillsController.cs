using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentSkillsController : MonoBehaviour {
    public GameObject[] skillsUI;
    public void ClearState(int index)
    {
        skillsUI[index].transform.GetChild(0).GetComponent<Image>().color = new Color(255,255,255,255);
    }
}
