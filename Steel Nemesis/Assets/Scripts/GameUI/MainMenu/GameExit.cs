using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI.MainMenu
{
    public class GameExit : MonoBehaviour
    {
        public void TriggerGameExit()
        {
            Application.Quit(1);
        }
    }
}
