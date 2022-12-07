using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState state;
    public  Camera cam;
    public  bool isOnMobile;

    [SerializeField] private  Enemy[] enemiPbs;
    [SerializeField] private  int waveLevel = 3;
    [SerializeField] private  int enemyLevel = 3;
    [SerializeField] private  int enemyUpLevel = 2;
    [SerializeField] private  int bulletExtra = 1;
    [SerializeField] private  float timeDownLevel = 0.0001f;

    private List<Enemy> _enemySpawned;
    private int _killed;
    private int _level =1;
    private int _Wave = 1;
    private int _score;

    private bool _isSlowed;
    private bool _isBeginSlow;

    public int Killed { get => _killed; set => _killed = value; }
    public bool IsSlowed { get => _isSlowed; set => _isSlowed = value; }
    public int Socre { get => _score; }

    public override void Awake()
    {
        MakeSingleton(false);
        _enemySpawned = new List<Enemy>();
    }

    public override void Start()
    {
        state = GameState.Starting;

        if(AudioController.Ins)
        {
            AudioController.Ins.PlayBackgroundMusic();
        }
    }

    private void Update() 
    {
        if (state != GameState.Playing) return;

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

                if(GUI.Ins)
                {
                    GUI.Ins.UpdateLevel(_level);
                    if (GUI.Ins.waveCompeletedDialog)
                    {
                        GUI.Ins.waveCompeletedDialog.Show(true);
                    }
                }
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

        if(Time.timeScale >= 0.9f && _isSlowed && !canSLow() && _killed < _enemySpawned.Count && 
            state != GameState.GameOver)
        {
            state = GameState.GameOver;

            if(GUI.Ins && GUI.Ins.gameoverDialog)
            {
                GUI.Ins.gameoverDialog.Show(true);
            }
            _score = 0;

            Debug.Log("Game Over!!!!!!");
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

        if(GUI.Ins)
        {
            GUI.Ins.UpdateBullet(Player.Ins.Bullet);
        }
    }

    public void ResetData()
    {
        _isSlowed = false;
        _isBeginSlow = false;
        _killed = 0;

        if (state == GameState.GameOver)
        {
            _Wave = 1;
            _level = 1;
        }

        state = GameState.Starting;

        if (_enemySpawned == null || _enemySpawned.Count <= 0) return;

        for (int i = 0; i < _enemySpawned.Count; i++)
        {
            var enemy = _enemySpawned[i];
            if (enemy)
            {
                Destroy(enemy.gameObject);
            }

        }
        _enemySpawned.Clear();

    }

    public void NextLevel()
    {
        if(state == GameState.WaveCompleted)
        {
            Timer.Schedule(this, 1f, () =>
            {
                _Wave = 1;
                ResetData();
                Spawn();
                state = GameState.Playing;
            });
        }

        if (AudioController.Ins)
        {
            AudioController.Ins.PlayBackgroundMusic();
        }
    }

    public void StarGame()
    {
        ResetData();
        Timer.Schedule(this, 1f, () =>
        {
            Spawn();
            state= GameState.Playing;
        });

        if (GUI.Ins)
        {
            GUI.Ins.UpdateLevel(_level);
            GUI.Ins.UpdateBullet(Player.Ins.Bullet);
            GUI.Ins.ShowGamePlay(true);
        }

        if (AudioController.Ins)
        {
            AudioController.Ins.PlayBackgroundMusic();
        }
    }

    public void AddScore()
    {
        _score++;
        Pref.bestScore = _score;
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
