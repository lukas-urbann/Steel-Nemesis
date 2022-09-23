using System.Collections;
using Collectible;
using Controllers;
using Enemy;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        public static Controller Instance;
        
        //Player Staty
        private int xp = 0;
        private int level = 1;
        
        
        //Ship Staty
        private float hitPoints = 100;
        private float maxHitPoints = 100;
        
        private float battery = 50;
        private float maxBattery = 50;
        
        private float batteryRecharge = 10;
        private float fireCost = 10;
        private float cooldown = 0.5f;
        private float firepower = 1;
        private float damage = 25;
        private float enginePerformance = 3; 
        private float aim = 5;
        private float turn = 5;
        private float scale = 5;
        
        private bool canFire = true; // 
        private bool isColliding = false; //Pro práci s kolizemi s triggery
        
        /*
         * Upgradování statů funguje 0-10
         * --------
         * Jednotlivé staty časem "korodují", to ovšem neznamená
         * že se horší přímo, ale jednoduše se časem stávají čím
         * dal tím slabšími oproti ostatním statům. Pro příklad, čím
         * více životů bude hráč mít, tím větší bude jeho lodď a tím
         * pádem bude potřeba silnější motor a jeho vylepšení. Hráč
         * bude muset kontrolovat dohromady s tím velikost jeho lodě.
         */
        
        //Záleží na tom v rotaci za kurzorem, jinak ignorovat
        private Vector3 vectorToTarget;
        private float angle;
        private Quaternion qt;

        [Header("-- Assignable --")]
        [Header("Game Objects")]
        public GameObject crosshair;
        public GameObject shipFlame;
        public GameObject laser;
        public GameObject playerHurtEffect;

        
        [Header("Transforms")]
        public Transform firePoint;
        
        [Header("AudioClips")]
        public AudioClip fire;
        public AudioClip pickup;
        public AudioClip playerHurt;

        

        
        [Header("Notification Pickup")]
        public TMP_Text notificationText;
        
        [SerializeField] private Color positive, negative;

        public float GetCrosshairSpeed()
        {
            return aim;
        }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        private void Start()
        {
            SetProportions();
        }

        private void Update()
        {
            RotatePlayer();            
            CheckMovement();
            CheckFire();
            CheckStats();
        }

        private void CheckStats()
        {
            if (battery < maxBattery)
                battery += batteryRecharge * (Time.deltaTime * 1);
            else
                battery = maxBattery;

            if (hitPoints > maxHitPoints)
                hitPoints = maxHitPoints;
        }

        private void CheckMovement()
        {
            var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            transform.position += move * (Time.deltaTime * enginePerformance);
        }

        private void RotatePlayer()
        {
            vectorToTarget = crosshair.transform.position - transform.position;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            qt = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            
            if(!Controllers.Pause.Instance.GetPauseState())
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qt, turn ); 
        }

        private void CheckFire()
        {
            if (Input.GetAxis("Fire") != 0 || Input.GetMouseButton(0))
                if (canFire && !Controllers.Pause.Instance.GetPauseState())
                {
                    if(battery < fireCost)
                        return;
                    
                    Audio.Instance.PlaySound(playerHurt);
                    battery -= (int) fireCost;
                    GameObject projectile = Instantiate(laser, firePoint.position, firePoint.rotation);
                    //-------SCALE
                    //projectile.transform.localScale = new Vector3(1.75f + (0.125f * (fireDamage - 15)), 1.75f + (0.125f * (fireDamage - 15)), 1);
                    //-------DAMAGE
                    projectile.GetComponent<Laser>().SetDamage(damage);
                    //-------FORCE
                    Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                    rb.AddForce(firePoint.up * firepower, ForceMode2D.Impulse);
                    //------------
                    StartCoroutine(FireCooldown());
                }
        }

        private IEnumerator FireCooldown()
        {
            canFire = false;
            yield return new WaitForSeconds(cooldown);
            canFire = true;
        }

        public void RemoveHitpoints(float value)
        {
            hitPoints -= value;
            
            CheckHP();
        }

        private void CheckHP()
        {
            if(hitPoints <= 0)
                Controllers.Game.Instance.GameOver();
        }

        public void AddFireDamage(int value)
        {
            damage += value;
        }
        
        public float GetFireDamage()
        {
            return damage;
        }

        public float GetEnergy()
        {
            return battery;
        }

        public float GetHitpoints()
        {
            return hitPoints;
        }
        
        public float GetMaxEnergy()
        {
            return maxBattery;
        }

        public float GetMaxHitpoints()
        {
            return maxHitPoints;
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Ship") && col.gameObject.layer == 8)
            {
                
            }

            switch (col.gameObject.tag)
            {
                case "Ship":
                    if (col.gameObject.layer != 8)
                        return;
                    
                    col.gameObject.GetComponent<BasicEnemy>().InstaKill();
                
                    if (hitPoints < (maxHitPoints / 4))
                        RemoveHitpoints(hitPoints);
                    else
                        hitPoints -= hitPoints / 2; 
                    break;
                case "Laser":
                    if (col.gameObject.layer != 8)
                        return;
                    
                    Other.CameraShake.Instance.ShakeScreen(0.2f, 0.2f);
                    Controllers.Audio.Instance.PlaySound(playerHurt);
                    Instantiate(playerHurtEffect, col.transform.position, Quaternion.identity);
                    Instantiate(Prefabs.Instance.scrap, col.transform.position, Quaternion.identity);
                    Destroy(col.gameObject);
                    hitPoints -= 20 + (Controllers.Wave.Instance.GetLevel() * 0.5f);
                    CheckHP();
                    break;
                default:
                    break;
            }
        }

        private void SetProportions()
        {
            //TODO: PŘEPSAT TEN CANCER, stavit stále jednotky
            /*
            shipTurnSpeed = 2 + ((fireCooldown * 5));
            crosshairSpeed = 5 / firePower;
            fireCost = fireDamage - 15;
            shipFlame.transform.localScale = new Vector3(1 * (playerSpeed / 7.5f), 1 * (playerSpeed / 7.5f), 1);
            transform.localScale = new Vector3(5 + ((maxHp - 100) / 100), 5 + ((maxHp - 100) / 100), 1);
            */
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (isColliding) return;
            isColliding = true;

            string collectibleType;
            
            if (col.gameObject.CompareTag("Collectible"))
            {
                Controllers.Audio.Instance.PlaySound(pickup);
                
                if (col.gameObject.layer == 13)
                {
                    Destroy(col.gameObject);
                    CreditCollection();
                    return;
                }
                
                BasicCollectible collectible = col.gameObject.GetComponent<BasicCollectible>();

                if (collectible.GetValue() > 0)
                {
                    notificationText.color = positive;
                    collectibleType = "++";
                }
                else
                {
                    notificationText.color = negative;
                    collectibleType = "--";
                }
                
                switch (collectible.pickupType)
                {
                    case CollectibleType.Barrier:
                        Controllers.Barrier.Instance.ChangeHitpoints(1);
                        collectible.Collect();
                        notificationText.text = collectibleType + " Barrier";
                        break;
                    
                    case CollectibleType.Cooldown:
                        cooldown -= collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " Cooldown";

                        if (cooldown < 0.05f)
                            cooldown = 0.05f;
                        else if (cooldown > 2)
                            cooldown = 2;
                        break;
                    
                    case CollectibleType.Damage:
                        Debug.Log(collectibleType);
                        damage += collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " Damage";
                        
                        if (damage < 5f)
                            damage = 5;
                        break;
                    
                    case CollectibleType.Energy:
                        maxBattery += collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " Battery";
                        
                        if (maxBattery < 20)
                            maxBattery = 20;
                        else if (maxBattery > 1000)
                            maxBattery = 1000;
                        break;
                    
                    case CollectibleType.Firepower:
                        firepower += collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " Firepower";

                        if (firepower < 0.1f)
                            firepower = 0.1f;
                        else if (firepower > 5)
                            firepower = 5;
                        break;
                    
                    case CollectibleType.Speed:
                        enginePerformance += collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " Speed";

                        if (enginePerformance < 0.5f)
                            enginePerformance = 0.5f;
                        else if (enginePerformance > 10)
                            enginePerformance = 10;
                        break;
                    
                    case CollectibleType.HP:
                        maxHitPoints += collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " HP";
                        
                        if (maxHitPoints < 20)
                            maxHitPoints = 20;
                        else if (maxHitPoints > 1000)
                            maxHitPoints = 1000;
                        break;
                    
                    default:
                        Debug.Log("the fuck");
                        break;
                }
                SpawnNotification();
                SetProportions();
            }
            StartCoroutine(TriggerCollisionReset());
        }

        private void CreditCollection()
        {
            Controllers.Credit.Instance.AddCredit(Random.Range(0, 50)); //Dodělat wave scaling
            
            notificationText.color = positive;
            notificationText.text = "++ Credit";
            SpawnNotification();
        }

        private void SpawnNotification()
        {
            notificationText.transform.position = transform.position;
            notificationText.GetComponent<Animator>().Play("NotificationText");
        }
        
        private IEnumerator TriggerCollisionReset()
        {
            yield return new WaitForEndOfFrame();
            isColliding = false;
        }

        public void UpgradeStatCall(ShipStats stat, int amount)
        {
            UpgradeStat(stat, amount);
        }

        private void UpgradeStat(ShipStats stat, int amount)
        {
            /*
            string editedStat = stat.ToString();
            char editChar = editedStat[0];
            editedStat.Remove(0);
            editedStat.Insert(0, editChar.ToString().ToLower());
            
            Debug.Log(editedStat);
            */

            switch (stat)
            {
                case ShipStats.Aim:
                    aim += amount;
                    break;
                case ShipStats.Battery:
                    battery += amount;
                    break;
                case ShipStats.BatteryRecharge:
                    batteryRecharge += amount;
                    break;
                case ShipStats.Cooldown:
                    cooldown += amount;
                    break;
                case ShipStats.Damage:
                    damage += amount;
                    break;
                case ShipStats.Firepower:
                    firepower += amount;
                    break;
                case ShipStats.Turn:
                    turn += amount;
                    break;
                case ShipStats.Scale:
                    scale += amount;
                    break;
                case ShipStats.EnginePerformance:
                    enginePerformance += amount;
                    break;
                case ShipStats.HitPoints:
                    hitPoints += amount;
                    break;
                default:
                    break;
            }
        }
    }
} 
