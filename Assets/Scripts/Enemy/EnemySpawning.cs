using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
 
public class EnemySpawning : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount;
    public int waveNumber = 1;
    public GameObject[] spawnPoints;
    public TextMeshProUGUI waveText;

    void Start()
    {
        SpawnEnemyWave(waveNumber);
    }
 
    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        
        waveText.text = "Wave: " + waveNumber;
        
        if (enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
        }
    }
 
    private Vector2 GenerateSpawnPoint()
    {
        int prefabIndex = Random.Range(0,4);
        GameObject pos;

        pos = Instantiate(spawnPoints[prefabIndex]);
        
        return pos.transform.position;
    }
 
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPoint(), enemyPrefab.transform.rotation);
        }
    }
}