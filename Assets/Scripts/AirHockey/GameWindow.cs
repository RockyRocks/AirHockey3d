using UnityEngine;
using System.Collections;

public class GameWindow : MonoBehaviour {

	public Camera MainCam;
	//Walls and thier collider referense setup
	public GameObject TopWallA;
	public GameObject TopWallB;
	public BoxCollider TopGoalPost;

	public GameObject BottomWallA;
	public GameObject BottomWallB;
	public BoxCollider BottomGoalPost;

	public GameObject LeftWall;
	public GameObject RightWall;
	public GameObject Plane;
	// Players Reference
	public Transform Player;
	public Transform AIplayer;

	// Total Hours worked on this project (15)
	// Gui Game States {not enough time}
	// Update is called once per frame 
	void Update () {
		MainCam.clearFlags = CameraClearFlags.Color;
	}
}
