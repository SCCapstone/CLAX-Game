using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPortal : MonoBehaviour
{
    public GameObject doorway;
    public Material change;

    private bool active = false;

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

    private void OnTriggerEnter(Collider other)
    {
        if (active && other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadSceneAsync("WinScreen");
        }
    }
}
