using UnityEngine;
using UnityEngine.Events;

public class SpawnPoint : MonoBehaviour
{
    [Header("Spawn Properties")]
    public string spawnName;

    [Header("Prefabs")]
    public GameObject playerPrefab;

    public Material inactiveMaterial;
    public Material activeMaterial;

    public AudioSource activationSound;

    public UnityEvent OnPlayerEnter = new UnityEvent();

    private bool active = false;
    private Renderer spawnPointRenderer;

    void Start()
    {
        spawnPointRenderer = gameObject.GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        spawnPointRenderer.material = active ? activeMaterial : inactiveMaterial;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPlayerEnter.Invoke();
        }
    }

    public void SetActive(bool active)
    {
        if (!this.active)
        {
            this.active = active;

            activationSound.Play();
        }
    }

    public void SetActiveSilent(bool active)
    {
        this.active = active;
    }

    public bool IsActive()
    {
        return active;
    }

    public void SpawnPlayer()
    {
        SetActiveSilent(true);

        playerPrefab = Instantiate(playerPrefab, transform.position, Quaternion.identity);
    }
}
