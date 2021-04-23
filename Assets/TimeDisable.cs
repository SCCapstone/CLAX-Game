using UnityEngine;

public class TimeDisable : MonoBehaviour
{
    void Awake()
    {
        Globals.timerCounting = false;
    }
}
