using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    public GameObject player;
    //private float powerupStrength = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            float powerupStrength = player.GetComponent<PlayerController>().PowerupStrength;
            Vector3 awayFromPlayer = other.transform.position - player.transform.position;
            other.GetComponent<Rigidbody>().AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
        FindObjectOfType<AudioManager>().Play("ForceFieldContact");
    }
}
