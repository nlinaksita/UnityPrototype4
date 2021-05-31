using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject[] powerupPrefabs;
    public int enemyCount;
    public int waveNumber = 1;
    private float spawnRange = 9;
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(waveNumber);
        int powerupIndex = Random.Range(0, powerupPrefabs.Length);
        //Debug.Log(powerupIndex);
        Instantiate(powerupPrefabs[powerupIndex], 
            GenerateSpawnPosition(), 
            powerupPrefabs[powerupIndex].transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            // Increase the wave number
            waveNumber++;
            SpawnEnemyWave(waveNumber);

            // Only spawn a powerup if there is not one already on the field
            if (FindObjectsOfType<Powerup>().Length == 0) { 
                int powerupIndex = Random.Range(0, powerupPrefabs.Length);
                //Debug.Log(powerupIndex);
                Instantiate(powerupPrefabs[powerupIndex], 
                    GenerateSpawnPosition(), 
                    powerupPrefabs[powerupIndex].transform.rotation);
            }
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab,
            GenerateSpawnPosition(),
            enemyPrefab.transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }
}
