using UnityEngine;

/*
 * Controls the logic for projectiles moving and despawning after a certain point
 */
public class Projectile : MonoBehaviour
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 gravity;

    public float lifeTime = 2.0f;

    private float creationTime;

    void Start()
    {
        transform.position = position;

        creationTime = Time.time;
    }

    void FixedUpdate()
    {
        // Destroy object at end of life
        if (Time.time - creationTime >= lifeTime)
        {
            Destroy(gameObject);

            return;
        }

        velocity += gravity * 0.5f * Mathf.Pow(Time.fixedDeltaTime, 2.0f);
        position += velocity * Time.deltaTime;

        transform.position = position;
    }
}
