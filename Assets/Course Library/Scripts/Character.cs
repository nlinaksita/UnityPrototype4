using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject rocket;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // rockets: Number of rockets
    public void FireRockets(int rockets)
    {
        float relativeAngle = Random.Range(0f, 90f);
        for (int i = 0; i < rockets; i++)
        {
            // Evenly distribute rockets around the player relative to a random rotation
            float rotation = (360 / (float)rockets) * i + relativeAngle;
            Instantiate(rocket, gameObject.transform.position, Quaternion.Euler(0, rotation, 0));
        }
    }
}
