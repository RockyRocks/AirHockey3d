using UnityEngine;
using System.Collections;

[AddComponentMenu("WorkAround/StayAlive")]
public class StayAlive : MonoBehaviour {
	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
}
