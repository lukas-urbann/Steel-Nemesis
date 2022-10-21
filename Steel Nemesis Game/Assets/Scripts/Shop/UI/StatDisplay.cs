using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using Player;
using TMPro;
using UnityEngine;
using Stats = Player.Stats;

namespace Shop.UI
{
    public class StatDisplay : MonoBehaviour
    {
        [SerializeField] private ShipStats stat;
        public TMP_Text label;
        
        private void Start()
        {
            Controllers.Shop.Instance.onOpen += UpdateLabel;
            Player.Controller.StatsInstance.onShipUpgrade += UpdateLabel;
            label = GetComponent<TMP_Text>();
            
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            for (int i = 0; i < Controller.StatsInstance.shipStatsList.Count; i++)
            {
                if (stat == Controller.StatsInstance.shipStatsList[i])
                    label.text = Player.Controller.StatsInstance.GetShipStats(Controller.StatsInstance.shipStatsList[i]).ToString("F2");
            }
        }
    }
}
