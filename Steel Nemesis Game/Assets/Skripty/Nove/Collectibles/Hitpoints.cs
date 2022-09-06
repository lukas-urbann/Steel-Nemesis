using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectible
{
    public class Hitpoints : BasicCollectible
    {
        private void OnEnable()
        {
            base.value = 20;
        }
    }
}
