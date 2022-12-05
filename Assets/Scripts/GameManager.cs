using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState state;
    public  Camera cam;
    public  bool isOnMobile;

    [SerializeField] private  Enemy[] enemiPbs;
    [SerializeField] private  int waveLevel;
    [SerializeField] private  int enemyLevel;
    [SerializeField] private  int enemyUpLevel;
    [SerializeField] private  int bulletExtra;
    [SerializeField] private  float timeDownLevel;

    private List<Enemy> _enemySpawned;
    private int _killed;
    private int _level =1;
    private int _Wave = 1;
    private int socre;

    private bool _isSlowed;
    private bool _isBeginSlow;

    public int Killed { get => _killed; set => _killed = value; }
    public bool IsSlowed { get => _isSlowed; set => _isSlowed = value; }
    public int Socre { get => socre; }

    public override void Awake()
    {
        MakeSingleton(false);
        _enemySpawned = new List<Enemy>();
    }

    public override void Start()
    {
        this.Spawn();
    }

    private void Update() 
    {
        if (canSLow() && !_isBeginSlow)
        {
            _isBeginSlow = true;
            float delay = Random.Range(0.01f , 0.05f);
            Timer.Schedule(this, delay, () => 
            {
                SlowController.Ins.DoSlowmotion();
            }, true);
        }

        if(Time.timeScale < 1 && _killed >= _enemySpawned.Count && state != GameState.WaveCompleted)
        {
            // tinh xem choi het cai wave trong 1 level chua
            if(_Wave % waveLevel == 0)
            {
                state = GameState.WaveCompleted;
                enemyLevel += enemyUpLevel;
                _level++;
                Debug.Log("Yoy Thắng");              
            }
            else
            {
                ResetData();
                Spawn();
                _Wave++;
                state = GameState.Playing;
            }
        }
    }

    public void Spawn()
    {
        if (!SlowController.Ins || !Player.Ins) return;
                
        SlowController.Ins.slowdownLength = (enemyLevel / 2 +1.5f) - timeDownLevel * _Wave;

        Player.Ins.Bullet = enemyLevel + bulletExtra;
        if(enemiPbs == null || enemiPbs.Length <=0) return;

        for (int i = 0; i < enemyLevel; i++)
        {
            int randIdx = Random.Range(0, enemiPbs.Length);

            var enemiPb = enemiPbs[randIdx];
            
            float spawnPosX = Random.Range(-8,8);
            float spawnPosY = Random.Range(7f,8.5f);

            Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, 0f);

            if (enemiPb)
            {
                var enemyClone = Instantiate(enemiPb, spawnPos, Quaternion.identity);
                _enemySpawned.Add(enemyClone);
            }
        }
    }

    public void ResetData()
    {
        _isSlowed = false;
        _isBeginSlow = false;
        _killed = 0;
        _enemySpawned.Clear();

        if (state == GameState.GameOver)
        {
            _Wave = 1;
            _level = 1;
        }

        state = GameState.Starting;
    }

    private bool canSLow()
    {
        if(_enemySpawned == null || _enemySpawned.Count <=0) return false;

        int check = 0;

        for (int i = 0; i < _enemySpawned.Count; i++)
        {
            var enemy = _enemySpawned[i];

            if (enemy && enemy.CanSlow)
            {
                check ++;
            }
        }

        if(check == _enemySpawned.Count)
        {
            return true;
        }
        return false;

    }
}
