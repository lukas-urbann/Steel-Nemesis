using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    public class Fullscreen : Switch
    {
        [SerializeField] private SettingsManager settingsManager;

        protected override void CustomStart()
        {
            switch (settingsManager.GetSettingSwitchState(SettingSwitchType.Fullscreen))
            {
                case 0:
                    isEnabled = false;
                    break;
                case 1:
                    isEnabled = true;
                    break;
            }
            
            PlayAnimation();
            SwitchAction();
        }

        protected override void SwitchAction()
        {
            Screen.fullScreen = isEnabled;
            
            if(isEnabled)
                settingsManager.SetSettingSwitchState(SettingSwitchType.Fullscreen,1);
            else
                settingsManager.SetSettingSwitchState(SettingSwitchType.Fullscreen,0);
        }
    }
}