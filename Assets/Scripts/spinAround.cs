using UnityEngine;

public class SpinAround : MonoBehaviour
{
    float randomSpin;
    
    void Start()
    {
        randomSpin = Random.Range(0.4f, 1.5f);
    }

    private void FixedUpdate()
    {
        this.transform.Rotate(0, randomSpin, randomSpin, Space.Self);
    }
}
