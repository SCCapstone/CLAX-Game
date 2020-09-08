using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 2.0f;

    public Vector3 position = Vector3.zero;
    public Vector3 velocity = Vector3.zero;
    public Vector3 gravity = Vector3.zero;

    private float creationTime;

    // Start is called before the first frame update
    void Start()
    {
        creationTime = Time.time;

        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy at end of life
        if (Time.time - creationTime > lifeTime)
        {
            Destroy(gameObject);

            return;
        }

        // Skip update
        if (velocity == Vector3.zero && gravity == Vector3.zero)
        {
            return;
        }

        // TODO: Check for collisions with projectile

        if (gravity != Vector3.zero)
        {
            velocity += gravity * 0.5f * Mathf.Pow(Time.fixedDeltaTime, 2.0f);
        }

        position += velocity * Time.deltaTime;

        transform.position = position;
    }
}
