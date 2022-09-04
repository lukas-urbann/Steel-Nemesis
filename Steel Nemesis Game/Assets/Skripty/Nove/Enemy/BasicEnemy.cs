using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
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
        protected bool isColliding = false;

        protected FlyDirection flyDirection;
        public GameObject hitEffect;

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

        protected void ChangeDirection(FlyDirection side)
        {
            
        } 

        protected void Fire()
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

        protected void DropConsumable()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(isColliding) return;
            isColliding = true;
            
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
                                Barrier.Instance.ChangeHitpoints(-1);
                                Controllers.Wave.Instance.SetRemainingEnemies(-1);
                                deathScript.DeathEvent();
                                break;
                            case 10:
                                GetComponent<Boundaries>().enabled = false;
                                break;
                        }
                        break;
                }
            }
            
            StartCoroutine(Reset());
        }
        
        IEnumerator Reset()
        {
            yield return new WaitForEndOfFrame();
            isColliding = false;
        }
    }
}
