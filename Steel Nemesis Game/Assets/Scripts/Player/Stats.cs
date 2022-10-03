using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using UnityEngine;

namespace Player
{
    public class Stats : MonoBehaviour
    {
        
        private List<ShipStats> shipStatsList = new List<ShipStats>();
        private List<float> shipStatsValueList = new List<float>();
        
        private List<PlayerStats> playerStatsList = new List<PlayerStats>();
        private List<float> playerStatsValueList = new List<float>();

        private void Start()
        {
            shipStatsList.AddRange(Enum.GetValues(typeof(ShipStats)).Cast<ShipStats>());

            foreach (ShipStats asd in shipStatsList)
            {
                Debug.Log("Loaded Ship Stat: " + asd);
            }
        }

        public List<ShipStats> GetShipStats()
        {
        }

        public void AssignShipStats()
        {
            
        }

        public void UpgradeShipStat(ShipStats stat, float amount)
        {
            for (int i = 0; i < shipStatsList.Count; i++)
                if (shipStatsList[i] == stat)
                    shipStatsValueList[i] += amount;
        }
        
        public float GetShipStats(ShipStats stat)
        {
            for (int i = 0; i < shipStatsList.Count; i++)
                if (shipStatsList[i] == stat)
                    return shipStatsValueList[i];

            return 0;
        }
    }
}