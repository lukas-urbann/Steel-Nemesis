using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Other
{
    public class Death : MonoBehaviour
    {
        public GameObject explosionPrefab;
        
        public void DeathEvent()
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
