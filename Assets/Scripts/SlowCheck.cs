using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        var enemy = col.GetComponent<Enemy>();

        if(enemy)
        {
            enemy.CanSlow = true;
        }
    }
}
