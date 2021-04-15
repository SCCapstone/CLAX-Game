using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSide : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject doorway;
    public GameObject player;
    public string sceneName;
    public int otherSideSpawnPoint;

    public Material change;

    private Renderer doorRenderer;

    private bool active = false;

    public AudioSource bossDeathSound;

    void Start()
    {
        doorRenderer = doorway.transform.GetChild(0).GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (!Globals.boss && !active)
        {
            doorRenderer.material = change;
            active = true;
            bossDeathSound.Play();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        //TODO add boss is dead logic
        if (other.gameObject.CompareTag("Player") && active)
        {
            Globals.spawnPoint = otherSideSpawnPoint;
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}