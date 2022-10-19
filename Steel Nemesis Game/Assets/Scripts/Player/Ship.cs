using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "Ship Type", menuName = "Ship Type", order = 1)]
    public class Ship : ScriptableObject
    {
        //Obsahuje jenom base staty pro typ lodě Cadet
        
        public string shipName;
        public Sprite shipSprite;
        
        [Header("Base Stats")]
        public float baseHitPoints = 20;
        public float baseMaxHitPoints = 20; //
        public float baseBattery = 10;
        public float baseMaxBattery = 10; //
        public float baseBatteryRecharge = 25; //
        public float baseCooldown = 0.5f; //
        public float baseFirepower = 0.75f; //
        public float baseDamage = 12;
        public float baseEnginePerformance = 2f;
        public float baseAim = 2;
        public float baseTurn = 4;
        public float baseScale = 5;
        
        [Header("Stat Multipliers")]
        public float maxHitPointsMultiplier = 1;
        public float maxBatteryMultiplier = 1;
        public float batteryRechargeMultiplier = 1;
        public float firepowerMultiplier = 1;
        public float cooldownMultiplier = 1;
        public float damageMultiplier = 1;
        public float enginePerformanceMultiplier = 1;
        public float costMultiplier = 1;
        
        [Header("Stat Max Levels")]
        public float maxHitPointsStat = 5;
        public float maxBatteryStat = 3;
        public float maxBatteryRechargeStat = 1;
        public float maxFirepowerStat = 3;
        public float maxCooldownStat = 5;
        public float maxDamageStat = 3;
        public float maxEnginePerformanceStat = 5;
    }
}
