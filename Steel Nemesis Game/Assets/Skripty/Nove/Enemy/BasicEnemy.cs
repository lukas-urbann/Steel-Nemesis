using System;
using System.Collections;
using System.Collections.Generic;
using Other;
using Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

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
       [HideInInspector] protected GameObject drop;
        protected float speed;
        protected float hp;
        protected float maxHp;
        protected float damage;
        protected float reloadTimeLeft;
        protected bool canShoot = true;
        protected bool isColliding = false;
        [SerializeField] protected int minWave, maxWave;
        protected bool visible = true;
        
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
            }
        }

        protected void Shoot(float reloadTime)
        {
            if (!canShoot)
                return;
            
            canShoot = false;
                
            if(reloadTimeLeft >= 2)
                reloadTimeLeft = Random.Range(reloadTime - 2f, reloadTime + 2f);
            else
                reloadTimeLeft = Random.Range(0, reloadTime);

            StartCoroutine(CalculateFireCooldown(reloadTimeLeft));
        }

        protected IEnumerator CalculateFireCooldown(float time)
        {
            yield return new WaitForSeconds(time);
            canShoot = true;
            Fire();
        }
        
        protected void Fire()
        {
            GameObject enemyLaser;
            
            foreach (Transform barrel in barrelPoints)
            {
                enemyLaser = Instantiate(laser, barrel.position, gameObject.transform.rotation);
                enemyLaser.GetComponent<Enemy.Laser>().SetDamage(damage);
                Rigidbody2D rb = enemyLaser.GetComponent<Rigidbody2D>();
                rb.AddForce(barrel.up * -1, ForceMode2D.Impulse);
            }
        }

        protected void Fly()
        {
            transform.position += movement * (Time.deltaTime * 2);
        }

        protected void FaceDown(float speed)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(0, Vector3.forward), speed ); 
        }

        protected void FacePlayer(float speed)
        {
            Vector3 vectorToTarget;
            float angle;
            Quaternion qt;
            
            vectorToTarget = Player.Player.Instance.transform.position - transform.position;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            qt = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            
            if(!Controllers.Pause.Instance.GetPauseState())
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qt, speed ); 
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

        protected IEnumerator ChangeDirections(float seconds)
        {
            Debug.Log("IEnumerator change dirs");
            ChangeDirectionRandomly();
            yield return new WaitForSeconds(Random.Range(seconds-1, seconds+1));
            StartCoroutine(ChangeDirections(Random.Range(seconds - 1, seconds + 1)));
        }

        protected void ChangeDirectionRandomly()
        {
            int[] random = new[] { 0, 1, 2, 3 };
            int selected = random[Random.Range(0, random.Length)];

            switch (selected)
            {
                case 0:
                    flyDirection = FlyDirection.Top;
                    break;
                case 1:
                    flyDirection = FlyDirection.Right;
                    break;
                case 2:
                    flyDirection = FlyDirection.Down;
                    break;
                case 3:
                    flyDirection = FlyDirection.Left;
                    break;
                default:
                    Debug.Log("wtf");
                    flyDirection = FlyDirection.Top;
                    break;
            }
            
            ChangeDirection(flyDirection);
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
                DropConsumable();
                deathScript.DeathEvent();
            }
        }
        
        protected void OnBecameInvisible()
        {
            visible = false;
        }

        protected virtual void CustomDeath()
        {
            Controllers.Score.Instance.AddScore((int) ((speed + maxHp/3 + damage) * Controllers.Wave.Instance.GetLevel()) );  
        }
        
        protected void DropConsumable()
        {
           int[] values = {0,0,1,0,4,0,0,3,0,6,0,0,7,0,0,5,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
           int selected = values[Random.Range(0, values.Length)];

           if (selected == 0) return;

           drop = Drops.Instance.GetDrop(selected);
           Instantiate(drop, transform.position, Quaternion.identity);
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
                                Controllers.Barrier.Instance.ChangeHitpoints(-1);
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
        
        private IEnumerator Reset()
        {
            yield return new WaitForEndOfFrame();
            isColliding = false;
        }

        public int GetMinLevel()
        {
            return minWave;
        }
        
        public int GetMaxLevel()
        {
            return maxWave;
        }
    }
}
