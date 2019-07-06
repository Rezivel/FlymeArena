using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class SaveDataPanel : MonoBehaviour {
    public GameObject Panel;
    public Image QR;
    public InputField Recipient;
    public InputField Amount;
    public InputField Attachment;
    public InputField Url;
    public float amount = 0.00000001f;
    //public string attachment = "Qwsw1";
    public void ShowPanel(string attachment, string recipient, string Asset)
    {
        Panel.SetActive(true);
        Recipient.text = recipient;
        Amount.text = decimal.Parse(amount.ToString(), System.Globalization.NumberStyles.Float).ToString().Replace(",",".");
        Attachment.text = attachment;
        Url.text = $"https://client.wavesplatform.com/#send/{Asset}?recipient={recipient}&amount={Amount.text}&attachment={attachment}";
        Texture2D myQR = generateQR(Url.text);
        QR.sprite = Sprite.Create(myQR, new Rect(0, 0, myQR.width, myQR.height), new Vector2(0.5f, 0.5f));
    }
    public void ClosePanel()
    {
        Panel.SetActive(false);
    }
    public void CopyToClipboard(InputField infield)
    {
        TextEditor te = new TextEditor();
        te.text = infield.text;
        te.SelectAll();
        te.Copy();
    }
    public Color32[] EncodeQR(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    public Texture2D generateQR(string text)
    {
        var encoded = new Texture2D(256, 256);
        var color32 = EncodeQR(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }
}
