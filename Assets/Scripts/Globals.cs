using UnityEngine;

public class Globals : MonoBehaviour
{
    // Pause

    private static bool isPaused = false;

    public static bool IsPaused()
    {
        return isPaused;
    }

    public static void Pause()
    {
        AudioListener.pause = true;
        Time.timeScale = 0.0f;

        isPaused = true;
    }

    public static void Unpause()
    {
        AudioListener.pause = false;
        Time.timeScale = 1.0f;

        isPaused = false;
    }

    // Enemy layer

    public static int enemyLayerNum = 9;

    // Spawn flags

    public static string desiredSpawnName;

    // Boss flags

    public static bool boss = true;
    public static bool pill = false;
    public static bool cube = false;
    public static bool pyramid = false;
    
    public static bool HasWon()
    {
        return pill && cube && pyramid;
    }

    // Settings

    public enum GameDifficulty
    {
        EASY,
        NORMAL,
        HARD
    }

    public static GameDifficulty difficulty = GameDifficulty.NORMAL;

    public static void IncreaseDifficulty()
    {
        switch (difficulty)
        {
            case GameDifficulty.EASY:
                difficulty = GameDifficulty.NORMAL;

                break;
            case GameDifficulty.NORMAL:
                difficulty = GameDifficulty.HARD;

                break;
            case GameDifficulty.HARD:
                difficulty = GameDifficulty.EASY;

                break;
            default:
                // This should never happen
                difficulty = GameDifficulty.NORMAL;

                break;
        }
    }

    public static bool timerEnabled = true;

    public static bool colorBlindEnabled = false;

    public struct VideoSettings
    {
        public bool vsyncEnabled;
        public float fieldOfView;
    }

    public struct AudioSettings
    {
        public float musicVolume;
    }
    //
    public struct MouseSettings
    {
        public float mouseSensitivity;
    }

    public static VideoSettings videoSettings = new VideoSettings()
    {
        vsyncEnabled = true,
        fieldOfView = 60.0f
    };

    public static AudioSettings audioSettings = new AudioSettings()
    {
        musicVolume = 0.2f
    };
    //
    public static MouseSettings mouseSettings = new MouseSettings()
    {
        mouseSensitivity = 1f
    };
}
