using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    private bool active = false;
    public GameObject doorway;
    public Material change;

    private Renderer doorRenderer;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("pill = " + Globals.pill + " cube = " + Globals.cube + " pyramid =" + Globals.pyramid + " won = " + Globals.won);
        doorRenderer = doorway.GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.won && !active)
        {
            doorRenderer.material = change;
            active = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("pill = " + Globals.pill + " cube = " + Globals.cube + " pyramid =" + Globals.pyramid + " won = " + Globals.won);
        if (other.gameObject.CompareTag("Player") && active)
        {
            SceneManager.LoadSceneAsync("WinScreen");
        }
    }
}
