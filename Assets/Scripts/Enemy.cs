using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private  float moveSpeed;
    [SerializeField] private GameObject deadPrefab;
    private bool _canSlow;
    private bool _isDead;

    
    public bool IsDead { get => _isDead; set => _isDead = value; }
    public bool CanSlow { get => _canSlow; set => _canSlow = value; }



    private void Update() 
    {
        if(Time.timeScale >=1)
        {
            transform.Translate(Vector3.down * Time.deltaTime * moveSpeed, Space.World);
        }
        else
        {
            transform.Translate(Vector3.down * Time.deltaTime, Space.World);
        }
    }
    public void Dead()
    {
        if(_isDead) return;

        _isDead=  true;
        Instantiate(deadPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
