using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugger
{
    public class DebugController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Player.Controller.Instance.CreditCollectionCheat();

                
                if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                }
            }
        }
    }
}
