using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSide : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject doorway;
    public GameObject player;
    public GameObject Boss;
    public string sceneName;

    public Material change;

    private Renderer renderer;
    
    private bool active;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        renderer = doorway.GetComponentInChildren<Renderer>();
        player = (GameObject)Instantiate(player, Vector3.zero, Quaternion.identity);
        player.transform.position = doorway.transform.GetChild(0).position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Boss.GetComponent<AliveObject>().health <= 0.0f)
        {
            active = true;
            renderer.material = change;
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