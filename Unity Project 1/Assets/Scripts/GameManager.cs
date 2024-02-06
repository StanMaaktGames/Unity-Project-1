using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
    }

    void Respawn()
    {
        Console.WriteLine("Respawn");
        SceneManager.LoadScene("SampleScene");
    }
}
