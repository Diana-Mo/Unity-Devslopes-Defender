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
    private Enemy[] enemies;
    //[SerializeField]
    private int maxEnemiesOnScreen;
    [SerializeField]
    private int totalEnemies = 3;
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
    private int enemiesToSpawn = 0;
    private gameStatus currentState = gameStatus.play;
    private AudioSource audioSource;

    public List<Enemy> EnemyList = new List<Enemy>();

    private bool readyToSpawn = true;


    const float spawnDelay = 1.0f;

    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }
    }

    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set
        {
            roundEscaped = value;
        }
    }

    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }
        set
        {
            totalKilled = value;
        }
    }

    public int TotalMoney
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

    public AudioSource AudioSource
    {
        get
        {
            return audioSource;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playBtn.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        showMenu();

    }

    void Update()
    {
        handleEscape();
        if (readyToSpawn && !playBtn.gameObject.activeInHierarchy)
        {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        readyToSpawn = false;
        while (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count >= totalEnemies)
                    break;

                //GameObject newEnemy = Instantiate(enemies[0]); //create enemies
                Enemy newEnemy = Instantiate(enemies[Random.Range(0, enemiesToSpawn)]);
                newEnemy.transform.position = spawnPoint.transform.position; //created enemies spawn at the spawnPoint
                                                                             //break;
                yield return new WaitForSeconds(spawnDelay/3);
            }
            yield return new WaitForSeconds(spawnDelay);
        }
        readyToSpawn = true;
        //yield return new WaitForSeconds(spawnDelay);
        //StartCoroutine(Spawn());
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
        //going through our registered list of enemies to destroy them
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }

        EnemyList.Clear();
    }

    public void addMoney(int amount)
    {
        TotalMoney += amount;
    }

    public void subtractMoney(int amount)
    {
        TotalMoney -= amount;
    }

    public void isWaveOver()
    {
        totalEscapedLbl.text = "Escaped " + TotalEscaped + "/10";
        if ((RoundEscaped + TotalKilled) == totalEnemies)
        {
            if (waveNumber <= enemies.Length)
            {
                enemiesToSpawn = waveNumber;
            }
            setCurrentGameState();
            showMenu();
        }
    }



    public void setCurrentGameState()
    {
        if (TotalEscaped >= 10)
        {
            currentState = gameStatus.gameover;
        }
        else if (waveNumber == 0 && (TotalKilled + RoundEscaped) == 0)
        {
            currentState = gameStatus.play;
        }
        else if (waveNumber >= totalWaves)
        {
            currentState = gameStatus.win;
        }
        else
        {
            currentState = gameStatus.next;
        }
    }

    public void showMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                playBtnLbl.text = "Play Again";
                AudioSource.PlayOneShot(SoundManager.Instance.GameOver); 
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

    public void playBtnPressed ()
    {
        //StopAllCoroutines();
        //Debug.Log("you pressed it <3");
        switch(currentState)
        {
            case gameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                enemiesPerSpawn = 1 + waveNumber / 2;
                //StartCoroutine(Spawn());
                break;
            default:
                //maxEnemiesOnScreen
                //totalEnemies
                //max enemies on Screen is 10 + total enemies
                //total enemies is += wave Number
                totalEnemies = 3;
                TotalEscaped = 0;
                TotalMoney = 10;
                enemiesToSpawn = 0;
                TowerManager.Instance.DestroyAllTower();
                TowerManager.Instance.RenameTagsBuildSites();
                totalMoneyLbl.text = totalMoney.ToString();
                totalMoneyLbl.text = TotalMoney.ToString();
                totalEscapedLbl.text = "Escaped " + TotalEscaped + "/10";
                audioSource.PlayOneShot(SoundManager.Instance.NewGame);
                //StartCoroutine(Spawn());
                //reset towers
                break;
        }
        DestroyAllEnemies();
        TotalKilled = 0;
        RoundEscaped = 0;
        currentWaveLbl.text = "Wave " + (waveNumber + 1);
        playBtn.gameObject.SetActive(false);
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
