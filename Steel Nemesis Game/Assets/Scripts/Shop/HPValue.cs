using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Shop
{
    public class HPValue : MonoBehaviour
    {
        private TMP_Text valueText;

        private void OnEnable()
        {
            valueText = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            Controllers.Shop.Instance.onOpen += UpdateText;
            UpdateText();
        }

        private void UpdateText()
        {
            valueText.text = Player.Controller.Instance.GetHitpoints() + " / " + Player.Controller.Instance.GetMaxHitpoints();
        }
    }
}
