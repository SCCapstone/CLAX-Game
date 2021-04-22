using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float growTime;
    public float maxSize;

    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Grow());

        Destroy(gameObject, growTime * 2);
    }

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

    private void OnTriggerEnter(Collider other)
    {
        // 9 is the layer id for enemy
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
