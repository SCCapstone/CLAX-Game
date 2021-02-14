using UnityEngine;
using UnityEngine.SceneManagement;

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
            Kill();
        }
    }

    /*
     * Deal damage this object
     */
    public void Damage(float amount)
    {
        // Check if hit cooldown is not active
        if (Time.time - lastHitTime > hitCooldown)
        {
            lastHitTime = Time.time;

            SetHealth(health - amount);

            Debug.Log("New Health is " + health);
        }
    }

    /*
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
    }

    // Kills the object
    public void Kill()
    {
        // TODO: Death events and animations
        // TODO: OnKill event

        if (gameObject.transform.CompareTag("Player"))
        {
            Debug.Log("Reloading scene");

            //Invoke("respawnPlayer", 1.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        Destroy(gameObject);
    }

    public void RespawnPlayer()
    {
        Debug.Log("Ran respawn");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
