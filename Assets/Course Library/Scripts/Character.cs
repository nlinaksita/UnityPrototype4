using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject rocket;
    public const int TYPE_PLAYER = 0;
    public const int TYPE_ENEMY = 1;
    protected bool isStomping;
    // Start is called before the first frame update
    void Start()
    {
        isStomping = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // rockets: Number of rockets
    // FireRockets enemy version where rockets are not homing rockets
    public void FireRockets(int rockets)
    {
        float relativeAngle = Random.Range(0f, 90f);
        for (int i = 0; i < rockets; i++)
        {
            // Evenly distribute rockets around the player relative to a random rotation
            float rotation = (360 / (float)rockets) * i + relativeAngle;
            Instantiate(rocket, gameObject.transform.position, Quaternion.Euler(0, rotation, 0));
        }
        FindObjectOfType<AudioManager>().Play("ShooterShoot");
    }

    public void FireRockets()
    {
        Enemy[] targets = FindObjectsOfType<Enemy>();

        for (int i = 0; i < targets.Length; i++)
        {
            // Calculate the relative position
            Vector3 relativePos = targets[i].transform.position - transform.position;
            GameObject instantiated = Instantiate(rocket, gameObject.transform.position, Quaternion.LookRotation(relativePos));
            Rocket r = instantiated.gameObject.GetComponent<Rocket>();
            r.target = targets[i].gameObject;
        }
        FindObjectOfType<AudioManager>().Play("PlayerShoot");
    }

    // Stomping methods
    // rb = rigidbody, jp = jumping power, sp = stomping power, ps = powerup strength,
    // sr = stomp range, type = 0: rb is player, type = 1: rb is enemy
    public void StompUp(Rigidbody rb, float jp, float sp, float ps, float sr, int type)
    {
        isStomping = true;

        // Stop velocity
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Reset rotation
        transform.rotation.eulerAngles.Set(0, 0, 0);

        // Apply upward force
        rb.AddForce(Vector3.up * jp, ForceMode.Impulse);

        switch (type) {
            case TYPE_PLAYER:
                FindObjectOfType<AudioManager>().Play("PlayerJump");
                break;
            case TYPE_ENEMY:
                FindObjectOfType<AudioManager>().Play("StomperJump");
                break;
        }

        StartCoroutine(StompDown(rb, sp, ps, sr, type));
    }

    private IEnumerator StompDown(Rigidbody rb, float sp, float ps, float sr, int type)
    {
        yield return new WaitForSeconds(0.3f);
        // Apply downward force
        rb.AddForce(Vector3.down * sp, ForceMode.Impulse);

        StartCoroutine(StompPushEnemies(rb, ps, sr, type));
    }

    private IEnumerator StompPushEnemies(Rigidbody rb, float ps, float sr, int type)
    {
        yield return new WaitForSeconds(0.2f);

        switch (type)
        {
            case TYPE_PLAYER:
                Enemy[] enemies = FindObjectsOfType<Enemy>();
                for (int i = 0; i < enemies.Length; i++)
                {
                    StompPushEnemies(enemies[i].gameObject.GetComponent<Rigidbody>(),
                        rb,
                        sr,
                        ps);
                }
                FindObjectOfType<AudioManager>().Play("PlayerStomp");
                break;
            case TYPE_ENEMY:
                PlayerController p = FindObjectOfType<PlayerController>();
                StompPushEnemies(p.gameObject.GetComponent<Rigidbody>(),
                    rb,
                    sr,
                    ps);
                FindObjectOfType<AudioManager>().Play("StomperStomp");
                break;
        }

        isStomping = false;
    }


    private void StompPushEnemies(Rigidbody enemy, Rigidbody self, float sr, float ps)
    {
        Vector3 awayFromSelf = (enemy.transform.position - self.transform.position);
        // calculate the distance from the enemy to the player
        float distance = Mathf.Abs((awayFromSelf).magnitude);
        //Debug.Log(distance);
        if (distance <= sr)
        {
            // apply force to enemy (enemies further away will be affected less than those closer)
            float stompMagPower = Mathf.Abs(distance - sr) / sr;
            //Debug.Log(stompMagPower * powerupStrength);

            enemy.AddForce(awayFromSelf * ps
                * stompMagPower, ForceMode.Impulse);
        }
    }
}
