using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crosshair
{
    public class Crosshair : MonoBehaviour
    {
        private SpriteRenderer crosshairSprite;

        private void Start()
        {
            crosshairSprite = GetComponent<SpriteRenderer>();
            Controllers.Shop.Instance.onClose += ChangeVisibility;
            Controllers.Shop.Instance.onOpen += ChangeVisibility;
        }

        private void Update()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
            
            if(!Controllers.Pause.Instance.GetPauseState())
                transform.position = mousePosition;
        }
        
        private void ChangeVisibility()
        {
            crosshairSprite.enabled = !crosshairSprite.enabled;
        }
    }
}

