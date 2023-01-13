using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemy;
using TMPro;
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

        public float minEnemies = 3, maxEnemies = 5, actualEnemies;
        public List<GameObject> spawnedEnemies = new List<GameObject>();
        private int level = 0;

        private bool waveStop = false; // Nešahat, legacy kod - asi se to bez toho rozbije nevim
        
        private const float CountdownTimeBase = 5.9999f;
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

        private Coroutine pendingWave;
        
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
            Shop.Instance.onClose += SignalWaveStart;
            
            //Idk proc to nedelam v inspectoru ale ok
            enemyList.Add(Prefabs.Instance.basicEnemy);
            enemyList.Add(Prefabs.Instance.attackerEnemy);
            enemyList.Add(Prefabs.Instance.fighterEnemy);
            enemyList.Add(Prefabs.Instance.tankEnemy);
            
            ResetCountdownText();
        }
        
        private IEnumerator WaveProcess()
        {
            if (waveStop)
            {
                WaveEnd();
                yield break;
            }

            waveStop = true;
            
            while (waveStop)
            {
                float timeToSpawn = Random.Range(1f, 5f);

                while (enemyCountdown < timeToSpawn)
                {
                    if (!waveStop)
                        yield break;

                    if (actualEnemies > 0)
                        enemyCountdown += Time.deltaTime;
                    
                    for (var i = spawnedEnemies.Count - 1; i > -1; i--)
                    {
                        if (spawnedEnemies[i] == null)
                            spawnedEnemies.RemoveAt(i);
                    }
                    
                    if(actualEnemies <= 0 && spawnedEnemies.Count == 0)
                        WaveEnd();

                    yield return null;
                }
                
                SpawnEnemy();
                enemyCountdown = 0;
                yield return null;
            }

            waveStop = false;
        }

        public void SignalWaveStart()
        {
            WaveStart();
        }

        private void WaveEnd()
        {
            waveStop = false; //Legacy kod, nešahat, asi se to bez toho rozbije nevim
            StopCoroutine(pendingWave);
            //onWaveEnd.Invoke();
            KillAllHostiles();

            if (!Shop.Instance.SignalShop())
                WaveStart(); // Přejda do dalšího kola a postará se o správnou inicializaci
        }

        private void WaveStart()
        {
            onWaveStart.Invoke();
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
            yield return new WaitForSeconds(CountdownTimeBase);
            pendingWave = StartCoroutine(WaveProcess());
        }

        private IEnumerator CountToZero()
        {
            yield return new WaitForSeconds(1);

            if (countdownTime < 1)
            {
                ResetCountdownText();
                ResetCountdownTimer();
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
            countdownTime = CountdownTimeBase;
        }

        private void IncreaseEnemyCount()
        {
            minEnemies += 0.3f;
            maxEnemies += 1;
        }
        
        private void KillAllHostiles()
        {
            //TODO: NEVIM JESTLI FUNGUJE, JSOU TU POTIZE
            var enemyListToDestroy = new List<BasicEnemy>();
            enemyListToDestroy.AddRange(FindObjectsOfType<BasicEnemy>().ToList());

            for (int i = 0; i < enemyListToDestroy.Count; i++)
            {
                if (enemyListToDestroy[i] == null)
                    enemyListToDestroy.RemoveAt(i);
                
                if (enemyListToDestroy.Count != 0)
                    DestroyImmediate(enemyList[i], true);
            }
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
            actualEnemies = Random.Range(Mathf.FloorToInt(minEnemies), maxEnemies);
        }

        private void GenerateSpawnDelays()
        {
            if(minSpawnDelay > 0.1f)
                minSpawnDelay = (0.5f - (level * 0.002f));

            if (maxSpawnDelay > 1)
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
