using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : MonoBehaviour
{
    public GameObject rocket;
    private int numRockets = 6;
    private float fireRate = 3f;
    // Start is called before the first frame update
    void Start()
    {
        FireRockets();
        StartCoroutine(FireRocketsRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FireRocketsRoutine()
    {
        yield return new WaitForSeconds(fireRate);
        FireRockets();
        StartCoroutine(FireRocketsRoutine());
    }

    private void FireRockets()
    {
        float relativeAngle = Random.Range(0f, 90f);
        for (int i = 0; i < numRockets; i++)
        {
            // Evenly distribute rockets around the player relative to a random rotation
            float rotation = (360 / (float)numRockets) * i + relativeAngle;
            Instantiate(rocket, gameObject.transform.position, Quaternion.Euler(0, rotation, 0));
        }
    }
}
