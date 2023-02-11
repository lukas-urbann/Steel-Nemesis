using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Settings
{
    public enum SettingSwitchType
    {
        VSync,
        Fullscreen,
    }
    
    public class SettingsManager : MonoBehaviour
    {
        public static string masterVolPref = "MasterVol";
        public static string effectVolPref = "EffectsVol";
        public static string musicVolPref = "MusicVol";
        public AudioMixer audioMixer;
        
        public static SettingsManager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }

        private void Start()
        {
            StartSound();
        }

        public int GetSettingSwitchState(SettingSwitchType type)
        {
            if (PlayerPrefs.HasKey(type.ToString()))
            {
                return PlayerPrefs.GetInt(type.ToString());
            }
            else
            {
                PlayerPrefs.SetInt(type.ToString(), 0);
                SavePlayerPrefs();
                return 0;
            }
        }

        public void SetSettingSwitchState(SettingSwitchType type, int value)
        {
            Debug.Log("Setting '" + type + "' to: " + value);
            PlayerPrefs.SetInt(type.ToString(), value);
            
            SavePlayerPrefs();
        }

        public void SavePlayerPrefs()
        {
            PlayerPrefs.Save();
        }
        
        private void StartSound()
        {
            audioMixer.SetFloat(masterVolPref, Mathf.Log10(PlayerPrefs.GetFloat(masterVolPref, 0.75f)) * 20);
            audioMixer.SetFloat(effectVolPref, Mathf.Log10(PlayerPrefs.GetFloat(effectVolPref, 0.75f)) * 20);
            audioMixer.SetFloat(musicVolPref, Mathf.Log10(PlayerPrefs.GetFloat(musicVolPref, 0.75f)) * 20);
        }
    }
}
