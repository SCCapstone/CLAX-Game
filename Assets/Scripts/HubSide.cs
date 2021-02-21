using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubSide : MonoBehaviour
{
    public string sceneName;
    bool spawnEnabled;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    OnTriggerEnter(Collider other)
    {
        //TODO add boss is dead logic
        if(Collider.Tag == "Player")
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
