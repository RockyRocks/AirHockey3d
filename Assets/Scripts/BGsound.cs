using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BGsound : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.HasKey ("MusicVolume")) {
			Properties.musicVolume = PlayerPrefs.GetFloat("MusicVolume");
		}
		if (PlayerPrefs.HasKey ("SFXVolume")) {
			Properties.sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
		}
	}
	
	// Update is called once per frame
	void Update () {
		audio.volume = Properties.musicVolume;
	}
}
