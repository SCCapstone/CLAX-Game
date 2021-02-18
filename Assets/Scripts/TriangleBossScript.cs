using System.Collections;
using UnityEngine;

public class TriangleBossScript : MonoBehaviour
{
    private const float UPDATE_INTERVAL = 1.0f;
    private const float ACTION_TIMEOUT = 10.0f;

    [HeaderAttribute("Movement")]
    public float movementDelay;

    [HeaderAttribute("Attack")]
    public float attackIntervalMin;
    public float attackIntervalMax;

    public float minimumRange;
    public float maximumRange;

    GameObject target;

    private enum BossState
    {
        Decide,
        Move,
        Attack
    }

    private BossState state;
    private bool inAction;
    private float actionStartTime;

    private float nextAttackTime = 0.0f;

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

        CheckSafety();

        GetTarget();

        if (state == BossState.Decide)
        {
            // Randomly choose between moving or attacking at first
            state = Random.value < 0.5f ? BossState.Move : BossState.Attack;
        }

        switch (state)
        {
            case BossState.Move:
                StartAction("Move");

                break;
            case BossState.Attack:
                if (Time.time - nextAttackTime >= 0.0f)
                {
                    nextAttackTime = Time.time + Random.Range(attackIntervalMin, attackIntervalMax);

                    StartAction("Attack");
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

    void StartAction(string actionName)
    {
        if (inAction)
        {
            return;
        }

        inAction = true;
        actionStartTime = Time.time;

        StartCoroutine(actionName);
    }

    void EndAction()
    {
        inAction = false;
        state = BossState.Decide;

        StopAllCoroutines();
    }

    // Safety checks to ensure the show goes on in the case of exceptional circumstances
    void CheckSafety()
    {
        // If not performing action, or in a deciding state, or action has timed out, then end all actions
        if (!inAction || state == BossState.Decide || Time.time - actionStartTime > ACTION_TIMEOUT)
        {
            EndAction();
        }
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

        EndAction();
    }

    IEnumerator Attack()
    {
        if (!target)
        {
            yield return null;
        }

        // Filler movement for observational debugging only

        Vector3 startPosition = transform.position;

        float delta = Time.fixedDeltaTime;

        float offset;

        for (float i = 0.0f; i < 1.0f; i += delta)
        {
            offset = Mathf.Sin(i * Mathf.PI) * 2.0f;

            transform.position = new Vector3(startPosition.x, startPosition.y + offset, startPosition.z);

            yield return new WaitForFixedUpdate();
        }

        transform.position = startPosition;

        yield return new WaitForSeconds(0.2f);

        EndAction();
    }
}
