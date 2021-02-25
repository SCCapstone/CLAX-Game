using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start()
    {
        doorRenderer = doorway.transform.GetChild(0).GetComponentInChildren<Renderer>();
        if(globals.spawnPoint == doorway.GetComponent<SpawnPoint>().spawnNum)
        {
            active = false;
            doorRenderer.material = change;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object entered");
        //TODO add boss is dead logic
        if (other.gameObject.CompareTag("Player") && active)
        {
            Debug.Log("Player Entered");
            globals.spawnPoint = 0;
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
