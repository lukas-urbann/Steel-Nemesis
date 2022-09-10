using System;
using System.Collections;
using System.Collections.Generic;
using Laser;
using UnityEngine;

namespace Enemy
{
    public class Basic : BasicEnemy
    {
        private void Start()
        {
            hp = 50;

            if(speed < 5.5f)
                speed = 1 + (Controllers.Wave.Instance.GetLevel() * 0.05f);
            
            flyDirection = FlyDirection.Down;
            ChangeDirection(flyDirection);
            
            InitMaxHP();
        }

        private void Update()
        {
            Fly();
        }

        protected override void LaserHit()
        {
            base.LaserHit();
        }
    }
}
