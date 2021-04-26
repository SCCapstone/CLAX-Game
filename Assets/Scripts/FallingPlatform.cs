using UnityEngine;
/**
 * Controls falling platform behavior, holds sound and movement
 */
public class FallingPlatform : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject platform;
    private Transform startPosition;

    [Header("ColorChange")]
    public Material start;
    public Material end;

    private Renderer platformRenderer;
    private BoxCollider platformCollider;

    [Header("Falling")]
    public bool collisionFall = true;
    public float fallSpeed;
    public float fallAcceleration;
    public float fallDelay;
    public float respawnDelay = 30;
    public float disappearHeight = -10;

    private bool isFalling = false;
    private float fallTimer = 0.0f;

    public AudioSource fallSound;

    void Start()
    {
        startPosition = platform.transform;
        platformRenderer = platform.GetComponentInChildren<Renderer>();
        platformCollider = platform.GetComponentInChildren<BoxCollider>();
    }


    /**
     * Platform falls when player collides with it
     */
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isFalling)
            {
                isFalling = true;
            }
        }
    }

    /**
     * causes the platform to fall for a bit, de render wait a period of time, and then reset.
     */
    void FixedUpdate()
    {
        if (isFalling)
        {
            if (fallTimer >= respawnDelay)
            {
                fallTimer = 0.0f;
                isFalling = false;
                platformRenderer.material = start;
                platformRenderer.enabled = true;
                platformCollider.enabled = true;
                transform.position = startPosition.position;
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                float distance = Vector3.Distance(player.transform.position, transform.position);

                if (platformRenderer.enabled && fallTimer + 1 >= fallDelay && distance < 20.0f)
                {
                    if (!!fallSound.isPlaying)
                    {
                        fallSound.Play();
                    }
                }
            }

            if (fallTimer >= fallDelay && platformRenderer.enabled)
            {
                float speed = (fallAcceleration * Mathf.Pow(Mathf.Abs(fallTimer), 2.0f)) + fallSpeed;

                Vector3 nextPosition = transform.position;
                nextPosition.y -= speed * Time.fixedDeltaTime;

                transform.position = nextPosition;

                if (transform.position.y <= disappearHeight)
                {
                    platformRenderer.enabled = false;
                    platformCollider.enabled = false;
                }
            }

            fallTimer += Time.fixedDeltaTime;

            if (fallTimer > 0)
            {
                colorChange(fallTimer / fallDelay);
            }
        }
    }

    /**
     * changes the color of the material
     */
    private void colorChange(float change)
    {
        if (change > 1)
        {
            change = 1;
        }

        platformRenderer.material.Lerp(start, end, change);
    }
}
