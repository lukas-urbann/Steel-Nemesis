using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class Bars : MonoBehaviour
    {
        public enum BarType
        {
            HitPoints,
            Energy
        }

        public BarType type;

        private Slider slider;

        private void OnEnable()
        {
            slider = GetComponent<Slider>();
        }

        private void Update()
        {
            switch (type)
            {
                case BarType.Energy:
                    slider.value = Player.Instance.GetEnergy();
                    break;
                case BarType.HitPoints:
                    slider.value = Player.Instance.GetHitpoints();
                    break;
            }
        }
    }
}
