using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class Volume : MonoBehaviour
    {
        
        private static string masterVol = "MasterVol", musicVol = "MusicVol", effectVol = "EffectsVol";

        public enum VolumeType
        {
            Master,
            Effect,
            Music
        }

        public VolumeType volumeType = VolumeType.Master;

        private void Start()
        {
            Slider slider = GetComponent<Slider>();

            switch (volumeType)
            {
                case VolumeType.Effect:
                    slider.value = PlayerPrefs.GetFloat(SettingsManager.effectVolPref, 0.75f);
                    break;
                case VolumeType.Master:
                    slider.value = PlayerPrefs.GetFloat(SettingsManager.masterVolPref, 0.75f);
                    break;
                case VolumeType.Music:
                    slider.value = PlayerPrefs.GetFloat(SettingsManager.musicVolPref, 0.75f);
                    break;
            }
        }

        public void SetLevel(float sliderValue)
        {
            SettingsManager.Instance.audioMixer.SetFloat(GetVolType(), Mathf.Log10(sliderValue) * 20);

            switch (volumeType)
            {
                case VolumeType.Effect:
                    PlayerPrefs.SetFloat(SettingsManager.effectVolPref, sliderValue);
                    break;
                case VolumeType.Master:
                    PlayerPrefs.SetFloat(SettingsManager.masterVolPref, sliderValue);
                    break;
                case VolumeType.Music:
                    PlayerPrefs.SetFloat(SettingsManager.musicVolPref, sliderValue);
                    break;
            }
        }

        private string GetVolType()
        {
            switch (volumeType)
            {
                case VolumeType.Master:
                    return masterVol;
                case VolumeType.Effect:
                    return effectVol;
                case VolumeType.Music:
                    return musicVol;
            }
            
            return null;
        }
    }
}
