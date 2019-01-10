using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Xml.Linq;
using UnityEngine.Networking;

public class OrderButton : MonoBehaviour {

    public GameObject orderPage;
    public Camera summaryCam;
    public RawImage summaryPreview;
    public RawImage orderPreview;

    public void OnOrderClick()
    {
        orderPreview.texture = summaryPreview.texture;
        orderPage.SetActive(true);

        Texture2D preview = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        RenderTexture.active = summaryCam.targetTexture;
        preview.ReadPixels(new Rect(0,0,256, 256), 0, 0);
        preview.Apply();
        RenderTexture.active = null;

        byte[] raw = preview.EncodeToPNG();

        SendProductPic(raw);
    }

    public void OnBackClick()
    {
        orderPage.SetActive(false);
    }

    public void SendProductPic(byte[] raw)
    {
        StartCoroutine(UploadToImgur(raw));
    }

    public IEnumerator UploadToImgur(byte[] raw)
    {
        WWW wwwIP = new WWW("https://api.ipify.org");
        yield return wwwIP;

        string uploadedUrl = "";
        UnityWebRequest www;

        WWWForm wwwdata = new WWWForm();
        wwwdata.AddField("image", Convert.ToBase64String(raw));
        wwwdata.AddField("type", "base64");
        www = UnityWebRequest.Post("https://api.imgur.com/3/image.xml", wwwdata);

        string clientID = "c627f2b5dd438db";
        www.SetRequestHeader("AUTHORIZATION", "Client-ID " + clientID);

        // if you want to display a loading image, here is where you should call it

        yield return www.SendWebRequest();

        // disable loading image

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            XDocument xDoc = XDocument.Parse(www.downloadHandler.text);
            uploadedUrl = xDoc.Element("data").Element("link").Value;
        }
        SendEmail.to = "mirko@else-corp.it";
        SendEmail.subject = "Order Confirmation from St. Gallen [FAC-SIMILE]";
        SendEmail.ipAddress = wwwIP.text;
        SendEmail.body = SendEmail.DefineHtmlString(uploadedUrl);
        SendEmail.Send();
    }
}
