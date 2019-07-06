using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class SelectedSkills : MonoBehaviour {
    public GameObject ErrorMsg;
    public GameObject[] selected;
    public int[] SelSkills;
    private void Start()
    {
        SelSkills = new int[3];
        for (int i = 0; i < SelSkills.Length; i++)
        {
            SelSkills[i] = -1;
        }
    }

    public void ShowSavePanel()
    {
        bool rdy = false;
        for (int i = 0; i < SelSkills.Length; i++)
        {
            if(SelSkills[i] == -1)
            {
                rdy = false;
                break;
            }
            rdy = true;
        }
        if (rdy)
        {
            ErrorMsg.SetActive(false);
            string numeric = SelSkills[0]+"A"+ SelSkills[1] + "A"+SelSkills[2];
            string attachment = AttachmentBase58.Base58NumericEncode(BigInteger.Parse(AttachmentBase58.HexToDecimal(numeric)));
            FindObjectOfType<SaveDataPanel>().ShowPanel(attachment,AddressInfo.recipient[0],AddressInfo.Asset);
        }
        else
        {
            ErrorMsg.SetActive(true);
            Debug.Log("Cant show WavesData panel!");
        }
    }

}
