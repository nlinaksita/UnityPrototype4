using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStomper : Enemy
{
    private float stomperSpeed = 2.5f;
    private float powerupStrength = 8f;
    private float jumpPower = 50f;
    private float stompPower = 100f;
    private float stompRange = 7f;
    private float stompRate = 5f;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        speed = stomperSpeed;
        StartCoroutine(StompUp());
    }

    IEnumerator StompUp()
    {
        yield return new WaitForSeconds(stompRate);
        StompUp(enemyRb,
            jumpPower,
            stompPower,
            powerupStrength,
            stompRange,
            TYPE_ENEMY
            );
        StartCoroutine(StompUp());
    }
}
