using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Shop
{
    public class PrefabHolder : MonoBehaviour
    {
        public static Shop.PrefabHolder Instance;

        public TMP_Text costText;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;
        }
    }
}
