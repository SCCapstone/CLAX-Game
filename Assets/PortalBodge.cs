using UnityEngine;

public class PortalBodge : MonoBehaviour
{
    public DoorScript cubeDoor;
    public DoorScript triangleDoor;

    void Start()
    {
        cubeDoor.SetForceDisable(Globals.cube);
        triangleDoor.SetForceDisable(Globals.pyramid);
    }
}
