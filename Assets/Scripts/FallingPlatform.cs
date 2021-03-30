using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class FallingPlatform : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject platform;
    private Transform startPosition;

    [Header("ColorChange")]
    public Material start;
    public Material end;
    private Material lerpMaterial;
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
    private void OnCollisionEnter(Collision collision)
    {

        if (!isFalling)
        {

            //Debug.Log(collision.);
            if (collision.gameObject.CompareTag("Player"))
            {
                isFalling = true;

                //fallTimer = fallDelay;
            }
        }
    }

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
            float distanceToPlayer = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position,
                transform.position);
            //Debug.Log("distance to player " + distanceToPlayer);

            if (platformRenderer.enabled && fallTimer + 1 >= fallDelay && fallSound.isPlaying == false &&
                distanceToPlayer < 20)
            {

                fallSound.Play();

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
                colorChange(fallTimer / fallDelay);
        }
    }

    private void colorChange(float change)
    {
        if (change > 1)
            change = 1;

        platformRenderer.material.Lerp(start, end, change);

    }
}
