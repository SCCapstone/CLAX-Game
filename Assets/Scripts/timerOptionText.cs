using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class timerOptionText : MonoBehaviour
{
    private TextMeshProUGUI textBoxGUI;
    public GameObject timerObject;

    // Start is called before the first frame update
    void Start()
    {
        textBoxGUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textBoxGUI.SetText("Timer: " + (timerObject.GetComponent<TimerScript>().timerVisible ? "ON" : "OFF"));
    }
}
