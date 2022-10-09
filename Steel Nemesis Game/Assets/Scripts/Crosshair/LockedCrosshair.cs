using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Crosshair
{
    public class LockedCrosshair : MonoBehaviour
    {
        public Transform crosshair;
        private SpriteRenderer crosshairSprite;
        
        public static LockedCrosshair Instance;
        private Vector3 targetedLocation = new Vector3(0,0,0);

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        private void Start()
        {
            crosshairSprite = GetComponent<SpriteRenderer>();
            Controllers.Shop.Instance.onClose += ChangeVisibility;
            Controllers.Shop.Instance.onOpen += ChangeVisibility;
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, crosshair.position, Time.deltaTime * Player.Controller.StatsInstance.GetShipStats(ShipStats.Aim));
            targetedLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }

        public Vector3 GetTargetedLocation()
        {
            return targetedLocation;
        }

        private void ChangeVisibility()
        {
            crosshairSprite.enabled = !crosshairSprite.enabled;
        }
    }
} 

