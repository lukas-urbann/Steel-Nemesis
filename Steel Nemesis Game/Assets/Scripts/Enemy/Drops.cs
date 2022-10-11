using System;
using UnityEngine;
using System.Collections.Generic;

namespace Enemy
{
    public class Drops : MonoBehaviour
    {
        public static Drops Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        [SerializeField] private List<GameObject> drops = new List<GameObject>();

        public int GetDropsLength()
        {
            return drops.Count;
        }

        public GameObject GetDrop(int index)
        {
            //TODO:tohle furt háže errory fix

            try
            {
                GameObject drop = drops[index - 1];

                return drop;
            }
            catch (Exception e)
            {
                Debug.LogWarning("ITEM ERROR > ŠPATNÝ INDEX: " + index + " | Drop error: " + e.StackTrace);

            }
            return null;
        }
    }
}