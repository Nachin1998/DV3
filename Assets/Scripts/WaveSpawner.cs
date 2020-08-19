using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string name;
        public Enemy enemy;
        public int enemyAmmount;
        public float spawnRate;
    }

    public enum SpawnState
    {
        Spawning, 
        Waiting, 
        Counting
    }
    SpawnState state = SpawnState.Counting;

    public Wave[] waves;
    public Transform[] spawnPoints;
    [Space]
    public float timeBetweenWaves = 5f;
    public float waveCountdown = 0f;

    float searchingLiveEnemiesCountdown = 1f;
    int nextWave = 0;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        if(state == SpawnState.Waiting)
        {
            if (!isEnemyAlive())
            {
                EndWave();
                return;
            }
            else
            {
                return;
            }
        }
        if(waveCountdown <= 0)
        {
            if(state != SpawnState.Spawning)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void EndWave()
    {
        Debug.Log("Wave Finished");

        state = SpawnState.Counting;
        waveCountdown = timeBetweenWaves;

        if(nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("All waves completed! Starting over");
        }
        else
        {
            nextWave++;              
        }
    }

    bool isEnemyAlive()
    {
        searchingLiveEnemiesCountdown -= Time.deltaTime;

        if(searchingLiveEnemiesCountdown <= 0)
        {
            searchingLiveEnemiesCountdown = 1f;
            if(GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("Spawning wave...");
        state = SpawnState.Spawning;

        for (int i = 0; i < wave.enemyAmmount; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1 / wave.spawnRate);
        }

        state = SpawnState.Waiting;
    }

    void SpawnEnemy(Enemy enemy)
    {
        Debug.Log("Spawning enemy");
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemy, spawnPoint.position, Quaternion.identity, transform);
    }
   
}
