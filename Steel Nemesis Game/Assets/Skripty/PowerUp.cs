using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    private bool pickedup = false;
    private int booster = 0;
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
    }

	void Update () {
        if(pickedup == false)
        {
            transform.Translate(Vector2.down * Time.deltaTime * 2);
        }
        Hrac stats = player.GetComponent<Hrac>();
        booster = stats.bonusPowerUp;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            pickedup = true;
            Debug.Log("pickup");
            if (gameObject.tag == "SpeedBoost")
            {
                StartCoroutine(BoostPickUp(other));
            } else if (gameObject.tag == "HealthBoost")
            {
                Hrac stats = other.GetComponent<Hrac>();
                stats.hp += 30;
                Destroy(gameObject);
            } else if (gameObject.tag == "ShieldBoost")
            {
                StartCoroutine(ShieldBooster(other));
            } else if (gameObject.tag == "EnergyBoost")
            {
                StartCoroutine(EnergyBooster(other));
            }
        } else if(other.gameObject.tag == "ItemKiller")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator BoostPickUp(Collider2D player)
    {
        Hrac stats = player.GetComponent<Hrac>();
        stats.speedhard = (stats.speedhard + 5);
        stats.speedboosttext.text = "speedbooster";

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(10 + booster);

        stats.speedhard = (stats.speedhard - 5);
        stats.speedboosttext.text = "";

        Destroy(gameObject);
    }

    IEnumerator EnergyBooster(Collider2D player)
    {
        Hrac stats = player.GetComponent<Hrac>();
        stats.isEnergyboosted = true;
        stats.energyboosttext.text = "energybooster";

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(10 + booster);

        stats.isEnergyboosted = false;
        stats.energyboosttext.text = "";
        Destroy(gameObject);
    }

    IEnumerator ShieldBooster(Collider2D player)
    {
        Hrac stats = player.GetComponent<Hrac>();
        stats.isInvincible = true;
        stats.shieldboostertext.text = "shieldbooster";

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(10 + booster);

        stats.shieldboostertext.text = "";
        stats.isInvincible = false;
        Destroy(gameObject);
    }
}
