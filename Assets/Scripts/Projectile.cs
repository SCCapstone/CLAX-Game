using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 2.0f;

    public Vector3 position = Vector3.zero;
    public Vector3 velocity = Vector3.zero;
    public Vector3 gravity = Vector3.zero;
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


    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("layer of object hit " + collision.gameObject.layer.ToString());
    //    //if (collision.gameObject.layer == )
    //}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");

        //9 is the layer id for enemy
        if (other.gameObject.layer == enemyLayerNum)
        {
            var enemy = other.gameObject.GetComponent<AliveObject>();
            enemy.health -= damage;

        }

    }


    // Update is called once per frame
    void Update()
    {
        // Skip update
        if (velocity == Vector3.zero && gravity == Vector3.zero)
        {
            return;
        }

        // TODO: Check for collisions with projectile

        velocity += gravity;
        position += velocity;

        transform.position = position;
    }
}
