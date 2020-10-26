using UnityEngine;
using UnityEngine.InputSystem;

public class FallingPlatform : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject platform;

    [Header("ColorChange")]
    public Material start;
    public Material end;
    private Color lerpedColor;
    private Color startColor;
    private Color endColor;
    private Renderer platformRenderer;

    [Header("Falling")]
    public bool collisionFall = true;
    public float fallSpeed;
    public float fallAcceleration;
    public float fallDelay;
    public float destroyAtHeight;

    private bool isFalling = false;
    private float fallTimer = 0.0f;

    void Start()
    {
        lerpedColor = start.GetColor("_Color");
        endColor = end.GetColor("_Color");
        startColor = lerpedColor;
        platformRenderer = platform.GetComponentInChildren<Renderer>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!isFalling)
            if (collision.gameObject.CompareTag("Player"))
            {
                isFalling = true;
                //fallTimer = fallDelay;
            }
    }

    void FixedUpdate()
    {
        if (isFalling)
        {
            if (transform.position.y <= destroyAtHeight)
            {
                Destroy(gameObject);

                return;
            }

            if (fallTimer >= fallDelay)
            {
                float speed = (fallAcceleration * Mathf.Pow(Mathf.Abs(fallTimer), 2.0f)) + fallSpeed;

                Vector3 nextPosition = transform.position;
                nextPosition.y -= speed * Time.fixedDeltaTime;

                transform.position = nextPosition;
            }

            fallTimer += Time.fixedDeltaTime;
            if(fallTimer > 0)
                colorChange(fallTimer/fallDelay);
        }
    }

    private void colorChange(float change)
    {
        if (change > 1)
            change = 1;
        if (lerpedColor != endColor)
        {
            lerpedColor = Color.Lerp(startColor, endColor, change);
            platformRenderer.material.SetColor("_Color", lerpedColor);
        }
    }
}
