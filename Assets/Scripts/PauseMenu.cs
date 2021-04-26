using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
/*
 * Controls the pause menu objects
 */
public class PauseMenu : MonoBehaviour
{
    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    public Slider soundSlider;
    public Slider musicSlider;
    public Slider mouseSlider;
    public Slider fovSlider;
    public Button fovResetButton;

    [Header("Text Objects")]
    public TextMeshProUGUI timerText;

    public Button vsyncButton;
    public Button colorblindButton;
    public Button difficultyButton;
    public Button timerButton;

    [Header("Colorblind materials settings")]
    public Material blue;
    public Material purple;
    public Material yellow;
    public Material green;

    public Material blueSub;
    public Material purpleSub;
    public Material yellowSub;
    public Material greenSub;
    public Material redSub;

    private PlayerInputActions inputs;

    private GameObject boss;
    private GameObject player;

    void Awake()
    {
        inputs = new PlayerInputActions();

        inputs.World.Pause.performed += OnPause;
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    void Start()
    {
        if (Globals.colorBlindEnabled)
        {
            ToSubColors();
        }

        if (!Globals.IsPaused())
        {
            HideAllMenus();
        }
        else
        {
            EnterPauseMenu();
        }

        UpdateText();

        // Initial values

        soundSlider.SetValueWithoutNotify(Globals.audioSettings.gameVolume);
        musicSlider.SetValueWithoutNotify(Globals.audioSettings.musicVolume);
        fovSlider.SetValueWithoutNotify(Globals.videoSettings.fieldOfView);
        mouseSlider.SetValueWithoutNotify(Globals.mouseSettings.mouseSensitivity);

        // Event listeners

        soundSlider.onValueChanged.AddListener(ChangeGameVolume);
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);

        fovSlider.onValueChanged.AddListener(ChangeFOV);
        fovResetButton.onClick.AddListener(ResetFOV);

        mouseSlider.onValueChanged.AddListener(ChangeMouseSlider);

        vsyncButton.onClick.AddListener(ToggleVSync);
        colorblindButton.onClick.AddListener(ToggleColorBlind);
        difficultyButton.onClick.AddListener(ChangeDifficulty);
        timerButton.onClick.AddListener(ToggleTimer);
    }

    private void ChangeGameVolume(float value)
    {
        Globals.audioSettings.gameVolume = value;
    }

    private void ChangeMusicVolume(float value)
    {
        Globals.audioSettings.musicVolume = value;
    }

    private void ChangeMouseSlider(float value)
    {
        Globals.mouseSettings.mouseSensitivity = value;
    }

