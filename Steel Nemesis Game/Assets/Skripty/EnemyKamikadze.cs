using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKamikadze : MonoBehaviour {

    public GameObject Explosion;
    public float timeLeft = 10;

    void Update () {
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0)
        {
            Instantiate(Explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
