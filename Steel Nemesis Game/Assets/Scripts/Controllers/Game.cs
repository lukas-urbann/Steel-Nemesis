using System;
using System.Collections;
using System.Collections.Generic;
using Other;
using TMPro;
using Triggers;
using UnityEngine;

namespace Controllers
{
    public class Game : MonoBehaviour
    {
        public static Game Instance;

        private float gameTimer = 0;
        private int gameKills = 0;
        
        public GameObject statScreen;
        public GameObject cursor;
        private bool gameover = false;

        [Header("StatScreen texts")]
        public TMP_Text statTime;
        public TMP_Text statKills;
        public TMP_Text statScore;
        public TMP_Text statMoney;

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

        public bool GetGameOver()
        {
            return gameover;
        }

        private void Update()
        {
            if (!gameover)
                gameTimer += Time.deltaTime * 1;
        }

        public void AddKill()
        {
            gameKills++;
        }

        public void GameOver()
        {
            Shop.Instance.shopWindow.SetActive(false);
            gameover = true;
            
            TriggerType[] trigs;
            trigs = FindObjectsOfType<TriggerType>();

            List<GameObject> ships = new List<GameObject>();

            foreach (TriggerType type in trigs)
            {
                if(type.triggerType == TypeOfTrigger.ship)
                    ships.Add(type.transform.root.gameObject);
            }
            
            Destroy(FindObjectOfType<Controllers.Wave>().gameObject);

            foreach (GameObject ship in ships)
            {
                ship.GetComponent<Death>().DeathEvent();
            }
            
            statTime.text = "Game Length: " + ((int)gameTimer / 60) + ":" + ((int)gameTimer % 60).ToString("00");
            statKills.text = "Enemies destroyed: " + gameKills;
            statScore.text = "Earned score: " + Score.Instance.GetScore();

            statScreen.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            cursor.SetActive(false);
        }
    }
}