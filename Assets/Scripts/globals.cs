using UnityEngine;

public class Globals : MonoBehaviour
{
    public struct VideoSettings
    {
        public bool vsyncEnabled;
        public float fieldOfView;
    };

    //Boss flags
    public static bool boss = true;

    public static bool pill = false;
    public static bool cube = false;
    public static bool pyramid = false;
    public static bool won = false;

    public static int spawnPoint = 0;

    public static int difficulty = 2;
    public static bool timerEnabled = true;

    public static bool colorBlindEnabled = false;

    public static VideoSettings videoSettings = new VideoSettings() {
        vsyncEnabled = true,
        fieldOfView = 60.0f
    };
}
