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
        
        [Tooltip("Staty:\n1. Max HP\n2. Max Battery\n3. Battery Recharge\n4. Cooldown\n5. Firepower\n6. Damage\n7. Scale\n8. Aim\n9. Turn\n10.Engine Perf.")]
        [SerializeField] private List<float> activeStatsValueList = new List<float>(); // Tohle je to co používá hráč aktivně, jsou v tom všecky sečtené
        private List<float> shipStatsValueList = new List<float>(); // Tohle je pouze loď
        private List<float> shipBaseStatsValueList = new List<float>(); // Tohle jsou pouze base staty lodě
        private List<float> skillpointStatsValueList = new List<float>(); // Tohle jsou pouze hráčovy skillpointy
        private List<float> statMultiplierValueList = new List<float>(); // Tohle je multiplikátor lodě
        
        private List<PlayerStats> playerStatsList = new List<PlayerStats>();
        private List<float> playerStatsValueList = new List<float>();

        public delegate void AttributeDelegate(); // volat na shop pro update labelu
        public AttributeDelegate onShipUpgrade;
        
        private void OnEnable()
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

            for (int i = 0; i < shipStatsList.Count; i++)
                skillpointStatsValueList.Add(0);
                
            shipBaseStatsValueList.AddRange(baseStats);
            
            statMultiplierValueList.AddRange(shipMultipliers);

            for (int i = 0; i < shipStatsList.Count; i++)
                shipStatsValueList.Add(shipBaseStatsValueList[i]);
            
            UpdateShipStats();
        }

        public void UpdateShipStats()
        {
            onShipUpgrade.Invoke();
            
            for (int i = 0; i < shipStatsList.Count; i++)
                activeStatsValueList.Add(shipBaseStatsValueList[i] +
                                         ((shipStatsValueList[i] * statMultiplierValueList[i]) - shipBaseStatsValueList[i]) + (skillpointStatsValueList[i] * statMultiplierValueList[i]));
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
            
            onShipUpgrade.Invoke(); // Nevim jestli je dobrej napad to sem davat
                
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