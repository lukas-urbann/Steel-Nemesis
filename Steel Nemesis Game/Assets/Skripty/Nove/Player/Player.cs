using System;
using System.Collections;
using System.Collections.Generic;
using Collectible;
using Controllers;
using Enemy;
using TMPro;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;
        
        private float hp = 100;
        private float maxHp = 100;
        private float energy = 50;
        private float maxEnergy = 50;
        private float energyGain = 10;
        private float fireCost = 10;
        private float fireCooldown = 0.5f;
        private float firePower = 1;
        private float fireDamage = 25;
        private float playerSpeed = 3;

        private float crosshairSpeed = 5;
        private float shipTurnSpeed = 5;
        
        private bool canFire = true;
        
        private Vector3 vectorToTarget;
        private float angle;
        private Quaternion qt;

        private bool isColliding = false;
        
        [Header("Assignable")]
        public GameObject crosshair;
        public GameObject shipFlame;
        public Transform firePoint;
        public GameObject laser;
        public AudioClip laserSfx, pickupSfx;
        
        [Header("Notification Pickup")]
        public TMP_Text notificationText;
        
        [SerializeField] private Color positive, negative;

        public float GetCrosshairSpeed()
        {
            return crosshairSpeed;
        }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
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
            if (energy < maxEnergy)
                energy += energyGain * Time.deltaTime;
            else
                energy = maxEnergy;

            if (hp > maxHp)
                hp = maxHp;
        }

        private void CheckMovement()
        {
            var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            transform.position += move * (Time.deltaTime * playerSpeed);
        }

        private void RotatePlayer()
        {
            vectorToTarget = crosshair.transform.position - transform.position;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            qt = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            
            if(!Controllers.Pause.Instance.GetPauseState())
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qt, shipTurnSpeed ); 
        }

        private void CheckFire()
        {
            if (Input.GetAxis("Fire") != 0 || Input.GetMouseButton(0))
                if (canFire && !Controllers.Pause.Instance.GetPauseState())
                {
                    if(energy < fireCost)
                        return;
                    
                    Controllers.Audio.Instance.PlaySound(laserSfx);
                    energy -= (int) fireCost;
                    GameObject projectile = Instantiate(laser, firePoint.position, firePoint.rotation);
                    //-------SCALE
                    projectile.transform.localScale = new Vector3(3.75f + (0.125f * (fireDamage - 15)), 3.75f + (0.125f * (fireDamage - 15)), 1);
                    //-------DAMAGE
                    projectile.GetComponent<Laser.PlayerLaser>().SetDamage(fireDamage);
                    //-------FORCE
                    Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                    rb.AddForce(firePoint.up * firePower, ForceMode2D.Impulse);
                    //------------
                    StartCoroutine(FireCooldown());
                }
        }

        private IEnumerator FireCooldown()
        {
            canFire = false;
            yield return new WaitForSeconds(fireCooldown);
            canFire = true;
        }

        public void RemoveHitpoints(float value)
        {
            hp -= value;
            
            CheckHP();
        }

        private void CheckHP()
        {
            if(hp <= 0)
                Controllers.Game.Instance.GameOver();
        }

        public void AddFireDamage(int value)
        {
            fireDamage += value;
        }
        
        public float GetFireDamage()
        {
            return fireDamage;
        }

        public float GetEnergy()
        {
            return energy;
        }

        public float GetHitpoints()
        {
            return hp;
        }
        
        public float GetMaxEnergy()
        {
            return maxEnergy;
        }

        public float GetMaxHitpoints()
        {
            return maxHp;
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Ship") && col.gameObject.layer == 8)
            {
                col.gameObject.GetComponent<BasicEnemy>().InstaKill();
                
                if (hp < (maxHp / 4))
                    RemoveHitpoints(hp);
                else
                    hp -= hp / 2; 
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (isColliding) return;
            isColliding = true;

            string collectibleType;
            
            if (col.gameObject.CompareTag("Collectible"))
            {
                BasicCollectible collectible = col.gameObject.GetComponent<BasicCollectible>();
                Controllers.Audio.Instance.PlaySound(pickupSfx);

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
                        fireCooldown -= collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " Cooldown";
                        shipTurnSpeed = 2 + ((fireCooldown * 5));

                        if (fireCooldown < 0.05f)
                            fireCooldown = 0.05f;
                        else if (fireCooldown > 2)
                            fireCooldown = 2;
                        break;
                    
                    case CollectibleType.Damage:
                        fireDamage += collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " Damage";
                        
                        if (fireDamage < 5f)
                            fireDamage = 5;
                        
                        fireCost = fireDamage - 15;
                        break;
                    
                    case CollectibleType.Energy:
                        maxEnergy += collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " Battery";
                        
                        if (maxEnergy < 20)
                            maxEnergy = 20;
                        else if (maxEnergy > 1000)
                            maxEnergy = 1000;
                        break;
                    
                    case CollectibleType.Firepower:
                        firePower += collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " Firepower";

                        crosshairSpeed = 5 / firePower;

                        if (firePower < 0.1f)
                            firePower = 0.1f;
                        else if (firePower > 5)
                            firePower = 5;
                        break;
                    
                    case CollectibleType.Speed:
                        playerSpeed += collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " Speed";

                        shipFlame.transform.localScale = new Vector3(1 * (playerSpeed / 7.5f), 1 * (playerSpeed / 7.5f), 1);

                        if (playerSpeed < 0.5f)
                            playerSpeed = 0.5f;
                        else if (playerSpeed > 10)
                            playerSpeed = 10;
                        break;
                    
                    case CollectibleType.HP:
                        maxHp += collectible.GetValue();
                        collectible.Collect();
                        notificationText.text = collectibleType + " HP";
                        transform.localScale = new Vector3(5 + ((maxHp - 100) / 100), 5 + ((maxHp - 100) / 100), 1);
                        
                        if (maxHp < 20)
                            maxHp = 20;
                        else if (maxHp > 1000)
                            maxHp = 1000;
                        break;
                    
                    default:
                        Debug.Log("the fuck");
                        break;
                }
                notificationText.transform.position = transform.position;
                notificationText.GetComponent<Animator>().Play("NotificationText");
            }
            
            StartCoroutine(TriggerCollisionReset());
        }

        private IEnumerator TriggerCollisionReset()
        {
            yield return new WaitForEndOfFrame();
            isColliding = false;
        }
    }
} 
