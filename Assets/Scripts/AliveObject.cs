using UnityEngine;

public class AliveObject : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public float hitCooldown = 0.5f;

    float lastHitTime = 0.0f;

    void Start()
    {
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if (health <= 0.0f)
        {
            kill();
        }
    }

    public float getHealth()
    {
        return health;
    }

    /*
     * Deal damage this object
     * Returns how much real damage was dealt
     */
    public void damage(float amount)
    {
        // Check if hit cooldown is not active
        if (Time.time - lastHitTime > hitCooldown)
        {
            lastHitTime = Time.time;

            setHealth(health - amount);
        }
    }

    /*
     * Sets the health of the object
     * Does not trigger hit cooldowns
     */
    public void setHealth(float amount)
    {
        health = amount;

        if (health < 0.0f)
        {
            health = 0.0f;
        }
    }

    // Kills the object
    public void kill()
    {
        // TODO: Death events and animations
        // TODO: OnKill event

        Destroy(gameObject);
    }
}
