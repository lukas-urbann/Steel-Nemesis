using Controllers;
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
                    slider.value = Controller.Instance.GetBattery();
                    slider.maxValue = Controller.StatsInstance.GetShipStats(ShipStats.MaxBattery);
                    break;
                case BarType.HitPoints:
                    slider.value = Controller.Instance.GetHP();
                    slider.maxValue = Controller.StatsInstance.GetShipStats(ShipStats.MaxHitPoints);
                    break;
            }
        }
    }
}
