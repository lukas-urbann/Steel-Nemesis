using System.Collections;
using UnityEngine;

public class Instancer : MonoBehaviour
{
    //TODO: Fixnout cancer
    public Controllers.Audio sfx;
    public Controllers.Barrier barrier;
    public Controllers.Credit credit;
    public Controllers.Game game;
    public Controllers.Music music;
    public Controllers.Pause pause;
    public Controllers.Prefabs prefabs;
    public Controllers.Score score;
    public Controllers.Shop shop;
    public Controllers.Wave wave;
    public Player.Controller player;
    
    private void Start()
    {
        sfx = Controllers.Audio.Instance;
        barrier = Controllers.Barrier.Instance;
        credit = Controllers.Credit.Instance;
        game = Controllers.Game.Instance;
        music = Controllers.Music.Instance;
        pause = Controllers.Pause.Instance;
        prefabs = Controllers.Prefabs.Instance;
        score = Controllers.Score.Instance;
        shop = Controllers.Shop.Instance;
        wave = Controllers.Wave.Instance;
        player = Player.Controller.Instance;
    }
}