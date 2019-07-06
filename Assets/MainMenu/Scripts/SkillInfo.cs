using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfo : MonoBehaviour {
    public Text Name;
    public Text Description;
    public Text Damage;
    public Text Range;

    public void SetProperties(string name,string description, int damage, int range)
    {
        Name.text = name;
        Description.text = "Description: " + description;
        Damage.text = "Damage: " + damage;
        Range.text = "Range: " + range;
    }
}
