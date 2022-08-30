using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        private int hp = 100;
        private int energy = 100;
        
        private float fireCooldown = 0.5f;
        private float firePower = 20;
        private bool canFire = true;

        public GameObject crosshair;
        public Transform firePoint;
        public GameObject laser;
        
        private Vector3 vectorToTarget;
        private float angle;
        private Quaternion qt;

        private void Update()
        {
            RotatePlayer();            
            CheckMovement();
            CheckFire();
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
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qt, Time.time ); 
        }

        private void CheckFire()
        {
            if (Input.GetAxis("Fire") != 0 || Input.GetMouseButton(0))
                if (canFire && !Controllers.Pause.Instance.GetPauseState())
                {
                    GameObject projectile = Instantiate(laser, firePoint.position, firePoint.rotation);
                    Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                    rb.AddForce(firePoint.up * 10, ForceMode2D.Impulse);
                    StartCoroutine(FireCooldown());
                }
        }

        private IEnumerator FireCooldown()
        {
            canFire = false;
            yield return new WaitForSeconds(fireCooldown);
            canFire = true;
        }
    }
} 
