using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public GameObject viewFiderPb;
    private int _bullet;
    private Camera _cam;
    private GameObject _viewFinderClone;

    public int Bullet { get => _bullet; set => _bullet = value; }

    public override void Awake() 
    {
        MakeSingleton(false);
        _bullet =10;
    }

    public override void Start()
    {
        if(!GameManager.Ins) return;

        _cam = GameManager.Ins.cam;

        if ( GameManager.Ins.isOnMobile || !viewFiderPb ) return;

        _viewFinderClone = Instantiate(viewFiderPb, new Vector3(100,100,0f), Quaternion.identity );
    }
    
    private void Update() 
    {
        if (!_cam) return;

        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        _viewFinderClone.transform.position = new Vector3(mousePos.x, mousePos.y, 0f);  

        if(Input.GetMouseButtonDown(0))
        {
            this.shoot(mousePos); 
        }
        
    }

    private void shoot(Vector3 mousePos)
    {
        if(_bullet <=0) return;

        _bullet--;

        Vector3 shootingDir = _cam.transform.position - mousePos;
        shootingDir.Normalize();
        //lay ra mang[] toan bo vat bi tia Raycast chieu trung duoi dang physic2D
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, shootingDir, 0.1f);

        if(hits == null || hits.Length <=0) return;
        for (int i = 0; i < hits.Length; i++)
        {
            var hitted = hits[i];

            if (!hitted.collider) continue;
           
            Debug.Log(hitted.collider.gameObject.name);

            var enemy = hitted.collider.GetComponent<Enemy>();
            if(enemy)
            {
                enemy.Dead();
            }
        }
    }
}
