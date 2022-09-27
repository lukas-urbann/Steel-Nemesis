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
        [SerializeField] private bool doNotStart = false; // Při zapnutí z inspektoru povolí hru spustit pro testovací účely

        private Vector2 minSpawnDimension = new Vector2(-5.50f, 4.5f);
        private Vector2 maxSpawnDimension = new Vector2(5.50f, 2f);

        private int minEnemies = 3, maxEnemies = 5, actualEnemies;
        private List<GameObject> spawnedEnemies = new List<GameObject>();
        private int level = 0;

        //private bool waveEnd = false;
        //private bool wavePending = false;
        //private bool countdownActive = false;
        //private bool canSpawn = true;
        
        private const float countdownTimeBase = 5.9999f;
        private float countdownTime = 5.9999f;
        private float enemyCountdown = 1;
        
        private float minSpawnDelay;
        private float maxSpawnDelay;

        [Header("Spawnable Enemies")]
        [SerializeField] private List<GameObject> enemyList = new List<GameObject>(); //Obsahuje list všech enemáků, loaduju ho ve Startu
        private List<GameObject> selectedEnemies = new List<GameObject>();
        
        //Obsahuje list enemáků, kteří splňují kritérium pro spawn v nté vlně.
        //Všechny tyto staty jsou nastavené jednotlivě v každé enemy lodi v inspectoru, protože se to řídí
        //z jejich základního skriptu BasicEnemy a je třeba to mít serializované už v něm.

        [Header("Assignable")] 
        public TMP_Text levelTitle;
        public TMP_Text levelSeconds;
        
        public delegate void WaveDelegate();
        public WaveDelegate onWaveEnd; //Použít na shop
        public WaveDelegate onWaveStart;
        public WaveDelegate startEvent;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        private void Start()
        {
            startEvent += GameStart;
            
            //Idk proc to nedelam v inspectoru ale ok
            enemyList.Add(Prefabs.Instance.basicEnemy);
            enemyList.Add(Prefabs.Instance.attackerEnemy);
            enemyList.Add(Prefabs.Instance.fighterEnemy);
            enemyList.Add(Prefabs.Instance.tankEnemy);
            
            ResetCountdownText();
        }
        
        private void Update()
        {
            /*
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
            */
            
            /*

            if (remainingToKill == 0)
            {
                if (Shop.Instance.PostWaveShopCall() && level != 0)
                {
                    interWaveBreak = true;
                    return;
                }

                if (!interWaveBreak)
                {
                    remainingToKill = 69;
                    waveEnd = true;
                } 
            }
            
            */
        }

        private IEnumerator WaveProcess()
        {

            yield return new WaitForNextFrameUnit();

        }

        private void WaveStart()
        {
//          onWaveStart.Invoke();
            Debug.Log("Wave Start");
            
            GenerateEnemiesForWave(); // Vygeneruje počet nepřátel + typy nepřátel
            GenerateSpawnDelays(); // Vygeneruje minimální a maximální časy pro spawn
            StartCoroutine(StartWaveCountdown()); // Odpočítá a spustí waveku
        }
        




        private IEnumerator StartWaveCountdown()
        {
            IncreaseLevel();
            ResetCountdownText();
            DisplayCountdownText();
            StartCoroutine(CountToZero());
            IncreaseEnemyCount();
            yield return new WaitForSeconds(countdownTimeBase);
            StartCoroutine(WaveProcess());
        }

        private IEnumerator CountToZero()
        {
            yield return new WaitForSeconds(1);

            if (countdownTime < 1)
            {
                ResetCountdownText();
                yield break;
            }
            
            countdownTime -= 1;
            DisplayCountdownText();
            StartCoroutine(CountToZero());
        }

        private void DisplayCountdownText()
        {
            levelTitle.text = "Wave " + level;
            levelSeconds.text = Mathf.FloorToInt(countdownTime).ToString();
        }
        
        private void ResetCountdownText()
        {
            levelTitle.text = "";
            levelSeconds.text = "";
        }

        private void ResetCountdownTimer()
        {
            countdownTime = countdownTimeBase;
        }

        private void IncreaseEnemyCount()
        {
            minEnemies++;
            maxEnemies = maxEnemies + 2 + (level / 2);
        }
        
        private void KillAllHostiles()
        {
            List<GameObject> enemyListToDestroy = new List<GameObject>();
            enemyListToDestroy.AddRange(FindObjectsOfType<BasicEnemy>());

            for(int i = 0; i < enemyList.Count; i++)
                Destroy(enemyList[i]);
        }

        private void SelectEligibleEnemies()
        {
            selectedEnemies.Clear();
            
            foreach (GameObject obj in enemyList)
            {
                BasicEnemy en = obj.GetComponentInChildren<BasicEnemy>();
                
                if (en.GetMinLevel() <= level && en.GetMaxLevel() >= level)
                    selectedEnemies.Add(obj);
            }
        }

        private void IncreaseLevel()
        {
            level++;
        }
        
        private void SpawnEnemy()
        {
            float x = Random.Range(minSpawnDimension.x, maxSpawnDimension.x);
            float y = Random.Range(minSpawnDimension.y, maxSpawnDimension.y);
            Vector2 spawnLocation = new Vector2(x, y);
            
            int enemyIndex = Random.Range(0, selectedEnemies.Count);

            GameObject spawnedEnemy = Instantiate(selectedEnemies[enemyIndex].gameObject, spawnLocation, Quaternion.identity);
            
            spawnedEnemies.Add(spawnedEnemy);
            actualEnemies--;
        }

        private void GenerateEnemiesForWave()
        {
            SelectEligibleEnemies();
            actualEnemies = Random.Range(minEnemies, maxEnemies);
        }

        private void GenerateSpawnDelays()
        {
            if(minSpawnDelay > 0.1f)
                minSpawnDelay = (0.5f - (level * 0.002f));

            if (maxSpawnDelay < 1)
                maxSpawnDelay = (6 - (level * 0.03f));
        }
        
        private void GameStart()
        {
            if (doNotStart)
                return;

            WaveStart();
        }

        public void GameStartTrigger()
        {
            startEvent.Invoke();
            Debug.Log("Game Start Trigger");
        }

        //GET
        public int GetLevel()
        {
            return level;
        }
    }
}
