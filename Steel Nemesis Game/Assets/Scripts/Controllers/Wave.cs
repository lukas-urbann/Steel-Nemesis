using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
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

        private bool waveEnd = false;
        private bool wavePending = false;
        private bool interWaveBreak = false;
        private bool countdownActive = false;
        
        private float waveCountdown = 5.99f;
        private float enemyCountdown = 1;
        private float minSpawnDelay;
        private float maxSpawnDelay;

        private List<GameObject> enemyList = new List<GameObject>(); //Obsahuje list všech enemáků, loaduju ho ve Startu
        private List<GameObject> selectedEnemies = new List<GameObject>();
        //Obsahuje list enemáků, kteří splňují kritérium pro spawn v nté vlně.
        //Všechny tyto staty jsou nastavené jednotlivě v každé enemy lodi v inspectoru, protože se to řídí
        //z jejich základního skriptu BasicEnemy a je třeba to mít serializované už v něm.

        public TMP_Text levelTitle;
        public TMP_Text levelSeconds;
        
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
            //Idk proc to nedelam v inspectoru ale ok
            enemyList.Add(Prefabs.Instance.basicEnemy);
            enemyList.Add(Prefabs.Instance.attackerEnemy);
            enemyList.Add(Prefabs.Instance.fighterEnemy);
            enemyList.Add(Prefabs.Instance.tankEnemy);
            
            ResetText();
        }

        private int minEnemies = 3, maxEnemies = 5, actualEnemies, remainingToKill = 100;
        private int level = 0;

        private void Update()
        {
            if (waveEnd)
                WaveEnd();

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
            if (Shop.Instance.PostWaveShopCall() && level != 0)
            {
                interWaveBreak = true;
                return;
            }
            
            StartCoroutine(WavePostEnd());
            onWaveEnd.Invoke();
            level++;
            levelTitle.text = "Wave " + (level);
            
            //Dosazené z Updatu z nějakého důvodu
            countdownActive = true;
            waveEnd = false;
        }

        private IEnumerator WavePostEnd()
        {
            yield return new WaitForSeconds(5.99f);
            Player.Controller.Instance.AddFireDamage(1);
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
            SelectEnemies();
            actualEnemies = Random.Range(minEnemies, maxEnemies);
            remainingToKill = actualEnemies;
            
            Debug.Log("Enemies for level " + level + ". Remaining to kill: " + actualEnemies);
            
            wavePending = true;

            if(minSpawnDelay > 0.1f)
                minSpawnDelay = (0.5f - (Controllers.Wave.Instance.level * 0.002f));

            if (maxSpawnDelay > 1)
                maxSpawnDelay = (6 - (Controllers.Wave.Instance.level * 0.02f));
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

        private void SelectEnemies()
        {
            selectedEnemies.Clear();
            
            foreach (GameObject obj in enemyList)
            {
                BasicEnemy en = obj.GetComponentInChildren<BasicEnemy>();
                
                if (en.GetMinLevel() <= level && en.GetMaxLevel() >= level)
                {
                    selectedEnemies.Add(obj);
                    //Debug.Log(en.name + " | Min Level: " + en.GetMinLevel() + " | Max Level: " + en.GetMaxLevel());
                }
            }
        }

        private void SpawnEnemy()
        {
            float x = Random.Range(minSpawnDimension.x, maxSpawnDimension.x);
            float y = Random.Range(minSpawnDimension.y, maxSpawnDimension.y);
            Vector2 spawnLocation = new Vector2(x, y);
            
            int enemyIndex = Random.Range(0, selectedEnemies.Count);
            
            GameObject spawnedEnemy =
                Instantiate(selectedEnemies[enemyIndex].gameObject, spawnLocation, Quaternion.identity);
        }
        
        public int GetLevel()
        {
            return level;
        }

        public void SetRemainingEnemies(int val)
        {
            remainingToKill += val;
        }

        public void ResumeGame()
        {
            waveEnd = true;
        }
    }
}
