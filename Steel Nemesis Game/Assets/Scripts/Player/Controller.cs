using System;
using System.Collections;
using System.Collections.Generic;
using Collectible;
using Controllers;
using Enemy;
using Other;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public enum PlayerShipType
    {
        Cadet,
        Starfighter,
        Pioneer,
        Scorpion
    }

    public class Controller : MonoBehaviour
    {
        public static Controller Instance;
        public static Stats StatsInstance;
        
        //Ship Staty
        [SerializeField] private float hitPoints;
        [SerializeField]private float maxHitPoints;

        [SerializeField] private float battery;
        [SerializeField] private float maxBattery;
        [SerializeField] private float batteryRecharge;

        [SerializeField] private float fireCost;
        [SerializeField] private float cooldown;
        [SerializeField] private float firepower;
        [SerializeField] private float damage;
        [SerializeField] private float enginePerformance;
        [SerializeField] private float aim;
        [SerializeField] private float turn;
        [SerializeField] private float scale;
        [SerializeField] private float bulletSpread;

        private bool canFire = true;
        private bool isColliding = false; //Pro práci s kolizemi s triggery
        public Ship shipType;

        public delegate void OnShipUpgrade();
        public OnShipUpgrade statUpgrade;
            
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

        [Header("-- Assignable --")] [Header("Game Objects")]
        public GameObject crosshair;

        public GameObject laser;
        public GameObject playerHurtEffect;

        [Header("Transforms")]
        public List<Transform> firePoints = new List<Transform>();

        [Header("AudioClips")] public AudioClip fire;
        public AudioClip pickup;
        public AudioClip playerHurt;

        [Header("Notification Pickup")]
        public TMP_Text notificationText;

        [SerializeField] private Color positive, negative;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
            {
                Instance = this;
                StatsInstance = GetComponent<Player.Stats>();
            }
        }

        private void Start()
        {
            Controllers.Shop.Instance.onClose += ShopCloseAction;
            statUpgrade += CalculateUpgrades;
        }

        private void OnEnable()
        {
            CalculateUpgrades();
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

            if (!Controllers.Pause.Instance.GetPauseState())
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qt, turn);
        }

        private void CheckFire()
        {
            if (Input.GetAxis("Fire") != 0 || Input.GetMouseButton(0))
                if (canFire && !Controllers.Pause.Instance.GetPauseState())
                {
                    if (battery < fireCost)
                        return;

                    Fire();
                }
        }

        private void Fire()
        {
            Audio.Instance.PlaySound(fire);
            battery -= (int)fireCost;
            Vector3 positionUp;

            foreach (Transform pos in firePoints)
            {
                positionUp = pos.up;
                GameObject projectile = Instantiate(laser, pos.position, pos.rotation);
                projectile.GetComponent<Laser>().SetDamage(damage);
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                Vector2 aimPosition =
                    new Vector2(Random.Range(positionUp.x - bulletSpread,positionUp.x + bulletSpread), pos.up.y); // dodělat random spread
                rb.AddForce(aimPosition * firepower, ForceMode2D.Impulse);
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

            CheckHp();
        }

        private void CheckHp()
        {
            if (hitPoints <= 0)
                Game.Instance.GameOver();
        }

        //GETY
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
        // !GETY

        public void OnCollisionEnter2D(Collision2D col)
        {
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
                    CheckHp();
                    break;
                default:
                    break;
            }
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
                    col.GetComponent<CreditDrop>().PickUp();
                    Destroy(col.gameObject);
                    CreditCollection();
                    StartCoroutine(TriggerCollisionReset());
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
            }
            StartCoroutine(TriggerCollisionReset());
        }

        private void CreditCollection()
        {
            Credit.Instance.AddCredit(Random.Range(0, 50)); //Dodělat wave scaling
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

        private void AssignStats()
        {
            
        }

        public void CalculateUpgrades()
        {
            maxHitPoints = (shipType.baseMaxHitPoints + StatsInstance.GetShipStats()) * shipType.maxBatteryMultiplier;
            // maxBattery = (shipType.baseMaxBattery + maxBattery) * shipType.maxBatteryMultiplier;
            // batteryRecharge = (shipType.baseBatteryRecharge + batteryRecharge) * shipType.batteryRechargeMultiplier;
            // fireCost = (shipType.baseFireCost + fireCost) * shipType.fireCostMultiplier;
            // cooldown = (shipType.baseCooldown + cooldown) * shipType.cooldownMultiplier; 
            // firepower = (shipType.baseFirepower + firepower) * shipType.firepowerMultiplier;
            // damage = (shipType.baseDamage + damage) * shipType.damageMultiplier;
            // enginePerformance = (shipType.baseEnginePerformance + enginePerformance) * shipType.enginePerformanceMultiplier;
            // aim = shipType.baseAim;
            // turn = shipType.baseTurn;
            // scale = shipType.baseScale;
            // bulletSpread = shipType.baseBulletSpread;
        }

        public float GetCrosshairSpeed()
        {
            return aim;
        }
        
        private void ShopCloseAction()
        {
            transform.localPosition = new Vector3(0, -2.5f, 0);
            canFire = true;
            battery = maxBattery;
        }
    }
} 
