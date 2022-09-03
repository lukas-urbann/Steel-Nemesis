using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class Music : MonoBehaviour
    {
        public static Music Instance;
        
        public List<AudioClip> musicClips = new List<AudioClip>();
        private AudioSource audioSource;
        private AudioClip selectedSong;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void OnEnable()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!audioSource.isPlaying)
            {
                selectedSong = musicClips[Random.Range(0, musicClips.Count)];
                audioSource.clip = selectedSong;
                audioSource.Play();
            }
        }
    }
}
