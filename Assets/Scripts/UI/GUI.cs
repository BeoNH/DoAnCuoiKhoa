using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : Singleton<GUI> 
{
    public GameObject mainMenu;
    public GameObject gamePlay;
    public Text bulletTxt;
    public Text levelTxt;
    public Image timerFilled;

    public Dialog gameoverDialog;
    public Dialog waveCompeletedDialog;

    public override void Awake()
    {
        MakeSingleton(false);
    }

    public override void Start()
    {
        base.Start();
        UpdateTimeBar(1, 1);
    }

    public void ShowGamePlay(bool isShow)
    {
        if(gamePlay)
        {
            gamePlay.SetActive(isShow);
        }

        if (mainMenu)
        {
            mainMenu.SetActive(!isShow);
        }
    }

    public void UpdateBullet(int bullet)
    {
        if (bulletTxt)
            bulletTxt.text = "x" + bullet;
    }

    public void UpdateLevel(int level)
    {
        if(levelTxt)
            levelTxt.text = "LEVEL" + level;
    }

    public void UpdateTimeBar(float cur, float total, bool isReverse = false)
    {
        if (!timerFilled) return;

        if (isReverse)
        {
            timerFilled.fillAmount = 1 - (cur / total);  
        }
        else
        {
            timerFilled.fillAmount = cur / total;
        }
    }
}
