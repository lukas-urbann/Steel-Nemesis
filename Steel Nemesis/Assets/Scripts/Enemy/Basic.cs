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
            dropValues = new int[] { 0,0,1,0,0,1,0,1,2,0,0,0,1,0 };

            hp = 22.5f + (Controllers.Wave.Instance.GetLevel() * 0.5f);

            if(speed < 5.5f)
                speed = 0.75f + (Controllers.Wave.Instance.GetLevel() * 0.05f);
            
            flyDirection = FlyDirection.Down;
            ChangeDirection(flyDirection);
            
            InitMaxHp();
        }

        private void Update()
        {
            Fly();
        }
    }
}
