using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Other
{
    public class ScrapDrop : MonoBehaviour
    {
        private SpriteRenderer sprite;
        private float rotationSpeed;
        private Vector3 movement;
        public List<Sprite> spriteList = new List<Sprite>();

        private void OnEnable()
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            sprite.sprite = spriteList[Random.Range(0, spriteList.Count)];
        }
    }
}
