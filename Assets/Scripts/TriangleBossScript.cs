using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleBossScript : MonoBehaviour
{
    private const float UPDATE_INTERVAL = 0.5f;

    [HeaderAttribute("Movement")]
    public float movementDelay;

    [HeaderAttribute("Attack")]
    public float attackIntervalMin;
    public float attackIntervalMax;

    public float minimumRange;
    public float maximumRange;

    float nextAttackTime = 0.0f;

    GameObject target;

    private enum BossState
    {
        Decide,
        Move,
        Attack
    }

    private BossState state;

    private float lastUpdateTime;

    void Start()
    {
        state = BossState.Decide;

        lastUpdateTime = 0;
    }

    void OnDestroy()
    {
        // Cleanup objects if necessary
    }

    void FixedUpdate()
    {
        if (Time.time - lastUpdateTime < UPDATE_INTERVAL)
        {
            return;
        }

        Debug.Log("Begin Triangle Boss update cycle");

        GetTarget();

        switch (state)
        {
            case BossState.Decide:
                // Do not execute a state coroutine
                // Randomly choose between moving or attacking at first
                state = Random.value < 0.5f ? BossState.Move : BossState.Attack;

                break;
            case BossState.Move:
                StartCoroutine("Move");

                break;
            case BossState.Attack:
                if (Time.time - nextAttackTime >= 0.0f)
                {
                    nextAttackTime = Time.time + Random.Range(attackIntervalMin, attackIntervalMax);

                    StartCoroutine("Attack");
                }

                break;
            default:
                // Should never happen
                Debug.Log("Triangle boss encountered an unknown error");
                Debug.Log("This should never happen");

                break;
        }

        lastUpdateTime = Time.time;
    }

    void GetTarget()
    {
        // In case there are >1 objects tagged with player, we want to select them randomly
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        int i = Random.Range(0, targets.Length);

        target = targets[i];
    }

    IEnumerator Move()
    {
        // Get random position and height around a circle of random radius centered around the player

        Vector3 center = target.transform.position;

        float angle = Random.Range(0.0f, 360.0f);
        float distance = Random.Range(minimumRange, maximumRange);

        Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
        Vector3 nextPosition = center + (direction * distance);

        Vector3 startPosition = transform.position;

        for (float alpha = 0.0f; alpha < movementDelay; alpha += Time.fixedDeltaTime)
        {
            transform.position = Vector3.Lerp(startPosition, nextPosition, alpha);
            transform.LookAt(target.transform.position);

            yield return new WaitForFixedUpdate();
        }

        transform.position = nextPosition;
        transform.LookAt(target.transform.position);

        yield return new WaitForSeconds(1.0f);
        
        state = BossState.Decide;
    }

    IEnumerator Attack()
    {
        if (!target)
        {
            yield return null;
        }

        state = BossState.Decide;
    }
}
