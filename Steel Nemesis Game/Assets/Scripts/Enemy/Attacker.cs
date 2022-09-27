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
            dropValues = new int[] { 0, 0, 1, 0, 0};
            
            hp = 50;
            
            if(speed < 6.5f)
                speed = 0.5f + (Controllers.Wave.Instance.GetLevel() * 0.05f);

            damage = 20 + (Controllers.Wave.Instance.GetLevel() * 1);
            
            flyDirection = FlyDirection.Down;
            ChangeDirection(flyDirection);
            
            InitMaxHp();
        }

        private void Update()
        {
            Fly();

            if (visible)
            {
                if (hp <= (maxHp / 2))
                {
                    FaceDown(3);
                }
                else
                {
                    FacePlayer(3); 
                    Shoot(2.5f);
                }
            }
        }
    }
}