using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image hpbar;

    public float barhp;

    public Hrac hphrac;


    void Update()
    {
        barhp = (hphrac.hp / hphrac.hpmax);

        hpbar.fillAmount = barhp;
    }
}
