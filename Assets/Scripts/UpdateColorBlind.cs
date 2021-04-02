using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateColorBlind : MonoBehaviour
{
    private TextMeshProUGUI textBoxGUI;
    //public GameObject difficultyObject;

    // Start is called before the first frame update
    void Start()
    {
        textBoxGUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

        textBoxGUI.SetText("ColorBlind: " + (Globals.colorBlindEnabled));
    }
}
