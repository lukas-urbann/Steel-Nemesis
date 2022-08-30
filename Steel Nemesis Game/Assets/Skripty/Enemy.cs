using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemys : MonoBehaviour {

    private int hp = 1; //Základní hodnota hp
	private float speed = 2f; //Základní hodnota rychlost
    private float rotationSpeed = 50; //Rychlost otáčení
    [SerializeField] private float shootSpeedMin; //Největší rychlost nabíjení
    [SerializeField] private float shootSpeedMax; //Nejmenší rychlost nabíjení
    [SerializeField] private int hpMax; //Největší možný počet životů
    [SerializeField] private int hpMin; //Nejmenší počet životů
    private bool right; //Používá se při změnu letu lodě, v tuto chvíli používá pouze Normal a Tank
    public bool canShoot = false;
    public GameObject explosion;
    public GameObject player;
    public GameObject playerLaserDestroy;
    public GameObject enemyLaser;
    private GameObject gameCont;
    public GameObject energyBoost, shieldBoost, medkit, speedBoost;
    private string enemyClass; //Slouží k sebeidentifikaci třídy
    public EnemyMothership mother;
    AudioSource audioSource;
    public AudioClip hit;
    

    void Start () {
        gameCont = GameObject.Find("GameController");

        speed = (speed + (gameCont.GetComponent<RoundController>().level * 0.04f));

        audioSource = GetComponent<AudioSource>();
        //Při spawnu najde hráče
        player = GameObject.FindWithTag("Player");

        shootSpeedMax = (shootSpeedMax - (gameCont.GetComponent<RoundController>().level * 0.01f));

        //Nastaví lodi třídu při spawnu a provede další vyžadované akce, pokud nějaké jsou
        if (gameObject.tag == "EnemyTank")
		{
			enemyClass = "Tank";
		} else if(gameObject.tag == "EnemyShooter")
		{
			enemyClass = "Shooter";
            StartCoroutine(RandomShoot(shootSpeedMin, shootSpeedMax));
        } else if(gameObject.tag == "EnemyNormal")
		{
			enemyClass = "Normal";
            StartCoroutine(RandomShoot(shootSpeedMin, shootSpeedMax));
            StartCoroutine(RandomDirection(0,5));
        } else if (gameObject.tag == "EnemyKamikadze")
        {
            enemyClass = "Kamikadze";
            StartCoroutine(RandomShoot(shootSpeedMin, shootSpeedMax));
        } else if (gameObject.tag == "EnemyMothership")
        {
            enemyClass = "Mothership";
            StartCoroutine(MothershipSpawner());
        }
        /*Tato if alley každé třídě přiřadí string s názvem její třídy, a proto,
          kdyby se v budoucnu chtělo něco provést u každé třídy jinak, stačilo by místo
        hromady ifů použít jednoduchý switch*/

        //Nastaví nepříteli náhodný počet jeho životů
        hp = Random.Range(hpMin, hpMax);

        //Sloužilo k debugu - při vytvoření nepřítele se jeho třída kontrolně napíše do konzole.
		Debug.Log(enemyClass);
	}

    void FixedUpdate()
    {
        //Počítá rotaci nepřítele ke hráči
        Vector3 vectorToTarget = player.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion qt = Quaternion.AngleAxis(angle + 90, Vector3.forward);

        //Určuje pohyb nepřátelské lodi
        switch (enemyClass)
        {
            case "Tank":
                transform.Translate(Vector3.down * Time.deltaTime * (speed/2), Space.World);

                switch (right)
                {
                    case true:
                        transform.Translate(Vector3.right * Time.deltaTime * (speed / 2), Space.World);
                        break;
                    case false:
                        transform.Translate(Vector3.left * Time.deltaTime * (speed / 2), Space.World);
                        break;
                }
                break;

            case "Normal":
                transform.Translate(Vector3.down * Time.deltaTime * speed, Space.World);

                switch (right)
                {
                    case true:
                        transform.Translate(Vector3.right * Time.deltaTime * speed, Space.World);
                        break;
                    case false:
                        transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
                        break;
                }
                break;
            case "Kamikadze":
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, (speed*3) * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qt, Time.deltaTime * rotationSpeed);
                break;
            case "Shooter":
                transform.rotation = Quaternion.RotateTowards(transform.rotation, qt, Time.deltaTime * rotationSpeed);
                transform.Translate(Vector3.down * Time.deltaTime * speed, Space.World);
                break;
            case "Mothership":
                transform.Translate(Vector2.down * Time.deltaTime * (speed / 2));
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        //Určuje kolizi
		if(other.gameObject.tag == "PlayerLaser")
		{
            //Pokud do nepřítele narazí hráčův náboj, proběhne následující
            player.gameObject.GetComponent<Hrac>().hp += 2;
            Destroy(other.gameObject);
            Instantiate(playerLaserDestroy, other.transform.position, other.transform.rotation);
			hp--;
			CheckHP();
		}

        if(other.gameObject.CompareTag("Player"))
        {
            //Pokud do nepřítele narazí hráč, tak hráč dostane poškození a nepřítel vybouchne
            other.gameObject.GetComponent<Hrac>().hp -= 60;
            hp = 0;
            CheckHP();
        }
    }

	void CheckHP() {
        audioSource.PlayOneShot(hit);
        //Zkontroluje počet životů, pokud je pod nulou, tak nepřítel vybouchne
		if(hp<=0)
		{
			Debug.Log("EnemyDeath");
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy(gameObject);
            player.gameObject.GetComponent<Hrac>().hp += 10;
            GameObject.Find("GameController").GetComponent<RoundController>().enemyAmount -= 1;

            switch(enemyClass)
            {
                case "Normal":
                player.gameObject.GetComponent<Hrac>().score += (10 * GameObject.Find("GameController").GetComponent<RoundController>().level);
                player.gameObject.GetComponent<Hrac>().credits += 10;
                break;
                case "Fighter":
                player.gameObject.GetComponent<Hrac>().score += (30 * GameObject.Find("GameController").GetComponent<RoundController>().level);
                player.gameObject.GetComponent<Hrac>().credits += 50;
                break;
                case "Tank":
                player.gameObject.GetComponent<Hrac>().score += (20 * GameObject.Find("GameController").GetComponent<RoundController>().level);
                player.gameObject.GetComponent<Hrac>().credits += 25;
                break;
                case "Mothership":
                player.gameObject.GetComponent<Hrac>().score += (100 * GameObject.Find("GameController").GetComponent<RoundController>().level);
                player.gameObject.GetComponent<Hrac>().credits += 100;
                break;
            }

            float random = Random.Range(0,10);
            if(random<=1)
            {
                DropBooster();
            }
		}
	}

    void Shoot()
    {
        if(canShoot)
        {
        //Vytvoří střelu, jednoduché
        if (gameObject.tag == "EnemyShooter")
        {
            GameObject BarrelLt = gameObject.GetComponent<EnemyShooter>().BarrelL;
            GameObject BarrelRt = gameObject.GetComponent<EnemyShooter>().BarrelR;
            Instantiate(enemyLaser, BarrelLt.transform.position, transform.root.rotation);
            Instantiate(enemyLaser, BarrelRt.transform.position, transform.root.rotation);
        }
        else
        {
            Instantiate(enemyLaser, transform.position, transform.root.rotation);
        }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("NoShoot"))
        {
            canShoot = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        {
            if(other.gameObject.CompareTag("ItemKiller"))
            {
                player.gameObject.GetComponent<Hrac>().hp = 0;
            }
        }
    }

    IEnumerator MothershipSpawner()
    {
        //Řídí spawn lodí typu kamikadze z lodě typu Mothership
        int spawn = Random.Range(1, 3);
        switch (spawn)
        {
            case 1:
                Instantiate(mother.drone, mother.spawnerL.position, mother.spawnerL.rotation);
                break;
            case 2:
                Instantiate(mother.drone, mother.spawnerR.position, mother.spawnerR.rotation);
                break;
        }

        yield return new WaitForSeconds(1.834890563907967679869878901378027402586748396498390274906489f);
        StartCoroutine(MothershipSpawner());
    }


    IEnumerator RandomShoot(float min, float max)
    {
        //Slouží pro více nepřátel - střílí náhodně pomocí vybraného času
        float countdown = Random.Range(min, max);
        countdown -= Time.deltaTime;
        yield return new WaitForSeconds(countdown);
        StartCoroutine(RandomShoot(min, max));
        Shoot();
    }

    IEnumerator RandomDirection(float min, float max)
    {
        //Nastavuje změnu směru nepřítele - v tomto případě používá pouze třída Normal a Tank
        float countdown = Random.Range(min, max);
        countdown -= Time.deltaTime;

        yield return new WaitForSeconds(countdown);
        StartCoroutine(RandomDirection(min, max));
        if (right)
        {
            right = false;
        }
        else
        {
            right = true;
        }
    }

    void DropBooster()
    {
        int boost = Random.Range(0,5);
        switch(boost)
        {
            case 1:
            Instantiate(medkit, transform.position, Quaternion.identity);
            break;
            case 2:
            Instantiate(speedBoost, transform.position, Quaternion.identity);
            break;
            case 3:
            Instantiate(shieldBoost, transform.position, Quaternion.identity);
            break;
            case 4:
            Instantiate(energyBoost, transform.position, Quaternion.identity);
            break;
            default:
            break;
        }
    }
}
