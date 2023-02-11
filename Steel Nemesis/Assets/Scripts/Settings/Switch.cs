using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    public class Switch : MonoBehaviour
    {
        private Animator anim;
        protected bool isEnabled = false;

        private void Start()
        {
            anim = GetComponent<Animator>();
            CustomStart();
        }

        protected virtual void CustomStart()
        {
            Debug.Log("Vanilla CustomStart from: " + gameObject.name);
        }

        public void SwitchState()
        {
            isEnabled = !isEnabled;
            PlayAnimation();
            SwitchAction();
        }

        protected virtual void SwitchAction()
        {
            Debug.Log("Vanilla SwitchAction from: " + gameObject.name);
        }

        protected void PlayAnimation()
        {
            if (isEnabled)
            {
                anim.Play("SwitchOn");
                return;
            }
            
            anim.Play("SwitchOff");
        }
    }
}
