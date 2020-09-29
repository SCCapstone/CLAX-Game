using UnityEngine;
using UnityEngine.InputSystem;

public class FallingPlatform : MonoBehaviour
{
    [Header("Falling")]
    public bool collisionFall = true;
    public float fallSpeed;
    public float fallAcceleration;
    public float fallDelay;
    public float destroyAtHeight;

    private bool isFalling = false;
    private float fallTimer = 0.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isFalling = true;
            fallTimer = fallDelay;
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

            if (fallTimer <= 0.0f)
            {
                float speed = (fallAcceleration * Mathf.Pow(Mathf.Abs(fallTimer), 2.0f)) + fallSpeed;

                Vector3 nextPosition = transform.position;
                nextPosition.y -= speed * Time.fixedDeltaTime;

                transform.position = nextPosition;
            }

            fallTimer -= Time.fixedDeltaTime;
        }
    }
}
