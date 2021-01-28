using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeAttackPlayer : MonoBehaviour
{
    private GameObject target;
    Vector3 goalCoords;
    float xzSpeed = 4;
    float ySpeed = 2;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        startPos = transform.position;
        getGoal();
    }

    void getGoal()
    {
        var target = GameObject.FindGameObjectWithTag("Player");
        goalCoords = target.transform.position;

    }

    void moveToGoal()
    {
        //transform.position += new Vector3()

    }

    private void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("goalCoords" + goalCoords);

    }
}
