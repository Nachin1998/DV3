using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    /*[System.Serializable]
    public class Wave
    {
        public string name;
        public List<BaseEnemy> enemy;
        public List<int> enemyAmmount;
        public float enemiesPerSecond;
    }*/

    public enum SpawnState
    {
        CountDown,
        Spawning,
        ActiveWave
    }
    [HideInInspector] public SpawnState state = SpawnState.CountDown;

    public Wave[] waves;
    [Space]
    public float timeBetweenWaves = 5f;
    [Space]
    public GameObject enemyAmmount;
    public TextMeshProUGUI waveStateText;

    int totalEnemies = 0;

    float searchingLiveEnemiesCountdown = 1f;
    float waveCountdown = 0f;
    int currentWave = 0;
    int nextWave = 0;

    TextMeshProUGUI enemyAmmountText;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
        enemyAmmountText = enemyAmmount.GetComponentInChildren<TextMeshProUGUI>();

        enemyAmmount.gameObject.SetActive(false);
    }

    void Update()
    {
        if(GameManager.Instance.won)
        {
            return;
        }

        if (state == SpawnState.ActiveWave)
        {
            totalEnemies = FindObjectsOfType<BaseEnemy>().Length;

            waveStateText.gameObject.SetActive(false);
            enemyAmmount.SetActive(true);
            enemyAmmountText.text = "x " + totalEnemies.ToString();

            if (!AreEnemiesAlive())
            {
                EndWave();
            }
            return;
        }
        
        if (waveCountdown <= 0)
        {
            waveStateText.text = "Spawning Nightmares...";
            if (state != SpawnState.ActiveWave)
            {
                waveCountdown = 0;
                if (state != SpawnState.Spawning)
                {
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }
            }           
        }
        else
        {
            waveStateText.gameObject.SetActive(true);
            enemyAmmount.gameObject.SetActive(false);
            waveCountdown -= Time.deltaTime;

            //enemyAmmountText.text = "All enemies defeated";
            waveStateText.text = "Next wave in: " + waveCountdown.ToString("F2");
        }        
    }

    void EndWave()
    {
        state = SpawnState.CountDown;
        waveCountdown = timeBetweenWaves;
        totalEnemies = 0;

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
        
        state = SpawnState.Spawning;

        for (int i = 0; i < wave.enemy.Count; i++)
        {
            for (int j = 0; j < wave.enemyAmmount[i]; j++)
            {
                SpawnEnemy(wave.enemy[i], wave.spawnPoint);
                yield return new WaitForSeconds(1 / wave.enemiesPerSecond);
            }
        }        

        state = SpawnState.ActiveWave;
    }

    void SpawnEnemy(BaseEnemy enemy, Transform spawnPoint)
    {
        Instantiate(enemy, spawnPoint.position, Quaternion.identity, spawnPoint);
    }
}
