using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using UnityEngine;

namespace Player
{
    public enum StatOrigin
    {
        Base,
        Upgrade,
        Skillpoint,
        Multiplicator
    }
    
    public class Stats : MonoBehaviour
    {
        public List<ShipStats> shipStatsList = new List<ShipStats>();
        
        [SerializeField] private List<float> activeStatsValueList = new List<float>(); // Tohle je to co používá hráč aktivně, jsou v tom všecky sečtené
        [SerializeField] private List<float> shipStatsValueList = new List<float>(); // Tohle je pouze loď
        [SerializeField] private List<float> shipBaseStatsValueList = new List<float>(); // Tohle jsou pouze base staty lodě
        [SerializeField] private List<float> skillpointStatsValueList = new List<float>(); // Tohle jsou pouze hráčovy skillpointy
        [SerializeField] private List<float> statMultiplierValueList = new List<float>(); // Tohle je multiplikátor lodě
        
        
        private List<PlayerStats> playerStatsList = new List<PlayerStats>();
        private List<float> playerStatsValueList = new List<float>();

        private void Start()
        {
            shipStatsList.AddRange(Enum.GetValues(typeof(ShipStats)).Cast<ShipStats>());

            foreach (ShipStats stat in shipStatsList)
                Debug.Log("Loaded Ship Stat: " + stat);
            
            AssignShipStats();
        }

        public void AssignShipStats()
        {
            float[] baseStats =
                {
                    Controller.Instance.shipType.baseMaxHitPoints,
                    Controller.Instance.shipType.baseMaxBattery,
                    Controller.Instance.shipType.baseBatteryRecharge,
                    Controller.Instance.shipType.baseCooldown,
                    Controller.Instance.shipType.baseFirepower,
                    Controller.Instance.shipType.baseDamage,
                    Controller.Instance.shipType.baseScale,
                    Controller.Instance.shipType.baseAim,
                    Controller.Instance.shipType.baseTurn,
                    Controller.Instance.shipType.baseEnginePerformance
                }
            ;

            float[] shipMultipliers =
                {
                    Controller.Instance.shipType.maxHitPointsMultiplier,
                    Controller.Instance.shipType.maxBatteryMultiplier,
                    Controller.Instance.shipType.batteryRechargeMultiplier,
                    Controller.Instance.shipType.cooldownMultiplier,
                    Controller.Instance.shipType.firepowerMultiplier,
                    Controller.Instance.shipType.damageMultiplier,
                    1, // Scale
                    1, // Aim
                    1, // Turn
                    Controller.Instance.shipType.enginePerformanceMultiplier
                }
            ;

            for (int i = 0; i < 10; i++)
                skillpointStatsValueList[i] = 0;
                
            for (int i = 0; i < shipStatsList.Count; i++)
                shipBaseStatsValueList.AddRange(baseStats);
            
            for (int i = 0; i < shipStatsList.Count; i++)
                statMultiplierValueList.AddRange(shipMultipliers);

            for (int i = 0; i < shipStatsList.Count; i++)
                shipStatsValueList[i] = ;
            
            for (int i = 0; i < shipStatsList.Count; i++)
                activeStatsValueList[i] = shipBaseStatsValueList[i] + ((shipStatsValueList[i] * statMultiplierValueList[i]) - shipBaseStatsValueList[i]) + (skillpointStatsValueList[i] * statMultiplierValueList[i]);
        }

        // public void UpgradeShipStat(ShipStats stat, float amount)
        // {
        //     for (int i = 0; i < shipStatsList.Count; i++)
        //         if (shipStatsList[i] == stat)
        //             activeStatsValueList[i] += amount;
        // }
        
        public float GetShipStats(ShipStats stat)
        {
            for (int i = 0; i < shipStatsList.Count; i++)
                if (shipStatsList[i] == stat)
                    return activeStatsValueList[i];

            return 0;
        }

        public float GetShipStats(ShipStats stat, StatOrigin origin)
        {
            for (int i = 0; i < shipStatsList.Count; i++)
                if (shipStatsList[i] == stat)
                    switch (origin)
                    {
                        case StatOrigin.Base:
                            return shipBaseStatsValueList[i];
                        case StatOrigin.Multiplicator:
                            return statMultiplierValueList[i];
                        case StatOrigin.Skillpoint:
                            return skillpointStatsValueList[i];
                        case StatOrigin.Upgrade:
                            return shipStatsValueList[i] - shipBaseStatsValueList[i];
                    }                    

            return 0;
        }
        
        
        public void UpgradeShipStats(ShipStats stat, StatOrigin origin, float value)
        {
            if (origin == StatOrigin.Base)
                return;
            
            if (origin == StatOrigin.Multiplicator)
                return;
                
            for (int i = 0; i < shipStatsList.Count; i++)
                if (shipStatsList[i] == stat)
                    switch (origin)
                    {
                        case StatOrigin.Skillpoint:
                            skillpointStatsValueList[i] += value;
                            break;
                        case StatOrigin.Upgrade:
                            shipStatsValueList[i] += value;
                            break;
                    }                    
        }
    }
}