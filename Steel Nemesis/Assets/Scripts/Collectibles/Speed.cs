using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectible
{
    public class Speed : BasicCollectible
    {
        private void OnEnable()
        {
            value *= 0.25f;
        }
    }
}
