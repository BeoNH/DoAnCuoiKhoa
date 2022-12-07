using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCompeleted : Dialog
{
    public override void Show(bool isShow)
    {
        base.Show(isShow);
        
    }

    public void BackToMenu()
    {
        if (SceneController.Ins)
        {
            SceneController.Ins.LoadCurrentScene();
        }
    }

    public void Replay()
    {
        Close();
        if(GameManager.Ins)
        {
            GameManager.Ins.StarGame();
        }
    }

    public void NextLevel()
    {
        Close();
        if(GameManager.Ins)
        {
            GameManager.Ins.NextLevel();
        }
    }
}
