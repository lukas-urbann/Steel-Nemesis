using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Collectible
{
    public class Randomizer : MonoBehaviour
    {
        public AudioClip dropSound;
        public GameObject negativeVariant, positiveVariant;
        
        private void OnEnable()
        {
            float chance = Random.Range(0f, 3f);

            if (chance > 1.5f)
            {
                Instantiate(positiveVariant, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(negativeVariant, transform.position, Quaternion.identity);
            }
            
            Controllers.Audio.Instance.PlaySound(dropSound);
            Destroy(gameObject);
        }
    }
}
