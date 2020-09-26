using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStatus
{
    next, play, gameover, win
}

public class GameManager : Singleton<GameManager> 
{
    //public static GameManager instance = null;
    [SerializeField]
    private int totalWaves = 10;
    [SerializeField]
    private Text totalMoneyLbl;
    [SerializeField]
    private Text currentWaveLbl;
    [SerializeField]
    private Text totalEscapedLbl;
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private int maxEnemiesOnScreen;
    [SerializeField]
    private int totalEnemies;
    [SerializeField]
    private int enemiesPerSpawn;
    [SerializeField]
    private Text playBtnLbl;
    [SerializeField]
    private Button playBtn;

    private int waveNumber = 0;
    private int totalMoney = 10;
    private int totalEscaped = 0;
    private int roundEscaped = 0;
    private int totalKilled = 0;
    private int whichEnemiesToSpawn = 0;
    private gameStatus currentState = gameStatus.play;

    public List<Enemy> EnemyList = new List<Enemy>();

    const float spawnDelay = 0.5f;

    public int TptalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLbl.text = totalMoney.ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playBtn.gameObject.SetActive(false);
        showMenu();

    }

    void Update()
    {
        handleEscape();
    }

    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count >= maxEnemiesOnScreen)
                    break;
                
                GameObject newEnemy = Instantiate(enemies[0]);
                newEnemy.transform.position = spawnPoint.transform.position;
                    //break;
            }
        }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
    }

    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyAllEnemies()
    {
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }

        EnemyList.Clear();
    }

    public void addMoney(int amount)
    {
        totalMoney += amount;
    }

    public void subtractMoney(int amount)
    {
        totalMoney -= amount;
    }

    public void showMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                playBtnLbl.text = "Play Again";
                // add gameover sounds
                break;
            case gameStatus.next:
                playBtnLbl.text = "Next Wave";
                break;
            case gameStatus.play:
                playBtnLbl.text = "Play";
                break;
            case gameStatus.win:
                playBtnLbl.text = "Play";
                break;
        }

        playBtn.gameObject.SetActive(true);
    }

    private void handleEscape()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.disableDragSprite();
            TowerManager.Instance.towerBtnPressed = null;
        }
    }
}
