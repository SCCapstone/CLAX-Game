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

        foreach (BoxCollider collider in GetComponents<BoxCollider>())
        {
            collider.enabled = false;
        }

        Destroy(gameObject, 1.0f);
    }
}
