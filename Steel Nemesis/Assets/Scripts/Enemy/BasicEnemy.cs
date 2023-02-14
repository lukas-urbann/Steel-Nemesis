using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Other;
using Triggers;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemy
{
    public enum EnemyDirection
    {
        X,
        Y,
        General
    }
    
    public enum EnemyType
    {
        Basic,
        Attacker,
        Tank,
        Fighter,
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
        //Hidden values
        [HideInInspector] private GameObject drop;
        protected FlyDirection flyDirection;
        private Vector3 movement = new Vector3(0, 0,0);
        private Death deathScript;

        [Header("--- Base enemy stats ---")]
        [SerializeField] protected float speed;
        protected float xSpeed;
        protected float ySpeed;
        [SerializeField] protected float hp;
        [SerializeField] protected float maxHp;
        [SerializeField] protected float damage;
        [SerializeField] protected float reloadTimeLeft;
        [SerializeField] protected EnemyType type;
        
        [Header("--- Advanced features ---")]
        [SerializeField] protected bool lockedInView = false;
        [Tooltip("Zaplé - loď má defaultní hodnoty\nVyplé - loď se vylepšuje růstem vlivem úrovně")]
        [SerializeField] protected bool defaultShipValues = false;
        [SerializeField] protected bool slowOnHit = false;
        [SerializeField] protected bool smoothSpeed = false;
        [SerializeField] protected bool canShoot = true;
        [SerializeField] protected bool isColliding = false;
        [SerializeField] protected bool disabledBoundaries = false;
        [SerializeField] protected bool visible = true;

        [Header("--- Additional features ---")]
        [SerializeField] protected int minWave;
        [SerializeField] protected int maxWave;
        
        [Header("--- Drop capabilities ---")]
        [SerializeField] protected int[] dropValues;
        
        [Header("--- Ship sound effects ---")]
        [SerializeField] protected AudioClip fireSound;
        
        [Header("--- Assignable objects ---")]
        [SerializeField] protected GameObject bulletPrefab;
        public GameObject damageEffectPrefab;

        [Header("--- Cannon barrels ---")]
        [SerializeField] protected List<Transform> barrelPoints = new List<Transform>();
        
        //NEGR
        public Vector3 oldPosition;     
        public Vector3 newPosition;
        public int positionOldX, positionNewX, positionOldY, positionNewY;
        public bool canDebilTurn = true, canDebilTurn2 = true;

        private void OnEnable()
        {
            oldPosition = transform.position;
            damage = 0;
            deathScript = transform.root.GetComponent<Death>();
        }

        protected void InitMaxHp()
        {
            maxHp = hp;
        }

        protected void SetDropValues(params int[] drops)
        {
            dropValues = new int[drops.Length];

            if(dropValues.Length == 0 || dropValues == null)
                return;
                
            for (int i = 0; i < dropValues.Length; i++)
            {
                dropValues[i] = drops[i];
            }
        }

        protected float GetVelocity(EnemyDirection direction)
        {
            newPosition = transform.position;
            var media =  (newPosition - oldPosition);
            Vector3 velocity = media /Time.deltaTime;
            oldPosition = newPosition;
            newPosition = transform.position;

            switch (direction)
            {
                case EnemyDirection.X:
                    return velocity.x;
                case EnemyDirection.Y:
                    return velocity.y;
                default:
                    return velocity.x + velocity.y + velocity.z;
            }
        }

        public void CheckDement()
        {
            Debug.LogWarning("dement 1");
            positionOldX = (int) (oldPosition.x * 10);
            positionNewX = (int) (newPosition.x * 10);

            if (positionOldX == positionNewX && canDebilTurn && GetVelocity(EnemyDirection.X) < 0.1f)
            {
                canDebilTurn = false;
                StartCoroutine(ChangeDirectionAfterStuck());
            }
        }

        public void CheckDement2()
        {
            Debug.LogWarning("dement y1");
            positionOldY = (int) (oldPosition.y * 10);
            positionNewY = (int) (newPosition.y * 10);

            if (positionOldY == positionNewY && canDebilTurn2 && GetVelocity(EnemyDirection.Y) < 0.1f)
            {
                canDebilTurn2 = false;
                StartCoroutine(ChangeDirectionAfterStuckY());
            }
        }
        
        private IEnumerator ChangeDirectionAfterStuckY()
        {
            Debug.LogWarning("dement y2");

            if (!canDebilTurn2)
            {
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            if (positionOldY == positionNewY)
            {
                canDebilTurn2 = false;
                SetNewRandomSpeed(-ySpeed / 2, -ySpeed, EnemyDirection.Y);
                Debug.LogWarning("dement y3");
                StartCoroutine(RETARD2());
            }
        }

        private IEnumerator ChangeDirectionAfterStuck()
        {
            Debug.LogWarning("dement 2");

            if (!canDebilTurn)
            {
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            if (positionOldX == positionNewX)
            {
                canDebilTurn = false;
                SetNewRandomSpeed(-xSpeed / 2, -xSpeed, EnemyDirection.X);
                Debug.LogWarning("dement 3");
                StartCoroutine(RETARD());
            }
        }
        
        private IEnumerator RETARD2()
        {
            yield return new WaitForSeconds(0.5f);
            canDebilTurn2 = true;
        }

        private IEnumerator RETARD()
        {
                yield return new WaitForSeconds(0.5f);
                canDebilTurn = true;
        }

        private void SetSpeed(float spd)
        {
            speed += spd;
            ChangeDirection(flyDirection);
        }

        protected void SetNewRandomSpeed(float minRange, float maxRange, EnemyDirection direction)
        {
            switch (direction)
            {
                case EnemyDirection.X:
                    xSpeed = Random.Range(minRange, maxRange);
                    break;
                case EnemyDirection.Y:
                    ySpeed = Random.Range(minRange, maxRange);
                    break;
            }
        }

        //Asi legacy
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
                enemyLaser = Instantiate(bulletPrefab, barrel.position, gameObject.transform.rotation);
                enemyLaser.GetComponent<Enemy.Laser>().SetDamage(damage);
                Rigidbody2D rb = enemyLaser.GetComponent<Rigidbody2D>();
                rb.AddForce(barrel.up * -1, ForceMode2D.Impulse);
            }
            
            Audio.Instance.PlaySound(fireSound);
        }

        protected void Fly()
        {
            transform.position += movement * (Time.deltaTime * 2);

            if (disabledBoundaries)
                ChangeDirection(FlyDirection.Down);
            
            if (smoothSpeed)
                SmoothSpeed();
                
        }

        private void SmoothSpeed()
        {
            movement.x = Mathf.Lerp(movement.x, xSpeed, 3f * Time.deltaTime);
            movement.y = Mathf.Lerp(movement.y, ySpeed, 3f * Time.deltaTime);
        }
        
        protected void FaceDown(float speed)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(0, Vector3.forward), speed); 
        }

        protected void FacePlayer(float speed)
        {
            if (Player.Controller.Instance == null)
                FaceDown(this.speed);
            
            Vector3 vectorToTarget;
            float angle;
            Quaternion qt;

            vectorToTarget = Player.Controller.Instance.transform.position - transform.position;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            qt = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            
            if(!Controllers.Pause.Instance.GetPauseState())
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qt, speed ); 
        }
        
        protected float GetPlayerDistance()
        {
            return Vector2.Distance(new Vector2(transform.position.x, transform.position.y),
                new Vector2(Player.Controller.Instance.transform.position.x,
                    Player.Controller.Instance.transform.position.y));
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Laser") && col.gameObject.layer == 7)
            {
                LaserHit();
                Instantiate(damageEffectPrefab, col.transform.position, Quaternion.identity);
                
                if(slowOnHit)
                    SetSpeed(-(speed - (speed / 2)));
                
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
                    flyDirection = FlyDirection.Down;
                    break;
            }
            
            ChangeDirection(flyDirection);
        }

        protected virtual void LaserHit()
        {
            Controllers.Audio.Instance.HitSound();
            hp -= Player.Controller.StatsInstance.GetShipStats(ShipStats.Damage);
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
                Other.CameraShake.Instance.ShakeScreen(0.1f, 0);
                Controllers.Game.Instance.AddKill();
                //Controllers.Wave.Instance.SetRemainingEnemies(-1);
                
                for (int i = 0; i < Random.Range(1, 6); i++)
                    Instantiate(Controllers.Prefabs.Instance.scrap, transform.position, Quaternion.identity);
                
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
            
           int selected = dropValues[Random.Range(0, dropValues.Length)];

           if (selected-1 == -1) return;

           drop = Drops.Instance.GetDrop(selected);

           if (drop == null)
               return;
           
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
                                CameraShake.Instance.ShakeScreen(0.5f, 1.2f);
                                Controllers.Barrier.Instance.ChangeHitpoints(-1);
                                //Controllers.Wave.Instance.SetRemainingEnemies(-1);
                                deathScript.DeathEvent();
                                break;
                            case 10:
                                if (!lockedInView)
                                {
                                    disabledBoundaries = true;
                                    GetComponent<Boundaries>().enabled = false;
                                }
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
