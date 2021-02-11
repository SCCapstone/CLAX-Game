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

    [Header("Falling")]
    public bool collisionFall = true;
    public float fallSpeed;
    public float fallAcceleration;
    public float fallDelay;
    public float respawnDelay = 30;
    public float disappearHeight = -10;

    private bool isFalling = false;
    private float fallTimer = 0.0f;

    void Start()
    {
        startPosition = platform.transform;
        platformRenderer = platform.GetComponentInChildren<Renderer>();
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
                transform.position = startPosition.position;
            }

            if (fallTimer >= fallDelay && platformRenderer.enabled)
            {
                float speed = (fallAcceleration * Mathf.Pow(Mathf.Abs(fallTimer), 2.0f)) + fallSpeed;

                Vector3 nextPosition = transform.position;
                nextPosition.y -= speed * Time.fixedDeltaTime;

                transform.position = nextPosition;
                if (transform.position.y <= disappearHeight)
                    platformRenderer.enabled = false;
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
