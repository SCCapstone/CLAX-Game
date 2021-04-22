using UnityEngine;
using UnityEngine.Events;

public class SpawnPoint : MonoBehaviour
{
    [Header("Spawn Properties")]
    public string spawnName;
    public Vector3 spawnOffset;
    public Vector3 spawnRotation;

    [Header("Prefabs")]
    public GameObject playerPrefab;

    public Material inactiveMaterial;
    public Material activeMaterial;

    [Header("Audio")]
    public bool playSound;
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

    public void PlayActivationSound()
    {
        if (playSound)
        {
            activationSound.Play();
        }
    }

    public void SetActive(bool active)
    {
        this.active = active;
    }

    public bool IsActive()
    {
        return active;
    }

    public void SpawnPlayer()
    {
        SetActive(true);

        playerPrefab = Instantiate(playerPrefab, transform.position + transform.TransformDirection(spawnOffset), Quaternion.identity);

        PlayerController pc = playerPrefab.GetComponent<PlayerController>();

        pc.SetCameraAngles(transform.rotation.eulerAngles + spawnRotation);
    }
}
