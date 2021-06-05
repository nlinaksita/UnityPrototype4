using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public GameObject[] powerupPrefabs;
    public int enemyCount;
    public static int waveNumber = 1;
    public Text waveNumberText;
    private float spawnRange = 9;
    private float powerupSpawnTime = 7f;
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(waveNumber);
        StartCoroutine(SpawnPowerup());
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            // Increase the wave number
            waveNumber++;
            waveNumberText.text = "Wave Number: " + waveNumber;
            SpawnEnemyWave(waveNumber);
        }
    }

    IEnumerator SpawnPowerup()
    {
        yield return new WaitForSeconds(powerupSpawnTime);
        if (FindObjectsOfType<Powerup>().Length == 0)
        {
            //SpawnPowerup();
            // Spawn a new powerup
            int powerupIndex = Random.Range(0, powerupPrefabs.Length);
            //Debug.Log(powerupIndex);
            Instantiate(powerupPrefabs[powerupIndex],
                GenerateSpawnPosition(),
                powerupPrefabs[powerupIndex].transform.rotation);
        }
        StartCoroutine(SpawnPowerup());
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
