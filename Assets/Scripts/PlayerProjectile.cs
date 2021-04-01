using UnityEngine;

public class PlayerProjectile : Projectile
{
    public float damage = 0;

    // TODO: Get enemy layer by name or constant? Magic numbers are legitimately scary.
    // Enemy layer is 9
    public int enemyLayerNum;

    public AudioSource hitBoss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == enemyLayerNum)
        {
            //Debug.Log("hit enemy");
            hitBoss.Play();
            AliveObject enemy = other.gameObject.GetComponent<AliveObject>();

            enemy.Damage(damage);

            Destroy(gameObject, .5f);
        }
    }
}
