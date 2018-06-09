using UnityEngine;
using System.Collections;

// This class is used to delete all the game objects  once the scene is unloading.
public class WorkAround : MonoBehaviour {

	// Find all the game objects in the scene

	public static void LoadLevelWorkaround(string level){
		Transform[] allTransforms  = FindObjectsOfType(typeof(Transform)) as Transform[];
	// Cycle through them and delete everything that isn't set to Persist
	
		for(int i= 0; i < allTransforms.Length; i++)	{
			// Only look at the transform roots
			if(allTransforms[i]) // Make sure the object is still around (might not be the case of the transform was another child of a root that has already been destroyed)	
			{
				Transform root= allTransforms[i].root;

				if(!root.GetComponent<StayAlive>()){
					if(root.gameObject.activeInHierarchy || !root.gameObject.activeInHierarchy)
					Destroy(root.gameObject);
				}
			}
		}
		// Additively load the specified level
		Application.LoadLevelAdditive(level);
	}
}
