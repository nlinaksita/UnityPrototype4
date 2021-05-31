using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public bool hasPowerup;
    public GameObject powerupIndicator;
    public Text playerLivesText;

    private int playerLives;
    private Rigidbody playerRb;
    private GameObject focalPoint;
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
        powerupIndicator.transform.position = transform.position + new Vector3(0, 1f, 0);

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
                hasPowerup = false;
                powerupIndicator.gameObject.SetActive(false);

                // Remove any previous force on the object to prevent it from moving when it respawns
                playerRb.velocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
                // Reset the player position
                gameObject.transform.position = Vector3.zero;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collided with " + collision.gameObject.name 
                + " with powerup set to " + hasPowerup);

            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        //Debug.Log("Game Over");
    }
}
