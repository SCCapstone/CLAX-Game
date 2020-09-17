using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatforms : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject startStage;
    public GameObject endStage;
    public GameObject stage;
    public float speed;
    Transform start;
    Transform end;
    Transform current;
    Transform goal;


    void Start()
    {
        start = startStage.transform;
        end = endStage.transform;
        goal = end;
    }

    // Update is called once per frame
    void Update()
    {
        current = stage.transform;
        if (Vector3.Distance(current.position, start.position) < 001f)
            goal = end;
        if (Vector3.Distance(current.position, end.position) < 001f)
            goal = start;
        stage.transform.position = Vector3.Lerp(current.position, goal.position, speed * Time.deltaTime);
    }
    void OnCollisionStay(Collision collision)
    {
        transform.parent = collision.transform;
    }

}
