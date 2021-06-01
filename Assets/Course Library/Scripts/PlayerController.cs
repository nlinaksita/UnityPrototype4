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
    private const int numRockets = 6;

    private const string PU_BOUNCE = "Powerup_Bounce(Clone)";
    private const string PU_ROCKETS = "Powerup_Rockets(Clone)";
    private const string PU_STOMP = "Powerup_Stomp(Clone)";

    private int playerLives;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    private int powerupIndex;
    private float powerupStrength = 15.0f;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

        // Start off with 3 lives
        playerLives = 3;
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
                }

                // Remove any previous force on the object to prevent it from moving when it respawns
                playerRb.velocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
                // Reset the player position
                gameObject.transform.position = Vector3.zero;
            }
        }

        // if the player has the rocket powerup, pressing space fires
        if (hasPowerup && powerupIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireRockets();
            }
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

    public void GameOver()
    {
        Time.timeScale = 0;
        //Debug.Log("Game Over");
    }
}
