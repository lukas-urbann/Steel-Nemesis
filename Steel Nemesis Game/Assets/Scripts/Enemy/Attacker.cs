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
            hp = 50;
            
            if(speed < 6.5f)
                speed = 0.5f + (Controllers.Wave.Instance.GetLevel() * 0.05f);

            damage = 20 + (Controllers.Wave.Instance.GetLevel() * 1);
            
            flyDirection = FlyDirection.Down;
            ChangeDirection(flyDirection);
            
            InitMaxHP();
        }

        private void Update()
        {
            Fly();

            if (visible)
            {
                Shoot(3);
            
                if(hp <= (maxHp / 2))
                    FacePlayer(2); 
            }
        }
    }
}