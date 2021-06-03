using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public bool hasPowerup;
    public GameObject[] powerupIndicator;
    public GameObject rocket;
    public Text playerLivesText;
    public GameObject forceField;
    private const int numRockets = 6;

    private const string PU_BOUNCE = "Powerup_Bounce(Clone)";
    private const string PU_ROCKETS = "Powerup_Rockets(Clone)";
    private const string PU_STOMP = "Powerup_Stomp(Clone)";
    private const string PU_REPEL = "Powerup_Repel(Clone)";

    private int playerLives;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    private int powerupIndex;
    private float powerupStrength = 10.0f;
    private float stompPower = 50f;
    private float stompRange = 5f;
    private bool isStomping = false;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

        // Start off with 3 lives
        playerLives = 3;

        isStomping = false;
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        playerRb.AddForce(focalPoint.transform.right * speed * horizontalInput);
        powerupIndicator[powerupIndex].transform.position = transform.position + new Vector3(0, 1f, 0);

        // If the player falls off the stage
        if (gameObject.transform.position.y < -10)
        {
            // Reduce the number of lives left
            playerLives--;

            // If there are no lives left and the player falls off the stage, gameover
            if (playerLives < 0)
            {
                GameOver();
            } else {
                playerLivesText.text = "x " + playerLives;

                // If the player has a powerup and falls, remove the powerup indicator
                if (hasPowerup)
                {
                    hasPowerup = false;
                    powerupIndicator[powerupIndex].gameObject.SetActive(false);
                    // stop the powerup countdown coroutine
                    StopCoroutine(PowerupCountdownRoutine());

                    // Disable force field
                    forceField.gameObject.SetActive(false);
                    StopCoroutine(DisableForceField());
                }

                // Remove any previous force on the object to prevent it from moving when it respawns
                playerRb.velocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
                // Reset the player position
                gameObject.transform.position = Vector3.zero;
            }
        }

        // if the player has the rocket powerup, pressing space fires
        if (Input.GetKeyDown(KeyCode.Space) && hasPowerup)
        {
            switch (powerupIndex)
            {
                // rocket powerup
                case 1:
                    FireRockets();
                    break;
                // stomp powerup
                case 2:
                    if (!isStomping) 
                    {
                        StompUp();
                    }
                    break;
            }
        }
    }

    // Create property for powerup strength
    public float PowerupStrength
    {
        get
        {
            return powerupStrength;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            //Debug.Log(other.gameObject.name);
            // Choose which powerup indicator to display
            switch (other.gameObject.name)
            {
                case PU_BOUNCE:
                    powerupIndex = 0;
                    break;
                case PU_ROCKETS:
                    powerupIndex = 1;
                    FireRockets();
                    break;
                case PU_STOMP:
                    powerupIndex = 2;
                    break;
                case PU_REPEL:
                    powerupIndex = 3;
                    EnableForceField();
                    break;
            }
            powerupIndicator[powerupIndex].gameObject.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator[powerupIndex].gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Bounce powerup pushes enemies (powerupIndex == 0)
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup && powerupIndex == 0)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collided with " + collision.gameObject.name 
                + " with powerup set to " + hasPowerup);

            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }

    private void FireRockets()
    {
        /*
        // For every enemy on the field, spawn a rocket at player position
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            // Find the direction the rocket should face
            Vector3 direction = (enemies[i].transform.position - transform.position);
            float angle = Vector3.Angle(direction, transform.forward);
            Debug.Log(angle);
            Vector3 rotationVector = new Vector3(0, angle, 0);
            Quaternion rotation = Quaternion.Euler(rotationVector);
            Instantiate(rocket, gameObject.transform.position, rotation);
            rocket.GetComponent<Rigidbody>().AddForce(
                (enemies[i].transform.position - transform.position).normalized * 100);
        }
        */
        float relativeAngle = Random.Range(0f, 90f);
        for (int i = 0; i < numRockets; i++)
        {
            // Evenly distribute rockets around the player relative to a random rotation
            float rotation = (360 / (float)numRockets)*i + relativeAngle;
            Instantiate(rocket, gameObject.transform.position, Quaternion.Euler(0, rotation, 0));
        }

    }

    private void StompUp()
    {
        // Set flag so that cannot interrupt stomp with another stomp
        isStomping = true;

        // Stop velocity
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        // Reset rotation
        transform.rotation.eulerAngles.Set(0, 0, 0);

        // Apply upward force
        playerRb.AddForce(Vector3.up * powerupStrength, ForceMode.Impulse);

        StartCoroutine(StompDown());
    }

    // Player stomps down after short delay
    private IEnumerator StompDown()
    {
        yield return new WaitForSeconds(0.3f);
        // Apply downward force
        playerRb.AddForce(Vector3.down * stompPower, ForceMode.Impulse);

        StartCoroutine(StompPushEnemies());
    }

    // Stomp applies force to enemies if they are within range
    private IEnumerator StompPushEnemies()
    {
        yield return new WaitForSeconds(0.2f);

        // Iterate over enemies and check if they are within stompRange
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 awayFromPlayer = (enemies[i].transform.position - transform.position);
            // calculate the distance from the enemy to the player
            float distance = Mathf.Abs((awayFromPlayer).magnitude);
            //Debug.Log(distance);
            if (distance <= stompRange)
            {
                // apply force to enemy (enemies further away will be affected less than those closer)
                float stompMagPower = Mathf.Abs(distance - stompRange)/stompRange;
                //Debug.Log(stompMagPower * powerupStrength);
                
                enemies[i].GetComponent<Rigidbody>().AddForce(awayFromPlayer * powerupStrength
                    * stompMagPower, ForceMode.Impulse);
            }
        }

        // reset stomping flag
        isStomping = false;
    }

    private void EnableForceField()
    {
        // Enable force field
        forceField.gameObject.SetActive(true);
        StartCoroutine(DisableForceField());
    }

    IEnumerator DisableForceField()
    {
        yield return new WaitForSeconds(7);
        forceField.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        //Debug.Log("Game Over");
    }
}
