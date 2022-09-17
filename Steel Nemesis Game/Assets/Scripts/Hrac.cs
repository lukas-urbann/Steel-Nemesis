using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hrac : MonoBehaviour {

    public GameObject strela;
    public GameObject explosion;
    public GameObject playerLaserDestroy;
    public GameObject BarrelL;
    public GameObject BarrelR;
    public GameObject BarrelM;
    public GameObject gameController;
    public GameObject canExit;
    public Text speedboosttext;
    public Text energyboosttext;
    public Text shieldboostertext;
    public Text gameover;
    public float speedhard = 5;
    public float speed;
    public int credits = 0;
    public int score = 0;
    public int laserlevel = 1;
    public int bonusPowerUp = 0;
    public float energymax = 100;
    public float hpmax = 100;
    public float energy = 100;
    public float hp = 100;
    public bool isDead = false;
    public bool isInvincible = false;
    public bool isEnergyboosted = false;
    AudioSource audioSource;
    public AudioClip pickup;
    public AudioClip blueLaser;
    public Text lasertext;
    public Text energytext;
    public Text shieldtext;
    public Text speedtext;
    public Text powertext;
    public Text cooldowntext;
    public int laserprice = 1500, energyprice = 500, shieldprice = 1000, speedprice = 500, powerupprice = 500, cooldownprice = 750;
    public float cooldownlaser = 0.1f, cooldownMax = 0.5f;
    private GameObject canvasShop;
    private bool shopOpen = false;

    void Awake()
    {
        speedboosttext.text = "";
        energyboosttext.text = "";
        shieldboostertext.text = "";
        audioSource = GetComponent<AudioSource>();
        gameController = GameObject.Find("/GameController");
        canvasShop = GameObject.Find("/UI/Shop");
        /*
        GameObject.Find ("LaserUpgrade/Text").GetComponent<Text>().text = " " + laserprice;
        GameObject.Find ("EnergyUpgrade/Text").GetComponent<Text>().text = " " + energyprice;
        GameObject.Find ("ShieldUpgrade/Text").GetComponent<Text>().text = " " + shieldprice;
        GameObject.Find ("SpeedUpgrade/Text").GetComponent<Text>().text = " " + speedprice;
        GameObject.Find ("PowerupUpgrade/Text").GetComponent<Text>().text = " " + powerupprice;
        GameObject.Find ("CooldownUpgrade/Text").GetComponent<Text>().text = " " + cooldownprice;
        */ //canvasShop.SetActive(false);
    }

    void Update() {
        speed = speedhard;
        cooldownlaser -= Time.deltaTime;
        if(Input.GetKeyDown("space")) {
            if(energy>10 && cooldownlaser<=0)
            {
                cooldownlaser = cooldownMax;
                Shoot();
            }
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            if(!shopOpen)
            {
                canvasShop.SetActive(true);
                shopOpen = true;
            } else {
                canvasShop.SetActive(false);
                shopOpen = false;
            }
            
            
        }

        if(energy < energymax) {
            energy += Time.deltaTime * 10;
        } else if(energy > energymax) {
            energy = energymax;
        }

        if(hp <= 0)
        {
            isDead = true;
            GameOver();
        }
        if(hp >= hpmax)
        {
            hp = hpmax;
        }

        if(isInvincible)
        {
            hp = hpmax;
        }

        if(isEnergyboosted)
        {
            energy = energymax;
        }

    }

    void FixedUpdate() {
        if(Input.GetAxis("Horizontal") < 0)
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);
        }
        if(Input.GetAxis("Horizontal") > 0)
        {
            transform.Translate(Vector2.right * Time.deltaTime * speed);
        }
        if(Input.GetAxis("Vertical") > 0)
        {
            transform.Translate(Vector2.up * Time.deltaTime * speed);
        }
        if(Input.GetAxis("Vertical") < 0)
        {
            transform.Translate(Vector2.down * Time.deltaTime * speed);
        }
    }

    void Shoot() {
        GamePause pause = gameController.GetComponent<GamePause>();
        if(!pause.pause)
        {
        audioSource.PlayOneShot(blueLaser);
        energy -= 10;

        switch(laserlevel)
        {
            case 1:
                Instantiate(strela, BarrelM.transform.position, BarrelM.transform.rotation);
            break;
            case 2:
                Instantiate(strela, BarrelL.transform.position, BarrelL.transform.rotation);
                Instantiate(strela, BarrelR.transform.position, BarrelR.transform.rotation);
            break;
            case 3:
                Instantiate(strela, BarrelM.transform.position, BarrelM.transform.rotation);
                Instantiate(strela, BarrelL.transform.position, BarrelL.transform.rotation);
                Instantiate(strela, BarrelR.transform.position, BarrelR.transform.rotation);
            break;
        }
        } else {
        }
        
    }

    void GameOver()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        gameObject.SetActive(false);
        showScore();

        GameObject mbg = GameObject.Find("ScoreBG");
        mbg.SetActive(false);
        GameObject cbg = GameObject.Find("MoneyBG");
        cbg.SetActive(false);
        GameObject ebr = GameObject.Find("EnergyBar");
        ebr.SetActive(false);
        GameObject hbr = GameObject.Find("HealthBar");
        hbr.SetActive(false);

        string[] tagy =
                     {
                 "EnemyTank",
                 "EnemyKamikadze",
                 "EnemyShooter",
                 "EnemyMothership",
                 "EnemyNormal",
                 "GameCon"
             };
        foreach (string tag in tagy)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject other in gameObjects)
            {
                Instantiate(explosion, other.transform.position, other.transform.rotation);
                Instantiate(canExit, other.transform.position, other.transform.rotation);
                canvasShop.SetActive(false);
                Destroy(other);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == 12)
        {
            audioSource.PlayOneShot(pickup);
        }
    }

    void showScore()
    {
        gameover.text = "GAME-OVER!\nScore: " + score + "\n\n\nPress return to enter game menu";
    }

    public void energyUpgrade()
    {
        if(credits >= energyprice)
        {
            credits -= energyprice;
            energymax += 10;
            energyprice += (energyprice/2);
            GameObject.Find ("EnergyUpgrade/Text").GetComponent<Text>().text = " " +  energyprice;
        }        
    }

    public void shieldUpgrade()
    {
        if(credits >= shieldprice)
        {
            credits -= shieldprice;
            hpmax += 10;
            shieldprice += (shieldprice/2);
            GameObject.Find ("ShieldUpgrade/Text").GetComponent<Text>().text = " " +  shieldprice;
        }        
    }

    public void speedUpgrade()
    {
        if(credits >= speedprice)
        {
            credits -= speedprice;
            speedhard += 0.5f;
            speedprice += (speedprice/2);
            GameObject.Find ("SpeedUpgrade/Text").GetComponent<Text>().text = " " +  speedprice;
        }        
    }

    public void cooldownUpgrade()
    {
        if(credits >= cooldownprice)
        {
            credits -= cooldownprice;
            cooldownMax -= 0.1f;
            cooldownprice += (cooldownprice/2);
            GameObject.Find ("CooldownUpgrade/Text").GetComponent<Text>().text = " " +  cooldownprice;
        }        
    }

    public void PowerupUpgrade()
    {
        if(credits >= powerupprice)
        {
            credits -= powerupprice;
            bonusPowerUp += 1;
            powerupprice += (powerupprice/2);
            GameObject.Find ("PowerupUpgrade/Text").GetComponent<Text>().text = " " +  powerupprice;
        }        
    }

    public void laserUpgrade()
    {
        if(credits >= laserprice)
        {
            laserlevel++;
            credits -= laserprice;
            laserprice += (laserprice/2);
            GameObject.Find ("LaserUpgrade/Text").GetComponent<Text>().text = " " +  laserprice;
            if(laserlevel == 3)
            {
                GameObject upgrade = GameObject.Find ("LaserUpgrade");
                Destroy(upgrade.gameObject);
            }
        }
    }   
}
