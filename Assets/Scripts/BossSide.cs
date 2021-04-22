using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSide : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject doorway;
    public GameObject player;
    public string sceneName;
    public string nextDesiredSpawnName;

    public Material change;

    private Renderer doorRenderer;

    public AudioSource bossDeathSound;

    void Start()
    {
        doorRenderer = doorway.transform.GetChild(0).GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (!Globals.boss)
        {
            doorRenderer.material = change;

            bossDeathSound.Play();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (!Globals.boss && other.gameObject.CompareTag("Player"))
        {
            Globals.desiredSpawnName = nextDesiredSpawnName;

            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}