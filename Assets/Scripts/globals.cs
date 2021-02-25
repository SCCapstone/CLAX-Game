using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globals : MonoBehaviour
{
    //Boss flags
    public static bool boss = true;

    public static bool pill = false;
    public static bool cube = false;
    public static bool pyramid = false;
    public static bool won = false;

    public static int spawnPoint = 0;

    public static int difficulty = 2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(pill && cube && pyramid && !won)
        {
            won = true;
        }
    }
}
