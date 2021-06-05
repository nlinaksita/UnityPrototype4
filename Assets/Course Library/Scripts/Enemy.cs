using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public Rigidbody enemyRb;
    GameObject player;
    public float speed;
    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyRb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            enemyRb.AddForce((player.transform.position - gameObject.transform.position).normalized * speed);
            if (gameObject.transform.position.y < -10)
            {
                Destroy(gameObject);
            }
        }
    }
}
