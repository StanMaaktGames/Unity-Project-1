using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool isPaused = false;

    public GameObject pauseMenu;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(Input.GetKey("r"))
        {
            Debug.LogWarning("Reset");
            SceneManager.LoadScene("SampleScene");
        }

        if (Input.GetKeyDown("p"))
        {
            TogglePause();
        }
    }

    void Respawn()
    {
        Console.WriteLine("Respawn");
        SceneManager.LoadScene("SampleScene");
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0.00000000001f : 1;
        pauseMenu.SetActive(isPaused);
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
