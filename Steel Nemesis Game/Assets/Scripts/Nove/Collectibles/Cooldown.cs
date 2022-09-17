using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectible
{
    public class Cooldown : BasicCollectible
    {
        private void OnEnable()
        {
            base.value = value * 0.1f;
        }
    }
}
