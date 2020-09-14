﻿using System.Collections;
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

    public List<Enemy> EnemyList = new List<Enemy>();

    const float spawnDelay = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());

    }

    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < maxEnemiesOnScreen)
                {    //break;

                    GameObject newEnemy = Instantiate(enemies[0]);
                    newEnemy.transform.position = spawnPoint.transform.position;
                    //break;
                }
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
}
