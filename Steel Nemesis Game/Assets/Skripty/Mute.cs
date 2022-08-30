using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mute : MonoBehaviour {

	public AudioListener listener;
	bool mute = false;

	void Update () {
		if(Input.GetKeyDown(KeyCode.M))
		{
			if(!mute)
			{
				AudioListener.volume = 0;
				mute = true;
			} else {
				AudioListener.volume = 1;
				mute = false;
			}
		}
	}
}
