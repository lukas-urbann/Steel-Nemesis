using System;
using System.Collections;
using System.Collections.Generic;
using Other;
using Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class Tank : BasicEnemy
    {
        private bool attacking = false;

        private void Start()
        {
            hp = 120;
            
            if(speed < 3.5f)
                speed = 0.35f + (Controllers.Wave.Instance.GetLevel() * 0.005f);
            
            flyDirection = FlyDirection.Down;
            ChangeDirection(flyDirection);
            
            InitMaxHp();
            StartCoroutine(AttackCooldown());
        }

        private void Update()
        {
            Fly();

            if (attacking && visible)
            {
                FacePlayer(2f);
                Shoot(2f);
            }
            else
            {
                flyDirection = FlyDirection.Down;
                ChangeDirection(flyDirection);
                FaceDown(2);
            }
        }

        private IEnumerator Attack()
        {
            yield return new WaitForSeconds(Random.Range(1,3));
            attacking = true;
            StartCoroutine(AttackCooldown());
        }

        private IEnumerator AttackCooldown()
        {
            yield return new WaitForSeconds(Random.Range(3, 5));
            attacking = false;
            StartCoroutine(Attack());
        }
    }
}