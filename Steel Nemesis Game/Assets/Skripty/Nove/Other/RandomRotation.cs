using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Other
{
    public class RandomRotation : MonoBehaviour
    {
        private void Awake()
        {
            transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        }
    }
}
