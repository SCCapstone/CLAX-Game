using UnityEngine;

public class DoorScript : AliveObject
{
    void Awake()
    {
        onDeath += OnDeath;
    }

    void OnDeath()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        Destroy(gameObject, 1.0f);
    }
}
