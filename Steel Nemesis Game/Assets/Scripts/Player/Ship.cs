using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "Ship Type", menuName = "Ship Type", order = 1)]
    public class Ship : ScriptableObject
    {
        public string shipName;
        public Sprite shipSprite;
        
        [Header("Base Stats")]
        public float baseHitPoints = 20;
        public float baseMaxHitPoints = 20;
        public float baseBattery = 10;
        public float baseMaxBattery = 10;
        public float baseBatteryRecharge = 25;
        public float baseFireCost = 10;
        public float baseCooldown = 0.5f;
        public float baseFirepower = 0.75f;
        public float baseDamage = 12;
        public float baseEnginePerformance = 2f;
        public float baseAim = 2;
        public float baseTurn = 4;
        public float baseScale = 5;
        public float baseBulletSpread = 0.15f;
        
        [Header("Stat Multipliers")]
        public float baseDamageMultiplier = 20;

    }
}
