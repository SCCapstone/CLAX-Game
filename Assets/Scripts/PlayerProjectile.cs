using UnityEngine;
/*
 * Deal damage to enemies that the bullet collides with
 */
public class PlayerProjectile : Projectile
{
    public float damage = 0;

    public AudioSource hitBoss;

    private bool dead = false;

    private void OnTriggerEnter(Collider other)
    {
        if (dead)
        {
            return;
        }

        if (other.gameObject.layer == Globals.enemyLayerNum)
        {
            AliveObject enemy = other.gameObject.GetComponent<AliveObject>();

            if (enemy != null)
            {
                enemy.Damage(damage);
            }

            hitBoss.Play();

            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;

            dead = true;

            Destroy(gameObject, 1.0f);
        }
    }
}
