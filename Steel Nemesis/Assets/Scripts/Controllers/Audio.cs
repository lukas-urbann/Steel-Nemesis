using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class Audio : MonoBehaviour
    {
        public static Audio Instance;
        private AudioSource audioSource;
        public AudioClip hit;
        
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

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip sound)
        {
            try
            {
                audioSource.PlayOneShot(sound);
            }
            catch (Exception e)
            {
                
            }
        }

        public void HitSound()
        {
            audioSource.PlayOneShot(hit);
        }
    }
}
