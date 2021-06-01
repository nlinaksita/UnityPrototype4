using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStomper : MonoBehaviour
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

        // Stop velocity
        enemyRb.velocity = Vector3.zero;
        enemyRb.angularVelocity = Vector3.zero;

        // Reset rotation
        transform.rotation.eulerAngles.Set(0, 0, 0);

        // Apply upward force
        enemyRb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

        StartCoroutine(StompDown());
    }

    // Player stomps down after short delay
    private IEnumerator StompDown()
    {
        yield return new WaitForSeconds(0.3f);
        // Apply downward force
        enemyRb.AddForce(Vector3.down * stompPower, ForceMode.Impulse);

        StartCoroutine(StompPushEnemies());
    }

    // Stomp applies force to enemies if they are within range
    private IEnumerator StompPushEnemies()
    {
        yield return new WaitForSeconds(0.2f);

        // Iterate over enemies and check if they are within stompRange
        PlayerController player = FindObjectOfType<PlayerController>();
        
        Vector3 awayFromEnemy = (player.transform.position - transform.position);
        // calculate the distance from the enemy to the player
        float distance = Mathf.Abs((awayFromEnemy).magnitude);
        //Debug.Log(distance);
        if (distance <= stompRange)
        {
            // apply force to enemy (enemies further away will be affected less than those closer)
            float stompMagPower = Mathf.Abs(distance - stompRange) / stompRange;
            //Debug.Log(stompMagPower * powerupStrength);

            player.GetComponent<Rigidbody>().AddForce(awayFromEnemy * powerupStrength
                * stompMagPower, ForceMode.Impulse);
        }
        

        // reset stomping flag
        isStomping = false;

        StartCoroutine(StompUp());
    }
}
