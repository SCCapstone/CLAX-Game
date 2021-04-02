using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    private TextMeshProUGUI textBoxGUI;

    // Start is called before the first frame update
    void Start()
    {
        textBoxGUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (globals.timerEnabled)
        {
            textBoxGUI.SetText(Time.time.ToString("0.00"));
        }
        else
        {
            textBoxGUI.SetText("");
        }
    }
}
