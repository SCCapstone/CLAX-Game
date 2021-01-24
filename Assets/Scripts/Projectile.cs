using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 2.0f;

    public Vector3 position = Vector3.zero;
    //public Vector3 velocity = Vector3.zero;
    public Vector3 velocity;
    public Vector3 gravity;
    public float damage = 0;

    public int enemyLayerNum;
    bool wasInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
        // Destroy at end of life

        transform.position = position;
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(int enemyLayerNum, float damage = 10)
    {
        wasInitialized = true;
        this.enemyLayerNum = enemyLayerNum;
        this.damage = damage;
        //this.velocity = this.transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Triggered");

        // 9 is the layer id for enemy
        if (other.gameObject.layer == enemyLayerNum)
        {
            AliveObject enemy = other.gameObject.GetComponent<AliveObject>();

            enemy.Damage(damage);

            Destroy(gameObject);
        }

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Collided");
    //    if (collision.gameObject.name == "Boss1")
    //    {
    //        Debug.Log("hit boss");

    //    }

    //    //foreach (var c in collision.contacts)
    //    //{
    //    //    Debug.Log("hit " + c);

    //    //}
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        //velocity = new Vector3(50, 0, 0);

        // Skip update
        if (velocity == Vector3.zero && gravity == Vector3.zero)
        {
            return;
        }

        // TODO: Check for collisions with projectile
        //Debug.Log("Vel " + velocity);
        //Debug.Log("grav " + gravity);
        //velocity += gravity;

        //position += velocity;

        if (gravity != Vector3.zero)
        {
            velocity += gravity * 0.5f * Mathf.Pow(Time.fixedDeltaTime, 2.0f);
        }

        position += velocity * Time.deltaTime;

        transform.position = position;
    }
}
