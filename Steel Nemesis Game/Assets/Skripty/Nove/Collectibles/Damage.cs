using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectible
{
    public class Damage : BasicCollectible
    {
        private void OnEnable()
        {
            base.value = 5;
        }
    }
}
