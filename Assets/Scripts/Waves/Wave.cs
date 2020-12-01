using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Waves", fileName = "Wave")]
public class Wave : ScriptableObject
{
    [System.Serializable]
    public class EnemyToSpawn
    {
        public string name;
        public BaseEnemy enemy;
        public int enemyAmmount;
    }

    public string name;
    public List<EnemyToSpawn> enemiesToSpawn;
    public float enemiesPerSecond;
    public List <Transform> spawnPoints;
}
