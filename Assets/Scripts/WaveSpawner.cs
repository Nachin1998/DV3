using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string name;
        public Enemy enemy;
        public int enemyAmmount;
        public float enemiesPerSecond;
    }

    public enum SpawnState
    {
        CountDown,
        Spawning,
        ActiveWave
    }
    SpawnState state = SpawnState.CountDown;

    public Wave[] waves;
    public Transform[] spawnPoints;
    [Space]
    public float timeBetweenWaves = 5f;
    [Space]
    public TextMeshProUGUI enemyAmmountText;
    public TextMeshProUGUI waveStateText;

    int totalEnemies = 0;
    float searchingLiveEnemiesCountdown = 1f;
    float waveCountdown = 0f;
    int currentWave = 0;
    int nextWave = 0;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        if (state == SpawnState.ActiveWave)
        {
            waveStateText.text = "Wave " + (currentWave + 1).ToString();
            enemyAmmountText.text = "x " + totalEnemies.ToString();
            if (!AreEnemiesAlive())
            {
                EndWave();
            }
            return;
        }
        
        if (waveCountdown <= 0)
        {
            waveStateText.text = "";
            if (state != SpawnState.ActiveWave)
            {
                waveCountdown = 0;
                if (state != SpawnState.Spawning)
                {
                    enemyAmmountText.text = "Spawning enemies";
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }           
        }
        else
        {
            waveCountdown -= Time.deltaTime;

            enemyAmmountText.text = "All enemies defeated";
            waveStateText.text = "Next wave in: " + waveCountdown.ToString("F2");
        }        
    }

    void EndWave()
    {
        Debug.Log("Wave Finished");

        state = SpawnState.CountDown;
        waveCountdown = timeBetweenWaves;
        totalEnemies = 0;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            currentWave = 0;
            Debug.Log("All waves completed! Starting over");
        }
        else
        {
            nextWave++;
            currentWave++;
        }
    }

    bool AreEnemiesAlive()
    {
        searchingLiveEnemiesCountdown -= Time.deltaTime;

        if (searchingLiveEnemiesCountdown <= 0)
        {
            searchingLiveEnemiesCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
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

        //totalEnemies = wave.enemyAmmount;

        for (int i = 0; i < wave.enemyAmmount; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1 / wave.enemiesPerSecond);
        }

        state = SpawnState.ActiveWave;
    }

    void SpawnEnemy(Enemy enemy)
    {
        Debug.Log("Spawning enemy");
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemy, randomSpawnPoint.position, Quaternion.identity, randomSpawnPoint.transform);
        totalEnemies++;
    }

}
