using System;
using System.Collections;
using System.Collections.Generic;
using Other;
using Triggers;
using UnityEngine;

namespace Enemy
{
    public class Attacker : BasicEnemy
    {
        private void Start()
        {
            hp = 100;
            
            if(speed < 5.5f)
                speed = 1 + (Controllers.Wave.Instance.GetLevel() * 0.05f);
            
            flyDirection = FlyDirection.Down;
            
            InitMaxHP();
        }
        
        

        protected override void LaserHit()
        {
            base.LaserHit();
        }
    }
}