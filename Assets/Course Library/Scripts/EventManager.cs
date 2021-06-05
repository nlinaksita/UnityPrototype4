using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public GameObject instructions;
    public GameObject titleWindow;
    public GameObject characterWindow;
    public GameObject powerupWindow;
    public AudioSource click;
    private void Start()
    {
        Time.timeScale = 0;
        instructions.gameObject.SetActive(true);
    }

    public void OnRestartButtonClick()
    {
        //Time.timeScale = 1;
        //Debug.Log(Time.timeScale);
        SceneManager.LoadScene("Prototype 4");
        FindObjectOfType<AudioManager>().Play("UIClick");
    }

    public void OnPlayButtonClick()
    {
        Time.timeScale = 1;
        instructions.gameObject.SetActive(false);
        FindObjectOfType<AudioManager>().Play("UIClick");
    }

    public void OnInstructionsClick()
    {
        titleWindow.gameObject.SetActive(false);
        characterWindow.gameObject.SetActive(true);
        FindObjectOfType<AudioManager>().Play("UIClick");
    }

    public void OnNextClick()
    {
        characterWindow.gameObject.SetActive(false);
        powerupWindow.gameObject.SetActive(true);
        FindObjectOfType<AudioManager>().Play("UIClick");
    }
}
