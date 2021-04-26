using UnityEngine;
using UnityEngine.SceneManagement;
/**
 * controls the non boss arena doors
 */
public class HubSide : MonoBehaviour
{
    public PortalTrigger doorTrigger;

    public string nextSceneName;
    public string nextDesiredSpawnName;

    public bool blockDuringBoss = false;

    public Material inactiveMaterial;
    public Material activeMaterial;

    private Renderer doorRenderer;

    private bool isSpawn = false;
    
    /**
     * spawns the player if the door is the desired spawn point of the previous level
     */
    void Start()
    {
        doorRenderer = GetComponentInChildren<Renderer>();

        if(Globals.desiredSpawnName != null && Globals.desiredSpawnName == GetComponentInChildren<SpawnPoint>().spawnName)
        {
            isSpawn = false;
        }

        doorTrigger.OnPlayerEnter.AddListener(delegate
        {
            if (IsActive())
            {
                Globals.desiredSpawnName = nextDesiredSpawnName;

                SceneManager.LoadSceneAsync(nextSceneName);
            }
        });
    }
    
    void Update()
    {
        doorRenderer.material = IsActive() ? activeMaterial : inactiveMaterial;
    }

    /**
     * sets the door to active or inactive based on wether they were a spawn or boss door
     */
    private bool IsActive()
    {
        if (blockDuringBoss)
        {
            return !Globals.boss;
        }

        if (isSpawn)
        {
            return false;
        }

        return true;
    }
}
