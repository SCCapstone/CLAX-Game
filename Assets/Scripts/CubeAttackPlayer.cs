using UnityEngine;

/**
 * Allows for the boss to target the player with thrown objects
 */
public class CubeAttackPlayer : MonoBehaviour
{
    public Vector3 goalCoords = Vector3.zero;
    public bool shouldMove = false;

    /**
     * aims the cube attack at the player with slight offset
     * 
     * takes in a (X,Y,Z) coordinate for how to offset the aim
     */
    void SetGoalRelative(int xOffset = 0, int yOffset = 0, int zOffset = 0)
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");

        goalCoords = target.transform.position;
        goalCoords += new Vector3(Random.Range(-xOffset, xOffset),
            Random.Range(-yOffset, yOffset),
            Random.Range(-zOffset, zOffset));
    }


    private void FixedUpdate()
    {
        if (shouldMove)
        {
            // Check if a goal has not been set yet
            if (goalCoords == Vector3.zero)
            {
                SetGoalRelative(2, 0, 2);
            }

            transform.position = Vector3.MoveTowards(transform.position, goalCoords, 1);
        }
    }
}
