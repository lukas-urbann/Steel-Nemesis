using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreController : MonoBehaviour {

    public Hrac hracvar;

    public TMP_Text scoretext;
    public TMP_Text moneytext;

    void Update()
    {
        scoretext.text = "Score: " + hracvar.score;
        moneytext.text = "Credits: " + hracvar.credits;
    }
}
