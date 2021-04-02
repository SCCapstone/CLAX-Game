using UnityEngine;
using TMPro;

public class VSyncOptionText : MonoBehaviour
{
    private TextMeshProUGUI textBoxGUI;

    void Start()
    {
        textBoxGUI = GetComponent<TextMeshProUGUI>();
    }

    // Not optimal and unecessarily expensive to perform this when variable doesn't change often
    void Update()
    {
        textBoxGUI.SetText("VSync: " + (globals.videoSettings.vsyncEnabled ? "ON" : "OFF"));
    }
}
