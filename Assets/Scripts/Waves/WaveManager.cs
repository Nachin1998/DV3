using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public enum WaveState
    {
        CountDown,
        Spawning,
        ActiveWave
    }
    [HideInInspector] public WaveState state = WaveState.CountDown;

    public Wave[] waves;
    [Space]
    public float timeBetweenWaves = 5f;

    [HideInInspector]public int totalEnemies = 0;

    float searchingLiveEnemiesCountdown = 1f;
    public float waveCountdown = 0f;
    [HideInInspector]public int currentWave = 0;
    int nextWave = 0;

    bool playSound = false;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        if(GameManager.Instance.won)
        {
            return;
        }

        if (state == WaveState.ActiveWave)
        {
            totalEnemies = FindObjectsOfType<BaseEnemy>().Length;

            if (!AreEnemiesAlive())
            {
                EndWave();
            }
            return;
        }
        
        if (waveCountdown <= 0)
        {
            if (!playSound)
            {
                AkSoundEngine.PostEvent("play_ambient", gameObject);
                playSound = true;
            }

            if (state != WaveState.ActiveWave)
            {
                waveCountdown = 0;
                if (state != WaveState.Spawning)
                {
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }           
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }        
    }

    void EndWave()
    {
        state = WaveState.CountDown;
        waveCountdown = timeBetweenWaves;
        totalEnemies = 0;
        playSound = false;
        if (nextWave + 1 > waves.Length - 1)
        {
            GameManager.Instance.won = true;
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
            if (totalEnemies == 0)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave wave)
    {        
        state = WaveState.Spawning;

        for (int i = 0; i < wave.enemiesToSpawn.Count; i++)
        {
            for (int j = 0; j < wave.enemiesToSpawn[i].enemyAmmount; j++)
            {
                SpawnEnemy(wave.enemiesToSpawn[i].enemy, wave.spawnPoints[Random.Range(0, wave.spawnPoints.Count)]);
                yield return new WaitForSeconds(1 / wave.enemiesPerSecond);
            }
        }

        state = WaveState.ActiveWave;
    }

    void SpawnEnemy(BaseEnemy enemy, Transform spawnPoint)
    {
        Instantiate(enemy, spawnPoint.position, Quaternion.identity, transform);
    }
}