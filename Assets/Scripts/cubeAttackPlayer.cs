using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeAttackPlayer : MonoBehaviour
{
    //private GameObject target;
    public Vector3 goalCoords;
    //float xzSpeed = 4;
    //float ySpeed = 2;
    public bool shouldMove = false;
    float timeMade;


    //public int idNum = -1;

    // Start is called before the first frame update
    void Start()
    {
        timeMade = Time.timeSinceLevelLoad;
        goalCoords = new Vector3(0, 0);
    }

    void getGoal(int xOffset = 0, int yOffset = 0, int zOffset = 0)
    {
        var target = GameObject.FindGameObjectWithTag("Player");
        goalCoords = target.transform.position;

        goalCoords += new Vector3(Random.Range(-xOffset, xOffset),
            Random.Range(-yOffset, yOffset),
            Random.Range(-zOffset, zOffset));
        //Debug.Log("goal coords " + goalCoords);

    }

    void moveToGoal()
    {
        //check if a goal has not been set yet
        if (goalCoords == new Vector3(0, 0))
        {
            getGoal(2, 0, 2);
        }
        transform.position = Vector3.MoveTowards(transform.position, goalCoords, 1);

    }

    private void FixedUpdate()
    {
        
        if (shouldMove)
        {
            moveToGoal();
        }
    }

    //void checkIfAwake()
    //{
    //    float timeExisted = Time.timeSinceLevelLoad - timeMade;
    //    if (timeExisted > )
    //}

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("goalCoords" + goalCoords);

    }
}
