﻿using System.Collections;
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
        player = GameObject.Find("Player");
        enemyRb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        enemyRb.AddForce((player.transform.position - gameObject.transform.position).normalized * speed);
        if (gameObject.transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
