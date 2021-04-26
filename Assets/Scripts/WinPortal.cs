using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Check when the player enters the win screen portal
 */
public class WinPortal : MonoBehaviour
{
    public GameObject doorway;
    public Material change;

    private bool active = false;

    /**
     * updates the portal based on the player beating all bosses through global flags
     */
    void FixedUpdate()
    {
        if (Globals.HasWon())
        {
            Renderer doorRenderer = doorway.GetComponentInChildren<Renderer>();

            if (doorRenderer != null)
            {
                doorRenderer.material = change;
            }

            active = true;
        }
    }

    /**
     * moves the player to new scene
     */
    private void OnTriggerEnter(Collider other)
    {
        if (active && other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadSceneAsync("WinScreen");
        }
    }
}
