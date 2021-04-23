using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    private TextMeshProUGUI gui;

    void Start()
    {
        gui = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (Globals.timerCounting)
        {
            Globals.timerCounter += Time.deltaTime;
        }

        gui.SetText(Globals.timerCounter.ToString("0.00"));
    }
}
