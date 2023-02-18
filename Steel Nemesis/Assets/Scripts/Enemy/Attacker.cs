using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class Attacker : Enemy
    {
        protected override void EarlyStart()
        {
            
        }

        protected override void AssignScriptShipValues()
        {
            SetDropValues(0,0,1,1,1,2);
            hp = defaultShipValues ? 30 : (30 + (Controllers.Wave.Instance.GetLevel() * 1f));
            speed = defaultShipValues ? 1 : (1f + (Controllers.Wave.Instance.GetLevel() * 0.005f));
            damage = 5;
        }

        private void Start()
        {
            StartCoroutine(ChangeDirection());
        }
        
        private IEnumerator ChangeDirection()
        {
            float timer = Random.Range(0f, 1.5f);

            float spd = Random.Range(speed / 2, speed);
            
            switch (Mathf.FloorToInt(Random.Range(0.001f, 1.999f)))
            {
                case 0:
                    switch (Mathf.FloorToInt(Random.Range(0.001f, 1.999f)))
                    {
                        case 0:
                            SetNewRandomSpeed(spd,speed, EnemyDirection.X);
                            break;
                        case 1:
                            SetNewRandomSpeed(-spd,-speed, EnemyDirection.X);
                            break;
                    }
                    break;
                case 1:
                    switch (Mathf.FloorToInt(Random.Range(0.001f, 1.999f)))
                    {
                        case 0:
                            SetNewRandomSpeed(spd,speed, EnemyDirection.Y);
                            break;
                        case 1:
                            SetNewRandomSpeed(-spd,-speed, EnemyDirection.Y);
                            break;
                    }
                    break;
            }
            yield return new WaitForSeconds(timer);
            debil();
        }

        private void debil()
        {
            StopCoroutine(ChangeDirection());
            StartCoroutine(ChangeDirection());
        }

        private void Update()
        {
            CheckStuckX();
            CheckStuckY();
            GetVelocity(EnemyDirection.X);
            Fly();

            if (visible)
            {
                FacePlayer(3); 
                Shoot(2.5f);
            }
            else
            {
                FaceDown(3);
            }
        }
    }
}