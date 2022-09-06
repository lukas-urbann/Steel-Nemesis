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
            base.value = value * 0.1f;
        }
    }
}
