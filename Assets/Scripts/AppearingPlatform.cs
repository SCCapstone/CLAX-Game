using UnityEngine;

public class AppearingPlatform : MonoBehaviour
{
    public GameObject platform;

    private Renderer platformRenderer;
    private BoxCollider platformCollider;

    void Start()
    {
        platformRenderer = platform.GetComponentInChildren<Renderer>();
        platformCollider = platform.GetComponentInChildren<BoxCollider>();

        if (!Globals.HasWon())
        {
            platformRenderer.enabled = false;
            platformCollider.enabled = false;
        }
    }
}
