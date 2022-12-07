using System.Collections;
using System.Collections.Generic;
//using UDEV.MonsterDefense;
using UnityEngine;

public class SlowController : Singleton<SlowController>
{
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;

    public override void Awake()
    {
        MakeSingleton(false);
    }

    void Update()
    {
         if (!GameManager.Ins || !GUI.Ins) return;

        // check game thuc su dang chay hay khong 
        if (GameManager.Ins.state != GameState.Playing) return;

        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
         
        if (GameManager.Ins.IsSlowed)
         {
             GUI.Ins.UpdateTimeBar(Time.timeScale, 1, true);
         }
    }

    public void DoSlowmotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;

        if (GameManager.Ins)
        {
            GameManager.Ins.IsSlowed = true;
        }
    }
}
