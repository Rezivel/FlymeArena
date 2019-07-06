using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jdenticon;
using System.IO;

public class JdenticonIcon : MonoBehaviour {
    public Image Icon;
    public Text Address;
	public void LoadJdenticon(string address) {
        Identicon icon = Identicon.FromValue(address,100);
        Identicon.DefaultStyle.BackColor= Jdenticon.Rendering.Color.Transparent;
        icon.SaveAsPng(Application.persistentDataPath + "/icon.png");

        Texture2D tex = null;
        byte[] fileData;
        if (File.Exists(Application.persistentDataPath + "/icon.png"))
        {
            fileData = File.ReadAllBytes(Application.persistentDataPath + "/icon.png");
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
        }

        Icon.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        Address.text = address;
    }
}
