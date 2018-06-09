using System; 
using UnityEngine;
using System.Text;
using System.Collections; 
using System.Collections.Generic; 
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
// Please use this class to call for any json requests in oddz api.
class Request : MonoBehaviour
{   
	public WWW GetUrl;
    [HideInInspector]
	public bool isDone=false;
	void Start(){}
	
	public WWW GET(string url)
	{
		GetUrl = new WWW (url);
		StartCoroutine (WaitForRequest (GetUrl));
		Debug.Log ("hurray we are in: GET");
		return GetUrl;
	}
	
	public WWW POST(string url, Dictionary<string,string> post)
	{
		WWWForm form = new WWWForm();
		foreach(KeyValuePair<String,String> post_arg in post)
		{
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
		Debug.Log ("hurray we are in: Post");
		return www; 
	}

	public WWW PostDataWithHeader(string url,Dictionary<string,string>POST,string HeaderType,string Header)
	{
		Hashtable headers = new Hashtable();
		headers.Add(HeaderType, Header);
		WWWForm form = new WWWForm();
		foreach(KeyValuePair<String,String> post_arg in POST)
		{
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www=new WWW (url,form.data,headers);
		StartCoroutine(WaitForRequest(www));
		Debug.Log ("hurray we are in: NewPostWithHeader");
		return www;
	}

	public WWW PostDataWithJson(string url,object jsonValue,string HeaderType,string Header)
	{
		Hashtable headers = new Hashtable();
		headers.Add(HeaderType, Header);
		string jsonString = JsonConvert.SerializeObject (jsonValue, Formatting.Indented);
		Debug.Log (jsonString);
		byte[] pData = Encoding.UTF8.GetBytes(jsonString.ToCharArray());
		WWW www=new WWW (url,pData,headers);
		StartCoroutine(WaitForRequest(www));
		Debug.Log ("hurray we are in: NewPostWithHeader");
		return www;
	}

	
	private IEnumerator WaitForRequest(WWW www)
	{
		Debug.Log ("hurray we are in: Wait");
		yield return www;
		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.text);
            DebugConsole.Log(www.text);
			isDone=true;
		} else {
			Debug.Log("WWW Error: "+ www.error);
            DebugConsole.Log(www.error);
			isDone=false;
		}  
	}
}