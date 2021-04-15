using UnityEngine;
using UnityEngine.SceneManagement;

public class HubSide : MonoBehaviour
{
    public GameObject doorway;
    public string sceneName;
    public int otherSideSpawnPoint;

    public Material change;

    private Renderer doorRenderer;

    private bool active = true;
    
    void Start()
    {
        doorRenderer = doorway.transform.GetChild(0).GetComponentInChildren<Renderer>();

        if(Globals.spawnPoint == doorway.GetComponent<SpawnPoint>().spawnNum)
        {
            active = false;
            doorRenderer.material = change;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object entered");

        //TODO add boss is dead logic
        if (other.gameObject.CompareTag("Player") && active)
        {
            Debug.Log("Player Entered");

            Globals.spawnPoint = 0;
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
