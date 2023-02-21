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
        public AudioSource musicAudioSource;

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
            AssignObjects();
        }

        private void AssignObjects()
        {
            if (musicAudioSource == null)
                musicAudioSource = GameObject.Find("MusicController").GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !Game.Instance.GetGameOver() && !pause)
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
                musicAudioSource.Pause();
            }
            else
            {
                pauseScreen.SetActive(false);
                Cursor.visible = false;
                Time.timeScale = 1;
                musicAudioSource.Play();
            }
        }
    }
}
