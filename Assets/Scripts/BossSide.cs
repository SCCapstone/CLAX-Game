using UnityEngine;
using UnityEngine.SceneManagement;


/**
 * controls the behavior of the doors on the boss arenas
 */
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


    /*
     * gets the renderer for the inside of the doorway
     */
    void Start()
    {
        doorRenderer = doorway.transform.GetChild(0).GetComponentInChildren<Renderer>();
    }

    /*
     * on boss death change the door material and play boss death sound
     */
    void Update()
    {
        if (!Globals.boss)
        {
            doorRenderer.material = change;

            bossDeathSound.Play();
        }
    }
    

    /*
     * on player collision, load the next scene with the set spawn.
     * 
     */
    void OnTriggerEnter(Collider other)
    {
        if (!Globals.boss && other.gameObject.CompareTag("Player"))
        {
            Globals.desiredSpawnName = nextDesiredSpawnName;

            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}