using System.Collections.Generic;
using UnityEngine;

public class SpiralCreator : MonoBehaviour
{
    public GameObject floorPrefab;

    List<GameObject> spiral = new List<GameObject>();

    void Start()
    {
        float radius = 20.0f;
        float heightIncrement = 5.0f;
        float revolutions = 2.0f;
        int totalIterations = 12;

        for (int i = 0; i <= totalIterations; ++i)
        {
            float t = (i * revolutions / totalIterations) * Mathf.PI;
            Vector3 offset = new Vector3(
                Mathf.Cos(t) * radius,
                t * heightIncrement,
                Mathf.Sin(t) * radius
            );

            offset = transform.TransformDirection(offset);

            GameObject floor = Instantiate(floorPrefab, transform);

            floor.transform.localScale = new Vector3(10.0f, 1.0f, 10.0f);
            floor.transform.position += offset;
            floor.transform.LookAt(transform.position + Vector3.up * t * heightIncrement);

            spiral.Add(floor);
        }
    }
}
