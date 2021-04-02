using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // learned from https://www.youtube.com/watch?v=pbeB9NsaoPs
    //[SerializeField] private GameObject pauseMenu;
    //[SerializeField] private bool isGamePaused;

    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject timerObject;
    public Slider fovSlider;
    public Button fovReset;

    public bool isGamePaused;

    private GameObject boss;
    private GameObject player;

    private PlayerInputActions inputs;

    public Material blue;
    public Material purple;
    public Material yellow;
    public Material green;

    public Material blueSub;
    public Material purpleSub;
    public Material yellowSub;
    public Material greenSub;
    public Material redSub;

    void Awake()
    {
        inputs = new PlayerInputActions();

        inputs.World.Pause.performed += OnPause;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (globals.colorBlindEnabled)
        {
            ChangeColor();
        }

        fovSlider.onValueChanged.AddListener(delegate { ChangeFOV(); });
        fovReset.onClick.AddListener(delegate { ResetFOV(); });
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void ResetFOV()
    {
        globals.videoSettings.fieldOfView = 60.0f;

        fovSlider.value = globals.videoSettings.fieldOfView;
    }

    private void ChangeFOV()
    {
        globals.videoSettings.fieldOfView = fovSlider.value;
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
        if (pauseMenu != null)
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
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);

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
        boss = GameObject.FindGameObjectWithTag("Boss");
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

    public void ChangeColor()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        foreach (GameObject ob in allObjects)
        {
            if (ob.GetComponent<Renderer>() != null)
            {
                Debug.Log("Object Found " + ob.GetComponent<Renderer>().material.name + " " + yellow.name);
                if (ob.GetComponent<Renderer>().material.name == blue.name + " (Instance)")
                {
                    ob.GetComponent<Renderer>().material = blueSub;
                }

                if (ob.GetComponent<Renderer>().material.name == yellow.name + " (Instance)")
                {
                    ob.GetComponent<Renderer>().material = yellowSub;
                }

                if (ob.GetComponent<Renderer>().material.name == purple.name + " (Instance)")
                {
                    ob.GetComponent<Renderer>().material = purpleSub;
                }

                if (ob.GetComponent<Renderer>().material.name == green.name + " (Instance)")
                {
                    ob.GetComponent<Renderer>().material = greenSub;
                }
            }

        }
    }

    public void ChangeBackToDefaultColor()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject ob in allObjects)
        {
            if (ob.GetComponent<Renderer>() != null)
            {
                if (ob.GetComponent<Renderer>().material.name == blueSub.name + " (Instance)")
                {
                    ob.GetComponent<Renderer>().material = blue;
                }
                if (ob.GetComponent<Renderer>().material.name == yellowSub.name + " (Instance)")
                {
                    ob.GetComponent<Renderer>().material = yellow;
                }
                if (ob.GetComponent<Renderer>().material.name == purpleSub.name + " (Instance)")
                {
                    ob.GetComponent<Renderer>().material = purple;
                }
                if (ob.GetComponent<Renderer>().material.name == greenSub.name + " (Instance)")
                {
                    ob.GetComponent<Renderer>().material = green;
                }
            }
        }
    }

    public void ToggleColorBlind()
    {
        if (globals.colorBlindEnabled == false)
        {
            ChangeColor();
            globals.colorBlindEnabled = true;
        }
        else
        {
            ChangeBackToDefaultColor();
            globals.colorBlindEnabled = false;
        }
    }

    public void ToggleVSync()
    {
        globals.videoSettings.vsyncEnabled = !globals.videoSettings.vsyncEnabled;

        QualitySettings.vSyncCount = globals.videoSettings.vsyncEnabled ? 1 : 0;
    }

    public void CloseOptions()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void ToggleTimer()
    {
        globals.timerEnabled = !globals.timerEnabled;
    }
}
