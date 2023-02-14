using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class Fighter : BasicEnemy
    {
        private Rigidbody2D rb;
        private IEnumerator speedChanger;
       
        private void Start()
        {
            SetDropValues(3,4,5,6,7,8);
            
            rb = GetComponent<Rigidbody2D>();
            hp = defaultShipValues ? 70 : (70 + (Controllers.Wave.Instance.GetLevel() * 1f));
            speed = defaultShipValues ? 2 : (2f + (Controllers.Wave.Instance.GetLevel() * 0.005f));
            
            InitMaxHp();
            //StartCoroutine(ChangeDirections(2f));
            speedChanger = ChangeDirection();
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
            CheckDement();
            CheckDement2();
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
            
            /*
            if(hp < maxHp / 4)
                ChangeDirectionRandomly();
                */
        }
        
        

        private void FlyTowardsPlayer()
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.Controller.Instance.gameObject.transform.position, speed * (Time.deltaTime * 2));
        }
    }
}