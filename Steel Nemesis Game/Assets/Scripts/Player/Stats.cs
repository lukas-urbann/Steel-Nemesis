using System;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Player
{
    public class Stats : MonoBehaviour
    {
        private string asd = "4.4";
    
        private float maxHitPoints = 0;
        private float maxBattery = 0;
        private float batteryRecharge = 0;
        private float fireCost = 0;
        private float cooldown = 0;
        private float firepower = 0;
        private float damage = 0;
        private float enginePerformance = 0;
        private float aim = 0;
        private float turn = 0;
        private float scale = 0;
        private float bulletSpread = 0;
        
        public void UpgradeStatCall(ShipStats stat, float amount)
        {
            UpgradeStat(stat, amount);
        }

        private void UpgradeStat(ShipStats stat, float amount)
        {
            switch (stat)
            {
                case ShipStats.Aim:
                    aim += amount;
                    break;
                case ShipStats.Battery:
                    maxBattery += amount;
                    break;
                case ShipStats.BatteryRecharge:
                    batteryRecharge += amount;
                    break;
                case ShipStats.Cooldown:
                    cooldown += amount;
                    break;
                case ShipStats.Damage:
                    damage += amount;
                    break;
                case ShipStats.Firepower:
                    firepower += amount;
                    break;
                case ShipStats.Turn:
                    turn += amount;
                    break;
                case ShipStats.Scale:
                    scale += amount;
                    break;
                case ShipStats.EnginePerformance:
                    enginePerformance += amount;
                    break;
                case ShipStats.HitPoints:
                    maxHitPoints += amount;
                    break;
            }
        }

        public float GetSkillpointStat(ShipStats stat)
        {
            switch (stat)
            {
                case ShipStats.Aim:
                    return aim;
                case ShipStats.Battery:
                    return maxBattery;
                case ShipStats.Cooldown:
                    return cooldown;
                case ShipStats.Damage:
                    return damage;
                case ShipStats.Firepower:
                    return firepower;
                case ShipStats.Scale:
                    return scale;
                case ShipStats.Turn:
                    return turn;
                case ShipStats.BatteryRecharge:
                    return batteryRecharge;
                case ShipStats.EnginePerformance:
                    return enginePerformance;
                case ShipStats.HitPoints:
                    return maxHitPoints;
            }

            return 0;
        }
    }
}