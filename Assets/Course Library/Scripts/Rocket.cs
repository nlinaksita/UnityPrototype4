using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody rocketRb;
    private const int xBounds = 20;
    private const int zBounds = 30;
    private float powerupStrength = 30f;
    // Start is called before the first frame update
    void Start()
    {
        rocketRb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // move object forward
        transform.Translate(Vector3.forward * Time.deltaTime * 20);

        // Destroy the rocket if it is out of bounds
        if (transform.position.x < -xBounds || transform.position.x > xBounds || 
            transform.position.z < -zBounds || transform.position.z > zBounds)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy")) 
        {
            Debug.Log("Rocket hit an enemy!");
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }

    }
     
}
