using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour {

    public Image enbar;

    public float baren;

    public Hrac enhrac;


    void Update()
    {
        baren = (enhrac.energy / enhrac.energymax);

        enbar.fillAmount = baren;
    }
}
