using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> 
{
    //public static GameManager instance = null;
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private GameObject [] enemies;
    [SerializeField]
    private int maxEnemiesOnScreen;
    [SerializeField]
    private int totalEnemies;
    [SerializeField]
    private int enemiesPerSpawn;

    private int enemiesOnScreen = 0;
    const float spawnDelay = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());

    }

    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && enemiesOnScreen < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (enemiesOnScreen >= maxEnemiesOnScreen)
                    break;
                
                GameObject newEnemy = Instantiate(enemies[0]);
                newEnemy.transform.position = spawnPoint.transform.position;
                enemiesOnScreen += 1;
                
                    //break;
            }
        }

            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
    }

    public void RemoveEnemyFromScreen()
    {
        if (enemiesOnScreen > 0)
            enemiesOnScreen -= 1;
    }
}
