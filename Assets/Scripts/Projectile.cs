using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 gravity;

    public float lifeTime = 2.0f;

    private float creationTime;

    private bool hasPlayedSound = false;

    public AudioSource bulletTimeout;

    void Start()
    {
        transform.position = position;

        creationTime = Time.time;
    }

    void FixedUpdate()
    {
        if (!hasPlayedSound && bulletTimeout != null && Time.time + .5f - creationTime >= lifeTime)
        {
            bulletTimeout.Play();
            hasPlayedSound = true;
        }
        // Destroy object at end of life
        if (Time.time - creationTime >= lifeTime)
        {
            //Debug.Log("was destroyed from project.cs");


            Destroy(gameObject);


            return;
        }

        velocity += gravity * 0.5f * Mathf.Pow(Time.fixedDeltaTime, 2.0f);
        position += velocity * Time.deltaTime;

        transform.position = position;
    }
}
