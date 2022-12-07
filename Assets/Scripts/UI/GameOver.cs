using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : Dialog
{
    public Text totalScoreTxt;
    public Text bestScoreTxt;

    public override void Show(bool isShow)
    {
        base.Show(isShow);

        if (totalScoreTxt && GameManager.Ins)
            totalScoreTxt.text = GameManager.Ins.Socre.ToString();

        if(bestScoreTxt)
            bestScoreTxt.text = Pref.bestScore.ToString();
        
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
        if (GameManager.Ins)
        {
            GameManager.Ins.StarGame();
        }
    }

}
