using UnityEngine;
/**
 * Controls the behavior of disappearing and reappering platforms, these are hard tied to the win condition of the game.
 */
public class AppearingPlatform : MonoBehaviour
{
    public GameObject platform;

    private Renderer platformRenderer;
    private BoxCollider platformCollider;

    /*
     * on creation sets the appearing platfrom to hidden
     * 
     */
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
