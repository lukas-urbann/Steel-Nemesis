using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour {
    [SerializeField] private float speed = 5;
    public GameObject destroyAnim;
    private string type;
    private GameObject gameCont;
    AudioSource audioSource;
    public AudioClip shooter;
    public AudioClip kamikadze;
    public AudioClip normal;

    void Start()
    {
        gameCont = GameObject.Find("GameController");

        speed = (speed + (gameCont.GetComponent<RoundController>().level * 0.15f));
        audioSource = GetComponent<AudioSource>();
        if(gameObject.CompareTag("EnemyLaserNormal"))
        {
            type = "Normal";
            audioSource.PlayOneShot(normal);
        } else if(gameObject.CompareTag("EnemyLaserShooter")) {
            type = "Shooter";
            audioSource.PlayOneShot(shooter);
        } else if(gameObject.CompareTag("EnemyLaserKamikadze")) {
            type = "Kamikadze";
            audioSource.PlayOneShot(kamikadze);
        }
    }

    void Update()
    {
        transform.Translate(Vector2.down * Time.deltaTime * speed);
    }

    void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
        Instantiate(destroyAnim, transform.position, transform.rotation);

        

        if(other.gameObject.CompareTag("Player"))
        {        
        switch(type){
            case "Normal":
            other.gameObject.GetComponent<Hrac>().hp -= 20;
            break;
            case "Shooter":
            other.gameObject.GetComponent<Hrac>().hp -= 30;
            break;
            case "Kamikadze":
            other.gameObject.GetComponent<Hrac>().hp -= 10;
            break;
            default:
            break;
        }
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
