using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Enemy;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;
        
        private float hp = 100;
        private float maxHp = 100;
        
        
        private float energy = 200;
        private float maxEnergy = 200;
        private float energyGain = 10;


        private float fireCost = 20;
        private float fireCooldown = 0.5f;
        private float firePower = 1.0f;
        private int fireDamage = 25;
        
        private bool canFire = true;
        public AudioClip laserSfx;
        
        
        public GameObject crosshair;
        public Transform firePoint;
        public GameObject laser;
        
        private Vector3 vectorToTarget;
        private float angle;
        private Quaternion qt;

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
            transform.position += move * (Time.deltaTime * 3);
        }

        private void RotatePlayer()
        {
            vectorToTarget = crosshair.transform.position - transform.position;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            qt = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            
            if(!Controllers.Pause.Instance.GetPauseState())
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qt, 5 ); 
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
        
        public int GetFireDamage()
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

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Ship") && col.gameObject.layer == 8)
            {
                col.gameObject.GetComponent<BasicEnemy>().InstaKill();

                if (!(hp < hp / 4))
                    hp -= hp / 2; 
                else
                    RemoveHitpoints(hp);
            }
        }
    }
} 
