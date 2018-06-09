using UnityEngine;
using System.Collections;

public class BoardProperties : MonoBehaviour 
{
	private static BoardProperties 		Instance;
	public static BoardProperties Get()
	{
		return Instance;
	}

	void Awake()
	{
		Instance = this;
	}


	public float 		m_FrictionRatioOnObjects;

	void Update()
	{
		m_FrictionRatioOnObjects = Mathf.Clamp (m_FrictionRatioOnObjects, 0.05f, 0.8f);
	}
}
