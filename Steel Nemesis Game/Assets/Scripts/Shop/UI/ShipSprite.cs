using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shop.UI
{
    public class ShipSprite : MonoBehaviour
    {
        private Image shipSprite;

        private void Start()
        {
            Controllers.Shop.Instance.onOpen += UpdateLabel;
            shipSprite = GetComponent<Image>();
            
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            shipSprite.sprite = Player.Controller.Instance.shipType.shipSprite;
        }
    }
}