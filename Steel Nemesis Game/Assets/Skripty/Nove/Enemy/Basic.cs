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
            base.minWave = 0;
            base.maxWave = 50;
            hp = 22.5f + (Controllers.Wave.Instance.GetLevel() * 0.5f);

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
    }
}
