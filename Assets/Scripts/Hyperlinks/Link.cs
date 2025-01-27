using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;
public class Link : MonoBehaviour
{
	[SerializeField]private string url = "https://dehub.depaul.edu/osi/home-(copy)/";

	public void SetLink(string newURL)
	{
		url = newURL;
	}
    public void OpenLinkJSPlugin()
	{
		#if !UNITY_EDITOR
		openWindow(url);
		#endif
	}

	[DllImport("__Internal")]
	private static extern void openWindow(string url);

}