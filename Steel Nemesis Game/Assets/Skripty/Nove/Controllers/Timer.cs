using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class Timer : MonoBehaviour
    {
        private float seconds;

        private void Update()
        {
            seconds += Time.deltaTime * 1;
        }

        public int GetSeconds()
        {
            return (int)seconds;
        }
    }
}
