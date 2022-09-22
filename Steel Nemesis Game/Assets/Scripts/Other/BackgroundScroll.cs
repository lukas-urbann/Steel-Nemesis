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
        private bool useController = false;

        private void Start()
        {
            if (Controllers.Wave.Instance != null)
                useController = true;
        }

        private void Update()
        {
            if (Controllers.Pause.Instance.GetPauseState())
                return;
            
            if(useController)
                backgroundRenderer.material.mainTextureOffset += new Vector2(sideScrollSpeed, ((baseScrollSpeed + (incrementalScrollSpeed * Controllers.Wave.Instance.GetLevel())) * Time.deltaTime));
            else
                backgroundRenderer.material.mainTextureOffset += new Vector2(sideScrollSpeed, baseScrollSpeed * Time.deltaTime);
        }
    }
}
