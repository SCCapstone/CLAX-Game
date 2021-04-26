using UnityEngine;

/**
 * Controls the behavior of the game objects that need to be destroyed or have a health bar
 */
public class AliveObject : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public bool invulnerable;

    public float hitCooldown = 0.5f;

    protected float lastHitTime = 0.0f;
    public AudioSource deathSound;

    public delegate void DeathHandler();
    public event DeathHandler onDeath;

    protected bool dead = false;

    void Start()
    {
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if (health <= 0.0f)
        {
            Kill();
        }
        else if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    /**
     * Deal damage this object
     */
    public void Damage(float amount)
    {
        // Check if hit cooldown is not active
        if (!invulnerable && Time.time - lastHitTime > hitCooldown)
        {
            lastHitTime = Time.time;

            SetHealth(health - amount);
        }
    }

    /**
     * Sets the health of the object
     * Does not trigger hit cooldowns
     */
    public void SetHealth(float amount)
    {
        health = amount;

        if (health <= 0.0f)
        {
            health = 0.0f;

            Kill();
        }
        else if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    /**
     * Sets the max health of the object
     */
    public void SetMaxHealth(float amount)
    {
        maxHealth = amount;
    }

    /**
     * Kills object
     */
    virtual public void Kill()
    {
        if (dead)
        {
            return;
        }

        dead = true;

        if (deathSound != null && !deathSound.isPlaying)
        {
            deathSound.volume = Globals.audioSettings.gameVolume;
            deathSound.Play();
        }

        if (onDeath == null)
        {
            // Default behaviour
            Destroy(gameObject);
        }
        else
        {
            onDeath();
        }
    }
}
