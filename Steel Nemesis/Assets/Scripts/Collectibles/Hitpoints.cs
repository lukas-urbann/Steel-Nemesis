using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Collectible
{
    public class Hitpoints : BasicCollectible
    {
        private void OnEnable()
        {
            value *= 10;
        }
    }
}
