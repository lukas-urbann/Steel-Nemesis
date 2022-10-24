using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class Tutorial : MonoBehaviour
    {
        private Animator anim;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        public void ShowTutorial()
        {
            anim.Play("TutorialShow");
        }

        public void HideTutorial()
        {
            anim.Play("TutorialHide");
        }
    }
}
