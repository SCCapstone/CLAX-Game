using UnityEngine;

public class PlayerProjectile : Projectile
{
    public float damage = 0;

    public AudioSource hitBoss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Globals.enemyLayerNum)
        {
            hitBoss.Play();

            AliveObject enemy = other.gameObject.GetComponent<AliveObject>();

            if (enemy != null)
            {
                enemy.Damage(damage);
            }
        }

        Destroy(gameObject, .5f);
    }
}
