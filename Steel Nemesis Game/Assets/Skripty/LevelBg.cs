using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBg : MonoBehaviour {

    public float bgSpeed;
    public Renderer bgRend;

    void Update()
    {
        bgRend.material.mainTextureOffset += new Vector2(0f, bgSpeed * Time.deltaTime);
    }
}
