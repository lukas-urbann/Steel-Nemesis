using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class Score : MonoBehaviour
    {
        public static Score Instance;
        
        private int score = 0;
        public TMP_Text scoreDisplay;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        private void Start()
        {
            StartCoroutine(TimeScoreBonus());
        }

        private void Update()
        {
            scoreDisplay.text = "Score: " + score;
        }

        private IEnumerator TimeScoreBonus()
        {
            yield return new WaitForSeconds(10);
            if (!Game.Instance.GetGameOver())
            {
                score += 10;
                StartCoroutine(TimeScoreBonus());
            }
        }

        public int GetScore()
        {
            return score;
        }

        public void AddScore(int amount)
        {
            score += amount;
        }

        public void RemoveScore(int amount)
        {
            score -= amount;
        }
    }
}
