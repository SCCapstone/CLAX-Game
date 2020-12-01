﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PauseMenu : MonoBehaviour
{
    // learned from https://www.youtube.com/watch?v=pbeB9NsaoPs
    //[SerializeField] private GameObject pauseMenu;
    //[SerializeField] private bool isGamePaused;

    public GameObject pauseMenu;
    public bool isGamePaused;

    private PlayerInputActions inputs;

    // Start is called before the first frame update
    void Awake()
    {
        inputs = new PlayerInputActions();

        inputs.World.Pause.performed += OnPause;
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    public void quitGame()
    {
        Application.Quit();
    }


    public void OnPause(InputAction.CallbackContext context)
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused == true)
        {
            Pause();
        }
        else
        {
            Unpause();
        }

    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        AudioListener.pause = true;
        Time.timeScale = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("ran pause");
        isGamePaused = true;
    }

    public void Unpause()
    {
        pauseMenu.SetActive(false);
        AudioListener.pause = false;
        Time.timeScale = 1;
        Debug.Log("ran unpause");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isGamePaused = false;
        //Cursor.visible = false;
        //isGamePaused = false;
    }

    public void options()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
