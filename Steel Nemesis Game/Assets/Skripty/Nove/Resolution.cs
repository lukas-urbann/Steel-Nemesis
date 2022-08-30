using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    public int width, height;
    
    public void ChangeResolution()
    {
        Screen.SetResolution(width, height, false);
    }
}
