﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public GameObject[] powerupPrefabs;
    public int enemyCount;
    public int waveNumber = 1;
    private float spawnRange = 9;
    private float powerupSpawnTime = 7f;
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(waveNumber);
        SpawnPowerup();
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
                SpawnPowerup();
            }
            // if the wave did not start with a powerup, start a coroutine to spawn one within
            // a certain time
            //else
            //{
            //    StartCoroutine(SpawnPowerupDelay());
            //}
        }
    }

    void SpawnPowerup()
    {
        int powerupIndex = Random.Range(0, powerupPrefabs.Length);
        //Debug.Log(powerupIndex);
        Instantiate(powerupPrefabs[powerupIndex],
            GenerateSpawnPosition(),
            powerupPrefabs[powerupIndex].transform.rotation);
    }

    IEnumerator SpawnPowerupDelay()
    {
        yield return new WaitForSeconds(powerupSpawnTime);
        SpawnPowerup();
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int enemyIndex = Random.Range(0, enemyPrefab.Length);
            Instantiate(enemyPrefab[enemyIndex],
            GenerateSpawnPosition(),
            enemyPrefab[enemyIndex].transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }

    public void StopMinionSpawn()
    {
        //StopCoroutine(SpawnMinions());
    }
}
