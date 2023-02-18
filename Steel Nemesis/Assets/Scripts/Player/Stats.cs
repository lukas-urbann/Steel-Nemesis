using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using Shop;
using UnityEngine;

namespace Player
{
    public enum StatOrigin
    {
        Base,
        Upgrade,
        Skillpoint,
        Multiplicator,
        MaxStat
    }
    
    public class Stats : MonoBehaviour
    {
        [SerializeField] private RepairButton repairButton;
        
        public List<ShipStats> shipStatsList = new List<ShipStats>();
        [Tooltip("Staty:\n1. Max HP\n2. Max Battery\n3. Battery Recharge\n4. Cooldown\n5. Firepower\n6. Damage\n7. Scale\n8. Aim\n9. Turn\n10.Engine Perf.")]
        [SerializeField] private List<float> activeStatsValueList = new List<float>(); // Tohle je to co používá hráč aktivně, jsou v tom všecky sečtené
        private List<float> shipStatsValueList = new List<float>(); // Tohle je pouze loď
        private List<float> shipBaseStatsValueList = new List<float>(); // Tohle jsou pouze base staty lodě
        private List<float> skillpointStatsValueList = new List<float>(); // Tohle jsou pouze hráčovy skillpointy
        private List<float> statMultiplierValueList = new List<float>(); // Tohle je multiplikátor lodě
        private List<float> shipMaxStatsList = new List<float>(); // Tohle obsahuje maximální upgrady jednotlivých lodí
        private List<float> shipBestStatsValueList = new List<float>(); // Tohle obsahuje maximální upgrady jednotlivých lodí

        public delegate void AttributeDelegate(); // volat na shop pro update labelu
        public AttributeDelegate onShipUpgrade;
        
        private void OnEnable()
        {
            LoadStats();            
            AssignShipStats();
        }

        private void LoadStats()
        {
            shipStatsList.AddRange(Enum.GetValues(typeof(ShipStats)).Cast<ShipStats>());
            
            foreach (ShipStats stat in shipStatsList)
                Debug.Log("Loaded Ship Stat: " + stat);
        }

        private void ManuallyAssignShipStats()
        {
            ManuallyAssignMaxStatList();
            ManuallyAssignShipBaseStats();
            ManuallyAssignShipMultipliers();
            ManuallyAssignShipBestStats();
        }

        private void ManuallyAssignShipBaseStats()
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

            shipBaseStatsValueList.AddRange(baseStats);
        }
        
        private void ManuallyAssignShipBestStats()
        {
            float[] bestStats =
                {
                    Controller.Instance.shipType.bestMaxHitPoints,
                    Controller.Instance.shipType.bestMaxBattery,
                    Controller.Instance.shipType.bestBatteryRecharge,
                    Controller.Instance.shipType.bestCooldown,
                    Controller.Instance.shipType.bestFirepower,
                    Controller.Instance.shipType.bestDamage,
                    Controller.Instance.shipType.bestScale,
                    Controller.Instance.shipType.bestAim,
                    Controller.Instance.shipType.bestTurn,
                    Controller.Instance.shipType.bestEnginePerformance
                }
                ;

            shipBestStatsValueList.AddRange(bestStats);
        }

        private void ManuallyAssignShipMultipliers()
        {
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
            
            statMultiplierValueList.AddRange(shipMultipliers);
        }

        private void ManuallyAssignMaxStatList()
        {
            float[] maxStats =
                {
                    Controller.Instance.shipType.maxHitPointsStat,
                    Controller.Instance.shipType.maxBatteryStat,
                    Controller.Instance.shipType.maxBatteryRechargeStat,
                    Controller.Instance.shipType.maxCooldownStat,
                    Controller.Instance.shipType.maxFirepowerStat,
                    Controller.Instance.shipType.maxDamageStat,
                    1, // Scale
                    1, // Aim
                    1, // Turn
                    Controller.Instance.shipType.maxEnginePerformanceStat
                }
                ;
            
            shipMaxStatsList.AddRange(maxStats);
        }

        private void LoadStatsIntoLists()
        {
            for (int i = 0; i < shipStatsList.Count; i++)
                skillpointStatsValueList.Add(0);

            for (int i = 0; i < shipStatsList.Count; i++)
                shipStatsValueList.Add(shipBaseStatsValueList[i]);
        }
        
        public void AssignShipStats()
        {
            ManuallyAssignShipStats();
            LoadStatsIntoLists();
            UpdateShipStats();
        }

        public void UpdateShipStats()
        {
            if (activeStatsValueList.Count == shipStatsList.Count)
            {
                UpdateActiveSlots();
                return;
            }
            
            AssignActiveSlots();
        }

        private void UpdateActiveSlots()
        {
            for (int i = 0; i < shipStatsList.Count; i++)
            {
                activeStatsValueList[i] = (shipBaseStatsValueList[i] +                                                                                                                       
                                           ((shipStatsValueList[i] * statMultiplierValueList[i]) - shipBaseStatsValueList[i]) + (skillpointStatsValueList[i] * statMultiplierValueList[i]));
                                             
                if (activeStatsValueList[i] >= shipBestStatsValueList[i])
                    activeStatsValueList[i] = shipBestStatsValueList[i];
                    
            }
        }

        private void AssignActiveSlots()
        {
            for (int i = 0; i < shipStatsList.Count; i++)
                activeStatsValueList.Add(shipBaseStatsValueList[i] +
                                         ((shipStatsValueList[i] * statMultiplierValueList[i]) - shipBaseStatsValueList[i]) + (skillpointStatsValueList[i] * statMultiplierValueList[i]));
        }

        public float GetShipStats(ShipStats stat)
        {
            for (int i = 0; i < shipStatsList.Count; i++)
                if (shipStatsList[i] == stat)
                    return activeStatsValueList[i];

            return 0;
        }
        
        public float GetBestShipStats(ShipStats stat)        
        {                                                
            for (int i = 0; i < shipStatsList.Count; i++)
                if (shipStatsList[i] == stat)            
                    return shipBestStatsValueList[i];      
                                                 
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
                        case StatOrigin.MaxStat:
                            return shipMaxStatsList[i];
                    }                    

            return 0;
        }
        
        public void UpgradeShipStats(ShipStats stat, StatOrigin origin, float value)
        {
            //TODO: Možná přidat možnost vylepšit kapacitu lodí
            
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

                            if (stat == ShipStats.MaxHitPoints)
                            {
                                Controller.Instance.SetHitpoints(shipStatsValueList[i]);
                                repairButton.Disappear();
                            }
                            
                            break;
                    }

            UpdateShipStats();
            
            if(onShipUpgrade != null)
                onShipUpgrade.Invoke(); // Nevim jestli je dobrej napad to sem davat
        }
    }
}