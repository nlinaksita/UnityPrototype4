using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArcher : Enemy
{
    private int numRockets = 6;
    private float fireRate = 3f;
    private float archerSpeed = 1f;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        speed = archerSpeed;
        StartCoroutine(FireRocketsRoutine());
    }


    IEnumerator FireRocketsRoutine()
    {
        yield return new WaitForSeconds(fireRate);
        FireRockets(numRockets);
        StartCoroutine(FireRocketsRoutine());
    }
}
