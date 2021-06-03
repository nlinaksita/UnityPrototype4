using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : Character
{
    private int numRockets = 6;
    private float fireRate = 3f;
    // Start is called before the first frame update
    void Start()
    {
        FireRockets(numRockets);
        StartCoroutine(FireRocketsRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FireRocketsRoutine()
    {
        yield return new WaitForSeconds(fireRate);
        FireRockets(numRockets);
        StartCoroutine(FireRocketsRoutine());
    }
}
