using UnityEngine;
using System;
using System.Collections;

namespace Controllers
{
    public enum ShipStats
    {
        HitPoints,
        Battery,
        BatteryRecharge,
        Cooldown,
        Firepower,
        Damage,
        Scale,
        Aim,
        Turn,
        EnginePerformance,
    }

    public enum PlayerStats
    {
        XP,
        Level,
        Score,
        Credits,
        CadetPoints,
        RepairCoupons,
        SkillPoints,
    }
    
    
    public class Stats : MonoBehaviour
    {
        //Managing stats
        //ShipStats.Aim.ToString() - get name
        //TODO: enemy share
    }
}