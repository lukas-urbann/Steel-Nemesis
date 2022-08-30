using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laser
{
    public class BasicLaser : MonoBehaviour
    {
        private void OnBecameInvisible() {
            Destroy(gameObject);
        }
    }
}
