using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class Wave : MonoBehaviour
    {
        public static Wave Instance;

        private Vector2 minSpawnDimension = new Vector2(-5.50f, 4.5f);
        private Vector2 maxSpawnDimension = new Vector2(5.50f, 2f);
        public TMP_Text levelTitle, levelSeconds;
        private bool waveEnd = false, wavePending = false;
        private bool countdownActive = false;
        private float waveCountdown = 5.99f;
        private float enemyCountdown = 1;
        private float minSpawnDelay, maxSpawnDelay;
        
        public delegate void WaveChangeDelegate();
        public WaveChangeDelegate onWaveEnd;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        private void Start()
        {
            ResetText();
        }

        private int minEnemies = 3, maxEnemies = 5, actualEnemies, remainingToKill = 100;
        private int level = 40;

        private void Update()
        {
            if (waveEnd)
            {
                WaveEnd();
                countdownActive = true;
                waveEnd = false;
            }

            if (wavePending)
                WavePending();

            if (countdownActive)
            {
                waveCountdown -= 1 * Time.deltaTime;
                float flooredCountdown = Mathf.FloorToInt(waveCountdown);
                levelSeconds.text = flooredCountdown.ToString("F0");
                
                if (waveCountdown <= 0.1f)
                    waveCountdown = 0.1f;
            }

            if (remainingToKill == 0)
            {
                remainingToKill = 69;
                waveEnd = true;
            }
        }

        private void ResetText()
        {
            levelTitle.text = "";
            levelSeconds.text = "";
        }

        private void WaveEnd()
        {
            StartCoroutine(WavePostEnd());
            onWaveEnd.Invoke();
            level++;
            levelTitle.text = "Wave " + (level);
        }

        private IEnumerator WavePostEnd()
        {
            yield return new WaitForSeconds(5.99f);
            Player.Player.Instance.AddFireDamage(1);
            waveCountdown = 5.99f;
            ResetText();
            countdownActive = false;
            minEnemies++;
            maxEnemies = maxEnemies + 2 + (level / 2);
            WaveStart();
        }

        private void WaveStart()
        {
            Debug.Log("Wave Start");
            actualEnemies = Random.Range(minEnemies, maxEnemies);
            remainingToKill = actualEnemies;
            
            Debug.Log("Enemies for level " + level + ". Remaining to kill: " + actualEnemies);
            
            wavePending = true;

            if(minSpawnDelay > 0.1f)
                minSpawnDelay = (0.5f - (Controllers.Wave.Instance.level * 0.002f));

            if (maxSpawnDelay > 1)
                maxSpawnDelay = (5 - (Controllers.Wave.Instance.level * 0.02f));
        }

        private void WavePending()
        {
            enemyCountdown -= 1 * Time.deltaTime;

            if (enemyCountdown < 0 && actualEnemies > 0)
            {
                SpawnEnemy();
                actualEnemies--;
                enemyCountdown = Random.Range(minSpawnDelay, 5);
            }
        }

        private void SpawnEnemy()
        {
            float x = Random.Range(minSpawnDimension.x, maxSpawnDimension.x);
            float y = Random.Range(minSpawnDimension.y, maxSpawnDimension.y);

            Vector2 spawnLocation = new Vector2(x, y);

            GameObject spawnedEnemy =
                Instantiate(Controllers.Prefabs.Instance.basicEnemy, spawnLocation, Quaternion.identity);
        }
        
        public int GetLevel()
        {
            return level;
        }

        public void SetRemainingEnemies(int val)
        {
            remainingToKill += val;
        }

        public void StartGame()
        {
            waveEnd = true;
        }
    }
}
