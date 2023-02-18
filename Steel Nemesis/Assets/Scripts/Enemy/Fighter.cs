using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class Fighter : Enemy
    {
        protected override void EarlyStart()
        {
            
        }

        protected override void AssignScriptShipValues()
        {
            SetDropValues(3,4,5,6,7,8);
            hp = defaultShipValues ? 70 : (70 + (Controllers.Wave.Instance.GetLevel() * 1f));
            speed = defaultShipValues ? 2 : (2f + (Controllers.Wave.Instance.GetLevel() * 0.005f));
            damage = 8;
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
            
            if (GetPlayerDistance() < 2.5f)
                FlyTowardsPlayer();
            else
                Fly();

            if (visible)
            {
                Shoot(2.5f); 
                FacePlayer(5);
            }
            else
            {
                FaceDown(5);
                flyDirection = FlyDirection.Down;
                ChangeDirection(flyDirection);
            }
        }

        private void FlyTowardsPlayer()
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.Controller.Instance.gameObject.transform.position, speed * (Time.deltaTime * 2));
        }
    }
}