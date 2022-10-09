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

        [SerializeField] private float hp;
        [SerializeField] private float battery;
        [SerializeField] private float fireCost;

        [SerializeField] private bool canMove = true;
        [SerializeField] private bool canFire = true;
        private bool isColliding = false; //Pro práci s kolizemi s triggery
        public Ship shipType;
        private Coroutine fireDelay;

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
            hp = shipType.baseHitPoints;
            battery = shipType.baseBattery;
            Controllers.Shop.Instance.onClose += ShopCloseAction;
            Controllers.Shop.Instance.onOpen += ShopOpenAction;
            //statUpgrade += CalculateUpgrades;
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
            if (battery < StatsInstance.GetShipStats(ShipStats.MaxBattery))
                battery += StatsInstance.GetShipStats(ShipStats.BatteryRecharge) * (Time.deltaTime * 1);
            else
                battery = StatsInstance.GetShipStats(ShipStats.MaxBattery);

            if (hp > StatsInstance.GetShipStats(ShipStats.MaxHitPoints))
                hp = StatsInstance.GetShipStats(ShipStats.MaxHitPoints);
        }

        private void CheckMovement()
        {
            if (!canMove)
                return;
            
            var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            transform.position += move * (Time.deltaTime * StatsInstance.GetShipStats(ShipStats.EnginePerformance));
        }

        private void RotatePlayer()
        {
            vectorToTarget = crosshair.transform.position - transform.position;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            qt = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            if (!Controllers.Pause.Instance.GetPauseState())
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qt, StatsInstance.GetShipStats(ShipStats.Turn));
        }

        private void CheckFire()
        {
            if (Input.GetAxis("Fire") != 0 || Input.GetMouseButton(0))
                if (!Controllers.Pause.Instance.GetPauseState())
                {
                    if (battery < fireCost)
                        return;

                    if (!canFire)
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
                projectile.GetComponent<Laser>().SetDamage(StatsInstance.GetShipStats(ShipStats.Damage));
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                Vector2 aimPosition =
                    new Vector2(Random.Range(positionUp.x - 0,positionUp.x + 0), pos.up.y); // dodělat random spread
                rb.AddForce(aimPosition * StatsInstance.GetShipStats(ShipStats.Firepower), ForceMode2D.Impulse);
                
                fireDelay = StartCoroutine(FireCooldown());
            }
        }

        private IEnumerator FireCooldown()
        {
            canFire = false;
            yield return new WaitForSeconds(StatsInstance.GetShipStats(ShipStats.Cooldown));
            canFire = true;
        }

        public void RemoveHitpoints(float value)
        {
            hp -= value;

            CheckHp();
        }

        private void CheckHp()
        {
            if (hp <= 0)
                Game.Instance.GameOver();
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            switch (col.gameObject.tag)
            {
                case "Ship":
                    if (col.gameObject.layer != 8)
                        return;

                    col.gameObject.GetComponent<BasicEnemy>().InstaKill();

                    if (hp < (StatsInstance.GetShipStats(ShipStats.MaxHitPoints) / 4))
                        RemoveHitpoints(hp);
                    else
                        hp -= hp / 2;
                    break;
                case "Laser":
                    if (col.gameObject.layer != 8)
                        return;

                    Other.CameraShake.Instance.ShakeScreen(0.2f, 0.2f);
                    Controllers.Audio.Instance.PlaySound(playerHurt);
                    Instantiate(playerHurtEffect, col.transform.position, Quaternion.identity);
                    Instantiate(Prefabs.Instance.scrap, col.transform.position, Quaternion.identity);
                    Destroy(col.gameObject);
                    hp -= 20; // TODO: Enemy dmg
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
                {/*
                    case CollectibleType.Barrier:
                        Controllers.Barrier.Instance.ChangeHitpoints(1);
                        collectible.Collect();
                        notificationText.text = collectibleType + " Barrier";
                        break;

                    case CollectibleType.Cooldown:
                        cooldown -= collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " Cooldown";
                        break;

                    case CollectibleType.Damage:
                        Debug.Log(collectibleType);
                        damage += collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " Damage";
                        break;

                    case CollectibleType.Energy:
                        maxBattery += collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " Battery";
                        break;

                    case CollectibleType.Firepower:
                        firepower += collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " Firepower";
                        break;

                    case CollectibleType.Speed:
                        enginePerformance += collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " Speed";
                        break;

                    case CollectibleType.HP:
                        maxHitPoints += collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " HP";
                        break;

                    default:
                        Debug.Log("the fuck");
                        break;
                */}
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
        
        public float GetHP()
        {
            return hp;
        }

        public float GetBattery()
        {
            return battery;
        }
        
        private void ShopOpenAction()
        {
            canMove = false;
            canFire = false;
            Cursor.visible = true;
            transform.localPosition = new Vector3(0, 0, 0);
            StopCoroutine(fireDelay);
        }

        private void ShopCloseAction()
        {
            canMove = true;
            canFire = true;
            transform.localPosition = new Vector3(0, -2.5f, 0);
            battery = StatsInstance.GetShipStats(ShipStats.MaxBattery);
        }
    }
} 
