using System.Collections;
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
    public GameObject optionsMenu;
    public GameObject timerObject;
    public bool isGamePaused;

    private GameObject boss;
    private GameObject player;

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

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("game was quit");

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

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
        Debug.Log("clicked open options");


    }

    public void changeDifficulty()
    {
        boss = GameObject.Find("Boss2");
        player = GameObject.Find("Character");

        globals.difficulty = ((globals.difficulty + 1) % 4);
        if (globals.difficulty == 0)
        {
            globals.difficulty = 1;
        }
        Debug.Log("difficulty " + globals.difficulty);

        if (globals.difficulty == 1)
        {
            float newBossHealth = boss.GetComponent<AliveObject>().maxHealth * 3 / 5;
            float curPercent = boss.GetComponent<AliveObject>().health / boss.GetComponent<AliveObject>().maxHealth;
            boss.GetComponent<AliveObject>().maxHealth = newBossHealth;
            boss.GetComponent<AliveObject>().health = curPercent * newBossHealth;

        }

        if (globals.difficulty == 2)
        {
            var newBossHealth = 1000;
            float curPercent = boss.GetComponent<AliveObject>().health / boss.GetComponent<AliveObject>().maxHealth;
            boss.GetComponent<AliveObject>().maxHealth = newBossHealth;
            boss.GetComponent<AliveObject>().health = curPercent * newBossHealth;

            var newPlayerHealth = 100;
            float curPercent2 = player.GetComponent<AliveObject>().health / player.GetComponent<AliveObject>().maxHealth;
            player.GetComponent<AliveObject>().maxHealth = newPlayerHealth;
            player.GetComponent<AliveObject>().health = curPercent2 * newPlayerHealth;
        }

        if (globals.difficulty == 3)
        {
            float newPlayerHealth = .5f * player.GetComponent<AliveObject>().maxHealth;

            float curPercent = player.GetComponent<AliveObject>().health / player.GetComponent<AliveObject>().maxHealth;
            player.GetComponent<AliveObject>().maxHealth = newPlayerHealth;
            player.GetComponent<AliveObject>().health = curPercent * newPlayerHealth;

        }

        Debug.Log("player health" + player.GetComponent<AliveObject>().health + " " + player.GetComponent<AliveObject>().maxHealth);
        Debug.Log("boss health" + boss.GetComponent<AliveObject>().health + " " + boss.GetComponent<AliveObject>().maxHealth);



    }

    public void CloseOptions()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);

    }

    public void ToggleTimer()
    {
        timerObject.GetComponent<TimerScript>().timerVisible = !timerObject.GetComponent<TimerScript>().timerVisible;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
