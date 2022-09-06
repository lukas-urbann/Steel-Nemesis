using System;
using System.Collections;
using System.Collections.Generic;
using Laser;
using UnityEngine;

namespace Enemy
{
    public class Basic : BasicEnemy
    {
        private Vector3 move;

        private void Start()
        {
            hp = 22.5f + (Controllers.Wave.Instance.GetLevel() * 0.5f);

            if(speed < 5.5f)
                speed = 1 + (Controllers.Wave.Instance.GetLevel() * 0.05f);
            
            move = new Vector3(0, -speed, 0);
            flyDirection = FlyDirection.Down;
            
            InitMaxHP();
        }

        private void Update()
        {
            switch (flyDirection)
            {
                case FlyDirection.Down:
                    transform.position += move * (Time.deltaTime * 2);
                    break;
            }
        }

        protected override void LaserHit()
        {
            base.LaserHit();
        }
    }
}
