using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using WavesCS;

public class Authorization : MonoBehaviour {
    public JdenticonIcon accountIcon;
    public AddressInfo UserInfo;
    public GameObject LoginPanel;
    public InputField addressField;
    private string text_Address;
    private readonly List<string> servers = new List<string>()
        { "3PDoCTxcLU9bHuw51TkGL9AByeLL9PBxnRn",
        "1PDoCTxcLU9bHuw51TkGL9AByeLL9PBxnRn" };

    public void Start()
    {
        string[] UserInfoString = LoadFile();
        if (UserInfoString.Length != 1)
        {
            LoginPanel.SetActive(true);
        }
        else
        {
            UserInfo.SetAddress(UserInfoString[0]);
            accountIcon.LoadJdenticon(UserInfoString[0]);
            FindObjectOfType<LoadGameHistory>().LoadHistory(UserInfo.address);
            LoadSelectedSkills();
        }
    }
    public void Login()
    {
        SaveFile();
        UserInfo.SetAddress(addressField.text);
        accountIcon.LoadJdenticon(addressField.text);
        LoginPanel.SetActive(false);
        FindObjectOfType<LoadGameHistory>().LoadHistory(UserInfo.address);
        LoadSelectedSkills();
    }

    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/UserInfo.txt";
        FileStream file;
        File.Create(destination).Close();
        if (File.Exists(destination)) file = File.Open(destination, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        else file = File.Open(destination, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        
        StreamWriter sw = new StreamWriter(file);
        
        sw.WriteLine(addressField.text);
        sw.Close();
        file.Close();
    }

    public string[] LoadFile()
    {
        string destination = Application.persistentDataPath + "/UserInfo.txt";
        if (!File.Exists(destination)){
            UnityEngine.Debug.LogError("File not found");
            return new string[0];
        }
        string[] UserInfoString = File.ReadAllLines(destination);
        return UserInfoString;
    }
    public void TextOnPointerEnter(Text textUI)
    {
        text_Address = textUI.text;
        textUI.text = "Change account address";
    }
    public void TextOnPointerExit(Text textUI)
    {
        textUI.text = text_Address;
    }
    public void ChangeAddress()
    {
        LoginPanel.SetActive(true);
        addressField.text = text_Address;
    }
    
    public void LoadSelectedSkills()
    {
        SkillsPlayer skills = FindObjectOfType<LoadGameHistory>().AddressSkills(UserInfo.address, new Node(Node.MainNetHost).GetHeight());
        if(skills.Skills != null && skills.Skills.Length == 3)
        {
            SelectedSkills ss = FindObjectOfType<SelectedSkills>();
            ss.SelSkills = skills.Skills;
            for (int i = 0; i < ss.SelSkills.Length; i++)
            {
                ss.selected[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("SkillPreview")[skills.Skills[i]];
                FindObjectOfType<ContentSkillsController>().skillsUI[skills.Skills[i]].GetComponent<ContentSkill>().ChangeColor();
            }
        }else
        {
            SelectedSkills ss = FindObjectOfType<SelectedSkills>();
            for (int i = 0; i < ss.SelSkills.Length; i++)
            {
                if (ss.SelSkills[i] != -1)
                {
                    ss.selected[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                    FindObjectOfType<ContentSkillsController>().ClearState(ss.SelSkills[i]);
                }
            }
            for (int i = 0; i < ss.SelSkills.Length; i++)
            {
                ss.SelSkills[i] = -1;
            }
        }

    }
}
