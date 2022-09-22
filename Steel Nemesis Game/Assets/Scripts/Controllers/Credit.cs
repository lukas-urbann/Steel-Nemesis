using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Controllers
{
    public class Credit : MonoBehaviour
    {
        public static Credit Instance;

        private int credit = 0;
        public TMP_Text creditDisplay; //Dosadit z indicatoru ze scény
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }
        
        private void Update()
        {
            creditDisplay.text = "Credit: " + credit;
        }
        
        public void AddCredit(int amount)
        {
            credit += amount;
        }
    }
}