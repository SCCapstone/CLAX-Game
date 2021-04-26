using System.Collections;
using UnityEngine;
/**
 * This file takes care of the logic for the second attack
 * The second attack creates a black orb that grows, shrinks, 
 * then dissapears. If a boss or door is touched by the orb it 
 * will take damage.
 */
public class Explosion : MonoBehaviour
{
    public float growTime;
    public float maxSize;

    public float damage;

    /**
     * Start is called before the first frame update
     */
    void Start()
    {
        StartCoroutine(Grow());

        Destroy(gameObject, growTime * 2);
    }
    /**
     * This method takes care of growing the attack by using the game 
     * time grow for a set amount of time, by changing the scale of the
     * object
     */
    private IEnumerator Grow()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = new Vector3(maxSize, maxSize, maxSize);

        float startTime = Time.time;
        float elapsed = 0.0f;

        while (elapsed < growTime)
        {
            elapsed = Time.time - startTime;

            transform.localScale = Vector4.Lerp(startScale, endScale, elapsed / growTime);

            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(Shrink());
    }

    /**
     * This method takes care of shrink the attack by using the game 
     * time shrink for a set amount of time, by changing the scale of the
     * object
     */
    private IEnumerator Shrink()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;

        float startTime = Time.time;
        float elapsed = 0.0f;

        while (elapsed < growTime)
        {
            elapsed = Time.time - startTime;

            transform.localScale = Vector4.Lerp(startScale, endScale, elapsed / growTime);

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }

    /**
     * This method takes care of the collisions for the orb and other
     * objects sucs as the boss and dorrs. Damage will be applied and 
     * 
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Globals.enemyLayerNum)
        {
            AliveObject enemy = other.gameObject.GetComponent<AliveObject>();

            enemy.Damage(damage);

            Destroy(gameObject);
        }

        if (other.CompareTag("EnemyAttack"))
        {
            Destroy(other.gameObject);
        }
    }
}
