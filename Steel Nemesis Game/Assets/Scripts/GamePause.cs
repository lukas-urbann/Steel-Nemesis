using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePause : MonoBehaviour {

	public bool pause = false;
	public Text pausetext;

	void Start()
	{
		pausetext.text = "";
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			if(!pause)
			{
				pausetext.text = "Pause";
				Time.timeScale = 0f;
				pause = true;
			} else {
				pausetext.text = "";
				Time.timeScale = 1f;
				pause = false;
			}
			
		}
	}
}
