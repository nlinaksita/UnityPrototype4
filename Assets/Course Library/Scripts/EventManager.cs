using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public void OnRestartButtonClick()
    {
        //Time.timeScale = 1;
        //Debug.Log(Time.timeScale);
        SceneManager.LoadScene("Prototype 4");
    }
}
