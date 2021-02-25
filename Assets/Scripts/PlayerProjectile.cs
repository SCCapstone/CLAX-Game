using UnityEngine;

public class PlayerProjectile : Projectile
{
    public float damage = 0;
    public int enemyLayerNum;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Triggered");

        // 9 is the layer id for enemy
        if (other.gameObject.layer == enemyLayerNum)
        {
            Debug.Log("hit enemy");
            AliveObject enemy = other.gameObject.GetComponent<AliveObject>();

            enemy.Damage(damage);

            Destroy(gameObject);
        }

    }
}
