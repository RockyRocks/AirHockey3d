using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity 6 removed the web-player WWW type and Application.ExternalEval / ExternalCall.
/// These shims keep the old Oddz and Facebook call sites compiling. They do not perform network I/O.
/// </summary>
public class WWW : CustomYieldInstruction
{
	public string text = string.Empty;
	public string error;
	public byte[] bytes = System.Array.Empty<byte>();
	public string url = string.Empty;

	public WWW(string url)
	{
		this.url = url ?? string.Empty;
	}

	public WWW(string url, byte[] postData)
	{
		this.url = url ?? string.Empty;
	}

	public WWW(string url, byte[] postData, Dictionary<string, string> headers)
	{
		this.url = url ?? string.Empty;
	}

	public WWW(string url, WWWForm form)
	{
		this.url = url ?? string.Empty;
	}

	public override bool keepWaiting
	{
		get { return false; }
	}
}

public static class LegacyWebPlayer
{
	public static void Eval(string script)
	{
	}

	public static void Call(string functionName, params object[] args)
	{
	}
}
