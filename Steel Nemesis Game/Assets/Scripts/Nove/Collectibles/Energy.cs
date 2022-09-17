using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectible
{
    public class Energy : BasicCollectible
    {
        private void OnEnable()
        {
            base.value = 20;
        }
    }
}
