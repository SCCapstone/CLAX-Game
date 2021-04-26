using UnityEngine;
/*
 * Ensures that the win screen door is unlocked
 */
public class PortalBodge : MonoBehaviour
{
    public Portal cubeDoor;
    public Portal triangleDoor;

    void Start()
    {
        cubeDoor.SetForceDisable(Globals.cube);
        triangleDoor.SetForceDisable(Globals.pyramid);
    }
}
