using System;
using System.Collections;
using System.Collections.Generic;
using Other;
using Triggers;
using UnityEngine;

namespace Enemy
{
    public enum EnemyType
    {
        Basic,
        Attacker,
        Tank,
        Fighter,
        Kamikadze,
        Mothership
    }
    
    public enum FlyDirection
    {
        Top,
        Right,
        Down,
        Left
    }

   public class BasicEnemy : MonoBehaviour
    {
        protected float speed;
        protected float hp;
        protected float maxHp;
        protected float damage;

        protected FlyDirection flyDirection;
        public GameObject hitEffect;
        
        protected Vector3 movement = new Vector3(0, 0,0);

        [SerializeField] protected GameObject laser;
        [SerializeField] protected List<Transform> barrelPoints = new List<Transform>();

        private Death deathScript;

        private void OnEnable()
        {
            damage = 0;
            deathScript = transform.root.GetComponent<Death>();
        }

        protected virtual void InitMaxHP()
        {
            maxHp = hp;
        }

        protected void SetSpeed(float spd)
        {
            speed += spd;
        }

        protected virtual void ChangeDirection(FlyDirection side)
        {
            switch (side)
            {
                case FlyDirection.Down:
                    movement = new Vector2(0, -speed);
                    break;
                case FlyDirection.Left:
                    movement = new Vector2(-speed, 0);
                    break;
                case FlyDirection.Right:
                    movement = new Vector2(speed, 0);
                    break;
                case FlyDirection.Top:
                    movement = new Vector2(0, speed);
                    break;
                default:
                    break;
            }
        }

        protected virtual void CalculateFire()
        {
            
        }

        protected void Fire()
        {
            
        }

        private void Update()
        {
            
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Laser") && col.gameObject.layer == 7)
            {
                LaserHit();
                Instantiate(hitEffect, col.transform.position, Quaternion.identity);
                SetSpeed(-1f);
                Destroy(col.gameObject);
            }
        }

        protected virtual void LaserHit()
        {
            Controllers.Audio.Instance.HitSound();
            hp -= Player.Player.Instance.GetFireDamage();
            CheckHp();
        }

        public void InstaKill()
        {
            hp = 0;
            CheckHp();
        }

        protected virtual void CheckHp()
        {
            if (hp <= 0)
            {
                Controllers.Game.Instance.AddKill();
                Controllers.Wave.Instance.SetRemainingEnemies(-1);
                CustomDeath();
                deathScript.DeathEvent();
            }
        }

        protected virtual void CustomDeath()
        {
            Controllers.Score.Instance.AddScore((int) ((speed + maxHp/3 + damage) * Controllers.Wave.Instance.GetLevel()) );  
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Trigger"))
            {
                TypeOfTrigger trigger;
                
                try
                {
                    trigger = col.GetComponent<TriggerType>().triggerType;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                switch (trigger)
                {
                    case TypeOfTrigger.border:
                        switch (col.gameObject.layer)
                        {
                            case 3:
                                Debug.Log("Konec hry");
                                Controllers.Game.Instance.GameOver();
                                break;
                            case 10:
                                GetComponent<Boundaries>().enabled = false;
                                break;
                        }
                        break;
                }
            }
        }
    }
}
