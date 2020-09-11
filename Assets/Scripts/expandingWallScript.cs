using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expandingWallScript : MonoBehaviour
{

    float lastWallExpandTime;
    float maxWallHeight = 50;
    float wallExpandFrequency = .03f;
    float maxHorizontalLength = 60;
    float growAmount = 0.7f;

    bool isGrowing = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    void wallExpand()
    {
        if (isGrowing && transform.localScale.x >= maxHorizontalLength)
        {
            isGrowing = false;
            //return;
        }
        if (!isGrowing && transform.localScale.x < 3)
            return;
        int direction = 1;
        if (!isGrowing)
            direction = -1;


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

    // Update is called once per frame
    void Update()
    {
        wallExpand();
    }
}
