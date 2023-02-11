using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    public class Vsync : Switch
    {
        [SerializeField] private SettingsManager settingsManager;

        protected override void CustomStart()
        {
            switch (settingsManager.GetSettingSwitchState(SettingSwitchType.VSync))
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
            if (isEnabled)
            {
                settingsManager.SetSettingSwitchState(SettingSwitchType.VSync,1);
                QualitySettings.vSyncCount = 1;
            }
            else
            {
                settingsManager.SetSettingSwitchState(SettingSwitchType.VSync,0);
                QualitySettings.vSyncCount = 0;
            }
        }
    }
}
