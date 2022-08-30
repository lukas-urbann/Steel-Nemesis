using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundController : MonoBehaviour
{
    public GameObject spawner1,spawner2,spawner3,spawner4,spawner5;
    public GameObject enemyNormal, enemyTank, enemyFighter, enemyMothership;
    private GameObject enemyType;
    [SerializeField]private float SpawnSpeed = 0, SpawnSpeedMin = 0, SpawnSpeedMax = 1;
    private float SpawnSpeedBonus = 0f;
    public int level = 1;
    private int SelectedSpawner;
    public float countdown = 6;
    public float spawnCooldown;
    public float timeoutTime = 5;
    public bool timeout = true, canSpawn = false;
    public int enemiesLeft = 5;
    public int enemyAmount = 5;
    public Text currentLevel;
    public Text countdownText;
    public int enemyClass;
    
    void Start()
    {
        enemiesLeft = enemyAmount;
        currentLevel.text = "";
        countdownText.text = "";
    }

    void Update()
    {
        SpawnSpeedBonus = (level * 0.1f);
        if(countdown <= 0)
            {
            canSpawn = true;
            timeout = false;
            countdown = 15;
            }

        if(timeout && !canSpawn)
        {
            currentLevel.text = "Level " + level;
            countdownText.text = "" + ((int)countdown).ToString();
            countdown -= Time.deltaTime;
        } else if(!timeout && canSpawn) {
            currentLevel.text = "";
            countdownText.text = "";
            spawnCooldown -= Time.deltaTime;

            if(spawnCooldown <= 0 && enemiesLeft > 0 && enemyAmount > 0)
            {
                SpawnEnemy();
            }

            if(enemiesLeft == 0 && enemyAmount == 0)
            {
                spawnCooldown = 3;
                canSpawn = false;
                timeout = true;
                countdown = 5.99f;
                level++;
                enemyAmount += (Random.Range(1,10 + level) + level);
                enemiesLeft = enemyAmount;
            }
        }
    }

    void SpawnEnemy()
    {
        enemiesLeft--;
        GenerateSpawner();
        GenerateClass();
        GenerateSpeed();
        Spawn(enemyClass, SelectedSpawner);
    }

    void GenerateSpawner()
    {
        SelectedSpawner = Random.Range(1, 5);
    }

    void GenerateSpeed()
    {
        spawnCooldown = Random.Range(SpawnSpeedMin, (SpawnSpeedMax - SpawnSpeedBonus));
    }

    void GenerateClass()
    {
    if(level > 2)
        {
           enemyClass = Random.Range(1, 3); 
        } else {
            enemyClass = Random.Range(1, 2);  
        }
        if(level > 4)
        {
          enemyClass = Random.Range(1, 5);  
        } 
    }

    void Spawn(int enemyClass, int SelectedSpawner)
    {
        Debug.Log(enemyClass);
        switch(enemyClass)
        {
            case 1:
            enemyType = enemyNormal;
            break;
            case 2:
            enemyType = enemyTank;
            break;
            case 3:
            enemyType = enemyFighter;
            break;
            case 4:
            enemyType = enemyMothership;
            break;
        }

        switch(SelectedSpawner)
        {
            case 1:
            Instantiate(enemyType, spawner1.transform.position, spawner1.transform.rotation);
            break;
            case 2:
            Instantiate(enemyType, spawner2.transform.position, spawner2.transform.rotation);
            break;
            case 3:
            Instantiate(enemyType, spawner3.transform.position, spawner3.transform.rotation);
            break;
            case 4:
            Instantiate(enemyType, spawner4.transform.position, spawner4.transform.rotation);
            break;
            case 5:
            Instantiate(enemyType, spawner5.transform.position, spawner5.transform.rotation);
            break;
            default:
            break;
        }
    }
}
