using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAround : MonoBehaviour
{
    float randomSpin;
    // Start is called before the first frame update
    void Start()
    {
        randomSpin = Random.Range(-1.5f, 1.5f);
        if (Mathf.Abs(randomSpin) < .4f)
        {
            randomSpin = .8f;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        this.transform.Rotate(0, randomSpin, randomSpin, Space.Self);

    }
}
