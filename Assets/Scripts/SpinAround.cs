using UnityEngine;

/*
 * Used for winning screen to make mini versions of bosses spin around
 */

public class SpinAround : MonoBehaviour
{
    float randomSpin;

    void Start()
    {
        randomSpin = Random.Range(0.4f, 1.5f);
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, randomSpin, randomSpin, Space.Self);
    }
}
