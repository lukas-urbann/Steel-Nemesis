using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Other;
using Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    /// <summary>
    /// Nový systém letu. Obsahuje označení os.
    /// </summary>
    public enum EnemyDirection
    {
        X,
        Y,
        General
    }
    
    /// <summary>
    /// Typy nepřátel zaznamenané v enumech.
    /// </summary>
    public enum EnemyType
    {
        Standard,
        Attacker,
        Tank,
        Fighter,
    }
    
    /// <summary>
    /// Legacy systém letu. Obsahuje možné směry letu.
    /// </summary>
    public enum FlyDirection
    {
        Top,
        Right,
        Down,
        Left
    }

   public abstract class Enemy : MonoBehaviour
   {
       #region Proměnné

       //General - Hidden values
       private GameObject drop;
       protected FlyDirection flyDirection;
       private Vector3 movement;
       private Death deathScript;
       private float maxHp;
       protected bool visible = true;
       private bool isColliding;
       private float reloadTimeLeft;
       private float xSpeed;
       private float ySpeed;

       //AI Location - Hidden values
       private Vector3 oldPosition;     
       private Vector3 newPosition;
       private int positionOldX, positionNewX, positionOldY, positionNewY;
       private bool canChangeXSpeed = true;
       private bool canChangeYSpeed = true;
       
       [Header("--- Base enemy stats ---")]
       [SerializeField] protected EnemyType type;
       [SerializeField] protected float speed;
       [SerializeField] protected float hp;
       [SerializeField] protected float damage;
       [Header("--- Advanced features ---")]
       [SerializeField] protected bool lockedInView;
       [Tooltip("Zaplé - loď má defaultní hodnoty\nVyplé - loď se vylepšuje růstem vlivem úrovně")] [SerializeField]
       protected bool defaultShipValues;
       [Tooltip("Zaplé - loď má hodnoty z inspectoru\nVyplé - loď používá hodnoty ze skriptu.")] [SerializeField]
       protected bool inspectorValues;
       [SerializeField] protected bool slowOnHit;
       [SerializeField] protected bool smoothSpeed;
       [SerializeField] protected bool canShoot = true;
       [SerializeField] protected bool disabledBoundaries;

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

       #endregion

       #region Inicializace

        /// <summary>
        /// Spouští úvodní metody
        /// </summary>
        private void OnEnable()
        {
            EarlyStart();
            SetShipValues();
            AssignScripts();
            InitMaxHp();
        }
        
        /// <summary>
        /// Inicializuje se před startem, je potřeba zde definovat věci, se kterými se
        /// pracuje ve Startu a OnEnable. Nejčastěji proměnné lodě.
        /// </summary>
        protected abstract void EarlyStart();

        /// <summary>
        /// Zde se píší základní hodnoty lodí pro případy, kdy by nebyly přiřazené v inspectoru.
        /// </summary>
        protected abstract void AssignScriptShipValues();
        
        /// <summary>
        /// Přiřazuje nějaké základní věci při spuštění, slouží spíše k čistějšímu kódu.
        /// </summary>
        private void AssignScripts()
        {
            oldPosition = transform.position;
            deathScript = transform.root.GetComponent<Death>();
        }

        /// <summary>
        /// Nastaví maximální HP nepřítele dle HP se kterými se spawne
        /// </summary>
        private void InitMaxHp()
        {
            maxHp = hp;
        }

        /// <summary>
        /// Každá loď má své defaultní hodnoty definované ve skriptech. Pokud je 'inspectorValues' zaškrtlé, tak
        /// loď bere své hodnoty právě z inspektoru.
        /// </summary>
        private void SetShipValues()
        {
            if (!inspectorValues)
            {
                AssignScriptShipValues();
                return;
            }
            
            SetDropValues(dropValues);
            hp = defaultShipValues ? hp : (hp + Controllers.Wave.Instance.GetLevel() * 1f);
            speed = defaultShipValues ? speed : (speed + (Controllers.Wave.Instance.GetLevel() * 0.005f));
            damage = defaultShipValues ? damage : (damage + (Controllers.Wave.Instance.GetLevel() * 0.1f));
        }

        /// <summary>
        /// Nastaví hodnoty dropů nepřátel dle ID
        /// </summary>
        /// <param name="drops">Jednotlivé čísla ID dropů</param>
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

        #endregion
        
       #region Ovládání letu nepřítele
        
        protected float GetVelocity(EnemyDirection direction)
        {
            newPosition = transform.position;
            var media =  (newPosition - oldPosition);
            Vector3 velocity = media / Time.deltaTime;
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
        
        protected void CheckStuckX()
        {
            positionOldX = (int) (oldPosition.x * 10);
            positionNewX = (int) (newPosition.x * 10);

            if (positionOldX == positionNewX && canChangeXSpeed && GetVelocity(EnemyDirection.X) < 0.1f)
            {
                canChangeXSpeed = false;
                StartCoroutine(ChangeDirectionAfterStuck());
            }
        }
        
        protected void CheckStuckY()
        {
            positionOldY = (int) (oldPosition.y * 10);
            positionNewY = (int) (newPosition.y * 10);

            if (positionOldY == positionNewY && canChangeYSpeed && GetVelocity(EnemyDirection.Y) < 0.1f)
            {
                canChangeYSpeed = false;
                StartCoroutine(ChangeDirectionAfterStuckY());
            }
        }
        
        private IEnumerator ChangeDirectionAfterStuckY()
        {
            if (!canChangeYSpeed)
                yield return null;

            yield return new WaitForSeconds(1f);

            if (positionOldY == positionNewY)
            {
                canChangeYSpeed = false;
                SetNewRandomSpeed(-ySpeed / 2, -ySpeed, EnemyDirection.Y);
                StartCoroutine(EnableCanChangeYSpeed());
            }
        }
        
        private IEnumerator ChangeDirectionAfterStuck()
        {
            if (!canChangeXSpeed)
                yield return null;

            yield return new WaitForSeconds(1f);

            if (positionOldX == positionNewX)
            {
                canChangeXSpeed = false;
                SetNewRandomSpeed(-xSpeed / 2, -xSpeed, EnemyDirection.X);
                StartCoroutine(EnableCanChangeXSpeed());
            }
        }
        
        private IEnumerator EnableCanChangeYSpeed()
        {
            yield return new WaitForSeconds(0.5f);
            canChangeYSpeed = true;
        }
        
        private IEnumerator EnableCanChangeXSpeed()
        {
            yield return new WaitForSeconds(0.5f);
            canChangeXSpeed = true;
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
        #endregion

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

        private IEnumerator CalculateFireCooldown(float time)
        {
            yield return new WaitForSeconds(time);
            canShoot = true;
            Fire();
        }

        private void Fire()
        {
            foreach (var barrel in barrelPoints)
            {
                var enemyLaser = Instantiate(bulletPrefab, barrel.position, gameObject.transform.rotation);
                enemyLaser.GetComponent<Laser>().damage = damage;
                var rb = enemyLaser.GetComponent<Rigidbody2D>();
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

        private void ChangeDirectionRandomly()
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

        private void CheckHp()
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
            if(!lockedInView)
                visible = false;
        }

        protected virtual void CustomDeath()
        {
            Controllers.Score.Instance.AddScore((int) ((speed + maxHp/3 + damage) * Controllers.Wave.Instance.GetLevel()) );  
        }

        private void DropConsumable()
        {
            try
            {
                if (dropValues?.Length == 0 || dropValues == null)
                    return;
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogWarning("Rozbitý enemy consumable drop script. " + e.Message);
                return;
            }
            
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
