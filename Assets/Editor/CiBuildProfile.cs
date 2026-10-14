using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class CiBuildProfile : IPreprocessBuildWithReport
{
	public int callbackOrder
	{
		get { return 0; }
	}

	public void OnPreprocessBuild(BuildReport report)
	{
		string[] args = Environment.GetCommandLineArgs();
		bool dev = Has(args, "-devBuild");
		bool prod = Has(args, "-prodBuild");
		if (!dev && !prod)
			return;

		PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel31;
		PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
		PlayerSettings.iOS.targetOSVersionString = "13.0";
		PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;

		EditorUserBuildSettings.development = dev;
		EditorUserBuildSettings.allowDebugging = dev;
		EditorUserBuildSettings.connectProfiler = false;

		NamedBuildTarget named = report.summary.platform == BuildTarget.iOS
			? NamedBuildTarget.iOS
			: NamedBuildTarget.Android;
		PlayerSettings.SetScriptingBackend(
			named,
			prod || report.summary.platform == BuildTarget.iOS
				? ScriptingImplementation.IL2CPP
				: ScriptingImplementation.Mono2x);
		PlayerSettings.SetIl2CppCompilerConfiguration(
			named,
			prod ? Il2CppCompilerConfiguration.Release : Il2CppCompilerConfiguration.Debug);
		SetDefine(named, dev ? "AIRHOCKEY_DEV" : "AIRHOCKEY_PROD", dev ? "AIRHOCKEY_PROD" : "AIRHOCKEY_DEV");
	}

	static void SetDefine(NamedBuildTarget named, string add, string remove)
	{
		string defines = PlayerSettings.GetScriptingDefineSymbols(named);
		string[] parts = defines.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
		string next = "";
		for (int i = 0; i < parts.Length; i++)
		{
			if (parts[i] == remove || parts[i] == add)
				continue;
			if (next.Length > 0)
				next += ";";
			next += parts[i];
		}
		if (next.Length > 0)
			next += ";";
		next += add;
		PlayerSettings.SetScriptingDefineSymbols(named, next);
	}

	static bool Has(string[] args, string name)
	{
		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] == name)
				return true;
		}
		return false;
	}
}
