using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState state;
    public Camera cam;
    public bool isOnMobile;

    public override void Awake()
    {
        MakeSingleton(false);
    }
}
