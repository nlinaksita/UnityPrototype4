using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStomper : Character
{
    private bool isStomping = false;
    private Rigidbody enemyRb;
    private float powerupStrength = 8f;
    private float jumpPower = 50f;
    private float stompPower = 100f;
    private float stompRange = 7f;
    private float stompRate = 5f;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = gameObject.GetComponent<Rigidbody>();
        StartCoroutine(StompUp());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StompUp()
    {
        yield return new WaitForSeconds(stompRate);
        // Set flag so that cannot interrupt stomp with another stomp
        isStomping = true;
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
