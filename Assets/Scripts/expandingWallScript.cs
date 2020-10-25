using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expandingWallScript : MonoBehaviour
{

    float lastWallExpandTime;
    float maxWallHeight = 50;
    float wallExpandFrequency = .015f;
    float maxHorizontalLength = 60;
    float growAmount = 0.7f;

    public bool isDone = false;
    bool isGrowing = true;

    public bool lockWallSize = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("lock wall is " + lockWallSize);
        lockWallSize = false;

    }

    void wallExpand()
    {
        if (isGrowing && transform.localScale.x >= maxHorizontalLength)
        {
            isGrowing = false;
            //return;
        }
        if (!isGrowing && transform.localScale.x < 3)
        {
            isDone = true;
            return;
        }

        int direction = 1;
        if (!isGrowing)
            direction = -1;

        if (isGrowing == false && lockWallSize)
        {
            direction = 0;
        }
        //Debug.Log("direction is " + direction);


        //only expand every so often
        if (Time.time - lastWallExpandTime >= wallExpandFrequency)
        {
            var curPos = transform.position;
            var curScale = transform.localScale;

            transform.localScale = new Vector3(curScale.x + growAmount * direction,
                Mathf.Min(Mathf.Max(curScale.y + .5f * direction, .2f), maxWallHeight), .2f);
            transform.position = new Vector3(curPos.x, 0, curPos.z);

            lastWallExpandTime = Time.time;
        }
    }

    //Update is called once per frame
    void Update()
    {
        wallExpand();
    }
}
