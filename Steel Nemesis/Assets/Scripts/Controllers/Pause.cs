using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class Pause : MonoBehaviour
    {
        public static Pause Instance;
        public GameObject pauseScreen;
        private bool pause = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }
        
        private void Start()
        {
            Cursor.visible = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !Game.Instance.GetGameOver())
                CheckPause();
        }

        public bool GetPauseState()
        {
            return pause;
        }

        public void CheckPause()
        {
            pause = !pause;

            if (pause)
            {
                pauseScreen.SetActive(true);
                Cursor.visible = true;
                Time.timeScale = 0;
            }
            else
            {
                pauseScreen.SetActive(false);
                Cursor.visible = false;
                Time.timeScale = 1;
            }
        }
    }
}
