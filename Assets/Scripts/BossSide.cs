using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSide : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject doorway;
    public GameObject player;
    public string sceneName;

    public Material change;

    private Renderer doorRenderer;
    
    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        doorRenderer = doorway.transform.GetChild(0).GetComponentInChildren<Renderer>();
        player = (GameObject)Instantiate(player, Vector3.zero, Quaternion.identity);
        player.transform.position = doorway.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (globals.pill && !active)
        {
            doorRenderer.material = change;
            active = true;
        } 
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object entered");
        //TODO add boss is dead logic
        if (other.gameObject.CompareTag("Player") && active)
        {
            Debug.Log("Player Entered");
            SceneManager.LoadSceneAsync(sceneName);
        }
    }

}