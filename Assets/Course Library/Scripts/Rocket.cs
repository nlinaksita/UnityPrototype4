using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject target;
    private Rigidbody rocketRb;
    private const int xBounds = 20;
    private const int yBounds = 10;
    private const int zBounds = 30;
    private float powerupStrength = 30f;
    private Vector3 relativePos;
    private float rocketSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        rocketRb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the target has fallen off the stage and has been deleted, delete the rocket
        if (target == null)
            Destroy(gameObject);
        else
        {
            relativePos = target.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(relativePos);
            // move object forward
            transform.Translate(Vector3.forward * Time.deltaTime * rocketSpeed);

            // Destroy the rocket if it is out of bounds
            if (transform.position.x < -xBounds || transform.position.x > xBounds ||
                transform.position.y < -yBounds || transform.position.y > yBounds ||
                transform.position.z < -zBounds || transform.position.z > zBounds)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy")) 
        {
            //Debug.Log("Rocket hit an enemy!");
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            Destroy(gameObject);
        }

    }
     
}
