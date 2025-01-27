using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Net;

public class EmailSend : MonoBehaviour
{
    public void SendEmailWithData(string address, CSVReader.CampusGroup[] groupResults)
    {
        string name0 = groupResults[0].name;
        string name1 = groupResults[1].name;
        string name2 = groupResults[2].name;
        string name3 = groupResults[3].name;
        string name4 = groupResults[4].name;

        string link0 = groupResults[0].link;
        string link1 = groupResults[1].link;
        string link2 = groupResults[2].link;
        string link3 = groupResults[3].link;
        string link4 = groupResults[4].link;

        string uri = "https://osicalc-emailsender.onrender.com/?address=" + address
                      + "&name0=" + name0
                      + "&name1=" + name1
                      + "&name2=" + name2
                      + "&name3=" + name3
                      + "&name4=" + name4
                      + "&link0=" + link0
                      + "&link1=" + link1
                      + "&link2=" + link2
                      + "&link3=" + link3
                      + "&link4=" + link4;

        Debug.Log(uri);

        StartCoroutine(GetRequest(uri));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }
}
