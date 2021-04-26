using UnityEngine;
using UnityEngine.SceneManagement;
/*
 * Enable the exit door for each level.
 */
public class Portal : MonoBehaviour
{
    public PortalTrigger portalTrigger;

    public string nextSceneName;
    public string nextDesiredSpawnName;

    public bool blockDuringBoss = false;

    public Renderer doorRenderer;
    public Material inactiveMaterial;
    public Material activeMaterial;

    private bool forceDisable = false;
    private bool isSpawn = false;

    void Start()
    {
        portalTrigger.OnPlayerEnter.AddListener(delegate
        {
            if (IsActive())
            {
                Globals.desiredSpawnName = nextDesiredSpawnName;

                SceneManager.LoadScene(nextSceneName);
            }
        });
    }

    void Update()
    {
        doorRenderer.material = IsActive() ? activeMaterial : inactiveMaterial;

        SpawnPoint sp = GetComponentInChildren<SpawnPoint>();

        if (sp != null && sp.IsActive())
        {
            isSpawn = true;
        }
    }

    public void SetForceDisable(bool disable)
    {
        forceDisable = disable;
    }

    public bool IsActive()
    {
        if (forceDisable)
        {
            return false;
        }

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
