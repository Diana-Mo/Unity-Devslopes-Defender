using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public GameObject spawnPoint;
    public GameObject [] enemies;
    public int maxEnemiesOnScreen;
    public int totalEnemies;
    public int enemiesPerSpawn;

    private int enemiesOnScreen = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemy();

    }

    void SpawnEnemy()
    {
        if (enemiesPerSpawn <= 0 || enemiesOnScreen >= totalEnemies)
            return;

        for(int i = 0; i < enemiesPerSpawn; i++)
        {
            if (enemiesOnScreen >= maxEnemiesOnScreen)
                break;

            GameObject newEnemy = Instantiate(enemies[0]);
            newEnemy.transform.position = spawnPoint.transform.position;
            enemiesOnScreen += 1;
        }
    }

    public void RemoveEnemyFromScreen()
    {
        if (enemiesOnScreen > 0)
            enemiesOnScreen -= 1;
    }
}
