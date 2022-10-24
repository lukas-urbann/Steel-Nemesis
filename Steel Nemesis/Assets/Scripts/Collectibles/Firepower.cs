using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Collectible
{
    public class Firepower : BasicCollectible
    {
        private void OnEnable()
        {
            value *= 0.1f;
        }
    }
}
