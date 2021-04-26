using UnityEngine;

/**
 * Allows the player to deal contact damage to the alive object
 */
public class ContactDamagePlayer : MonoBehaviour
{
    public float damageAmount;

    /*
     * cause the player to deal contact damage to things it bumps into
     */
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var aliveObject = other.gameObject.GetComponent<AliveObject>();

            if (aliveObject != null)
            {
                aliveObject.Damage(damageAmount);
            }
        }
    }
}
