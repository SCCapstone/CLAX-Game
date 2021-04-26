using UnityEngine;
using UnityEngine.Events;
/*
 * trigger an event when the player walks into a door
 */
public class PortalTrigger : MonoBehaviour
{
    public UnityEvent OnPlayerEnter = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPlayerEnter.Invoke();
        }
    }
}
