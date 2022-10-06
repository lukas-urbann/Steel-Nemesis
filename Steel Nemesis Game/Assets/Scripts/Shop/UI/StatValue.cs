using System;
using System.Collections.Generic;
using Controllers;
using TMPro;
using UnityEngine;

namespace Shop.UI
{
    public class StatValue : MonoBehaviour
    {
        private TMP_Text valueText;
        [SerializeField] private ShipStats shipStat;

        private void Start()
        {
            Controllers.Shop.Instance.onOpen += UpdateLabel;
            valueText = GetComponent<TMP_Text>();
        }

        private void UpdateLabel()
        {
            valueText.text = Player.Controller.StatsInstance.GetShipStats(shipStat).ToString("F0");
        }
    }
}
