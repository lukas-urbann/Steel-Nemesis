using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Other
{
    public class BackgroundScroll : MonoBehaviour {

        [SerializeField] private float baseScrollSpeed;
        [SerializeField] private float incrementalScrollSpeed;
        [SerializeField] private float sideScrollSpeed;
        public MeshRenderer backgroundRenderer;

        private void Update()
        {
            backgroundRenderer.material.mainTextureOffset += new Vector2(sideScrollSpeed, ((baseScrollSpeed + (incrementalScrollSpeed * Controllers.Wave.Instance.GetLevel())) * Time.deltaTime));
        }
    }
}
