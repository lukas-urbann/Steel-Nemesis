using System;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public enum AttributeType
    {
        Ship,
        Player,
    }
    
    public class Attribute : MonoBehaviour
    {
        [SerializeField] protected ShipStats statType;
        [SerializeField] protected AttributeType attributeType;
        public GameObject slots;
        protected List<Image> upgradeIndicators = new List<Image>();
        [SerializeField] protected Sprite takenIndicator, openIndicator;
        protected int statLevel = 0;

        public Button plusButton, minusButton;

        private void Start()
        {
            foreach (Transform child in slots.transform)
                upgradeIndicators.Add(child.GetComponent<Image>());
            
            CheckLevel();
        }

        protected void DisableButton()
        {
            
        }
        
        protected void EnableButton()
        {
            
        }

        protected void Increase()
        {
            statLevel++;
            CheckLevel();
        }

        protected void Decrease()
        {
            statLevel--;
            CheckLevel();
        }

        protected void CheckLevel()
        {
            for (int i = 0; i < upgradeIndicators.Count; i++)
            {
                upgradeIndicators[i].sprite = openIndicator;
            }

            for (int i = 0; i < statLevel; i++)
            {
                upgradeIndicators[i].sprite = takenIndicator;
            }
        }
    }
}