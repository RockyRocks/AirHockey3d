using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Stand-in for the UnityEngine.WWW type removed in Unity 6.
/// Existing request code yields on it and reads text, bytes, and error.
/// </summary>
public class WWW : CustomYieldInstruction, IDisposable
{
	readonly UnityWebRequest request;

	public WWW(string url)
	{
		request = UnityWebRequest.Get(url);
		request.SendWebRequest();
	}

	public WWW(string url, WWWForm form)
	{
		request = UnityWebRequest.Post(url, form);
		request.SendWebRequest();
	}

	public WWW(string url, byte[] postData, Hashtable headers)
	{
		request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
		request.uploadHandler = new UploadHandlerRaw(postData ?? Array.Empty<byte>());
		request.downloadHandler = new DownloadHandlerBuffer();
		request.disposeUploadHandlerOnDispose = true;
		request.disposeDownloadHandlerOnDispose = true;
		CopyHeaders(headers);
		request.SendWebRequest();
	}

	public override bool keepWaiting
	{
		get { return request != null && !request.isDone; }
	}

	public string error
	{
		get
		{
			if (request == null || !request.isDone)
				return null;
			if (request.result == UnityWebRequest.Result.Success)
				return null;
			return string.IsNullOrEmpty(request.error) ? request.result.ToString() : request.error;
		}
	}

	public string text
	{
		get { return request != null && request.downloadHandler != null ? request.downloadHandler.text : string.Empty; }
	}

	public byte[] bytes
	{
		get { return request != null && request.downloadHandler != null ? request.downloadHandler.data : Array.Empty<byte>(); }
	}

	public bool isDone
	{
		get { return request == null || request.isDone; }
	}

	public void Dispose()
	{
		if (request != null)
			request.Dispose();
	}

	void CopyHeaders(Hashtable headers)
	{
		if (headers == null)
			return;
		foreach (DictionaryEntry entry in headers)
		{
			if (entry.Key == null || entry.Value == null)
				continue;
			request.SetRequestHeader(entry.Key.ToString(), entry.Value.ToString());
		}
	}
}
