using UnityEngine;
#if UNITY_EDITOR
public class AddObjectBlur {
	
	[UnityEditor.MenuItem("Custom/Add Object Blur")]
	static void AddBlur() {
        foreach (Transform t in UnityEditor.Selection.transforms)
        {
			AddBlurRecursive(t);
		}
	}
	
	static void AddBlurRecursive(Transform t) {
		if (t.GetComponent<MeshRenderer>() && !t.GetComponent<ObjectBlur>()) {
			t.gameObject.AddComponent(typeof(ObjectBlur));
		}
		foreach (Transform child in t) {
			AddBlurRecursive(child);
		}
	}

    [UnityEditor.MenuItem("Custom/Add Object Blur", true)]
	static bool ValidateAddBlur() {
        return UnityEditor.Selection.activeTransform;
	}
	
}
#endif 