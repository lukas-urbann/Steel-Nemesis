using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class Resolution : MonoBehaviour
    {
        private List<UnityEngine.Resolution> screenResolutions = new List<UnityEngine.Resolution>();
        public TMPro.TMP_Dropdown resolutionDropdown;
        
        private void Start()
        {
            screenResolutions.AddRange(Screen.resolutions);
            resolutionDropdown.ClearOptions();

            int _minW = 800;
            int _minH = 600;
            
            for (int i = 0; i < screenResolutions.Count; i++)
            {
                if (screenResolutions[i].width < _minW || screenResolutions[i].height < _minH)
                {
                    screenResolutions.Remove(screenResolutions[i]);
                    i--; //Fuck you
                }
            }

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < screenResolutions.Count; i++)
            {
                string option = screenResolutions[i].width + "x" + screenResolutions[i].height + " @" + screenResolutions[i].refreshRate + "Hz";
                options.Add(option);

                if (screenResolutions[i].width == Screen.width &&
                    screenResolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }
            
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
        
        public void SetResolution(int resolutionIndex)
        {
            UnityEngine.Resolution resolution = screenResolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            //Manager.SoundManager.instance.PlaySelectSound();
        }
    }
}