    private void ResetFOV()
    {
        Globals.videoSettings.fieldOfView = 60.0f;

        fovSlider.value = Globals.videoSettings.fieldOfView;
    }
    /*
     * Changes the FOV
     */
    private void ChangeFOV(float value)
    {
        Globals.videoSettings.fieldOfView = value;
    }
    /*
 * When the player quits
 */
    public void OnQuit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    /*
     * When the player pauses the game
     */
    public void OnPause(InputAction.CallbackContext context)
    {
        if (Globals.IsPaused())
        {
            HideAllMenus();

            Globals.Unpause();
        }
        else
        {
            EnterPauseMenu();

            Globals.Pause();
        }
    }
    /*
     * When the player unpauses the game
     */
    public void OnResume()
    {
        HideAllMenus();

        Globals.Unpause();
    }
    /*
     * Remove the pause overlay from the screen
     */
    public void HideAllMenus()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }
    /*
     * When the player pauses the game
     */
    public void EnterPauseMenu()
    {
        HideAllMenus();

        pauseMenu.SetActive(true);
    }
    /*
     * When the player unpauses the game
     */
    public void ExitPauseMenu()
    {
        HideAllMenus();
    }

    public void EnterOptionsMenu()
    {
        HideAllMenus();

        settingsMenu.SetActive(true);
    }

    public void ExitOptionsMenu()
    {
        HideAllMenus();

        pauseMenu.SetActive(true);
    }
    /*
     * Update colors for color blind mode
     */
    public void ToSubColors()
    {
        foreach (GameObject ob in FindObjectsOfType<GameObject>())
        {
            if (ob.GetComponent<Renderer>() != null)
            {
                if (ob.GetComponent<Renderer>().material.name == blue.name + " (Instance)")
                {
                    ob.GetComponent<Renderer>().material = blueSub;
                }
                else if (ob.GetComponent<Renderer>().material.name == yellow.name + " (Instance)")
                {
                    ob.GetComponent<Renderer>().material = yellowSub;
                }
                else if (ob.GetComponent<Renderer>().material.name == purple.name + " (Instance)")
                {
                    ob.GetComponent<Renderer>().material = purpleSub;
                }
                else if (ob.GetComponent<Renderer>().material.name == green.name + " (Instance)")
                {
                    ob.GetComponent<Renderer>().material = greenSub;
                }
            }

        }
    }
    /*
     * Change back out of color blind mode
     */
    public void ToOriginalColors()
    {
        foreach (GameObject ob in FindObjectsOfType<GameObject>())
        {
            if (ob.GetComponent<Renderer>() != null)
            {
                if (ob.GetComponent<Renderer>().material.name == blueSub.name + " (Instance)")
                {
                    ob.GetComponent<Renderer>().material = blue;
                }
                else if (ob.GetComponent<Renderer>().material.name == yellowSub.name + " (Instance)")
                {
                    ob.GetComponent<Renderer>().material = yellow;
                }
                else if (ob.GetComponent<Renderer>().material.name == purpleSub.name + " (Instance)")
                {
                    ob.GetComponent<Renderer>().material = purple;
                }
                else if (ob.GetComponent<Renderer>().material.name == greenSub.name + " (Instance)")
                {
                    ob.GetComponent<Renderer>().material = green;
                }
            }
        }
    }
    /*
     * Change button text to reflect changes
     */
    public void UpdateText()
    {
        SetButtonText(vsyncButton, string.Format("VSync: {0}", Globals.videoSettings.vsyncEnabled ? "ON" : "OFF"));
        SetButtonText(colorblindButton, string.Format("Colorblind: {0}", Globals.colorBlindEnabled ? "ON" : "OFF"));
        SetButtonText(difficultyButton, string.Format("Difficulty: {0}", Globals.difficulty.ToString()));
        SetButtonText(timerButton, string.Format("Timer: {0}", Globals.timerEnabled ? "ON" : "OFF"));
    }
    /*
     * Changes the text of button to text
     */
    private void SetButtonText(Button button, string text)
    {
        TextMeshProUGUI gui = button.GetComponentInChildren<TextMeshProUGUI>();

        if (gui != null)
        {
            gui.SetText(text);
        }
    }
    /*
     * Update boss health and player health based on difficulty
     */
    private void ChangeDifficulty()
    {
        Globals.IncreaseDifficulty();

        UpdateText();

        // TODO: Move difficulty configurations to boss/player code

        boss = GameObject.FindGameObjectWithTag("Boss");
        player = GameObject.Find("Character");

        /*
         * EASY
         *  - Boss max health is 60%
         *  - Player max health is 100%
         * NORMAL
         *  - Boss max health is 100%
         *  - Player max health is 100% 
         * HARD
         *  - Boss max health is 100%
         *  - Player max health is 50%
         */

        if (Globals.difficulty == Globals.GameDifficulty.EASY)
        {
            float newBossHealth = boss.GetComponent<AliveObject>().maxHealth * 3 / 5;
            float curPercent = boss.GetComponent<AliveObject>().health / boss.GetComponent<AliveObject>().maxHealth;
            boss.GetComponent<AliveObject>().maxHealth = newBossHealth;
            boss.GetComponent<AliveObject>().health = curPercent * newBossHealth;
        }
        else if (Globals.difficulty == Globals.GameDifficulty.NORMAL)
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
        else if (Globals.difficulty == Globals.GameDifficulty.HARD)
        {
            float newPlayerHealth = .5f * player.GetComponent<AliveObject>().maxHealth;

            float curPercent = player.GetComponent<AliveObject>().health / player.GetComponent<AliveObject>().maxHealth;
            player.GetComponent<AliveObject>().maxHealth = newPlayerHealth;
            player.GetComponent<AliveObject>().health = curPercent * newPlayerHealth;
        }
    }

    private void ToggleColorBlind()
    {
        Globals.colorBlindEnabled = !Globals.colorBlindEnabled;

        UpdateText();

        if (Globals.colorBlindEnabled)
        {
            ToSubColors();
        }
        else
        {
            ToOriginalColors();
        }
    }

    private void ToggleVSync()
    {
        Globals.videoSettings.vsyncEnabled = !Globals.videoSettings.vsyncEnabled;

        UpdateText();
    }

    private void ToggleTimer()
    {
        Globals.timerEnabled = !Globals.timerEnabled;

        UpdateText();

        timerText.gameObject.SetActive(Globals.timerEnabled);
    }
}
