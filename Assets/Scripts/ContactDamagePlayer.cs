using UnityEngine;

public class ContactDamagePlayer : MonoBehaviour
{
    public float damageAmount;

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
