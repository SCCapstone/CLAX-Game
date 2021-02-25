using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamagePlayer : MonoBehaviour
{
    public float damageAmount = 10;

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("other tag" + other.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("hit player");

            var enemy = other.gameObject.GetComponent<AliveObject>();
            enemy.Damage(damageAmount);
            //Debug.Log("player health" + enemy.health);

        }
    }
}
