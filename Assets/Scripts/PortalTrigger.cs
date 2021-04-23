using UnityEngine;
using UnityEngine.Events;

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
