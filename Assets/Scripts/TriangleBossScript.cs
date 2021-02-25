using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriangleBossScript : AliveObject
{
    private const float UPDATE_INTERVAL = 1.0f;
    private const float ACTION_TIMEOUT = 10.0f;
    private const float DEATH_ANIMATION_TIME = 8.0f;
    private const float DEATH_ANIMATION_TARGET_HEIGHT = -2.0f;
    private const float DEATH_ANIMATION_INTENSITY = 0.15f;

    public enum BossState
    {
        Decide,
        Move,
        Attack
    }

    [HeaderAttribute("Movement")]
    public float movementDelay;

    [HeaderAttribute("Attack")]
    public GameObject projectilePrefab;
    public float projectileDamage;
    public float projectileSpeed;
    public float projectileLifetime;
    public float projectileDelay;

    public float attackDuration;
    public float attackWaves;

    public float attackIntervalMin;
    public float attackIntervalMax;

    public float minimumRange;
    public float maximumRange;

    [HeaderAttribute("Action Weights")]
    public Dictionary<BossState, float> actionWeights = new Dictionary<BossState, float>()
    {
        {BossState.Move, 10},
        {BossState.Attack, 20}
    };

    GameObject target;

    private BossState state;
    private bool inAction;
    private float actionStartTime;

    private float nextAttackTime = 0.0f;

    private float lastUpdateTime;

    bool dead = false;

    void Start()
    {
        state = BossState.Decide;

        lastUpdateTime = 0;
    }

    void OnDestroy()
    {
        // Cleanup objects if necessary

        globals.pyramid = true;
    }

    void FixedUpdate()
    {
        // TODO: This should really be handled in the parent class
        if (health <= 0.0f)
        {
            Kill();
        }

        if (dead || Time.time - lastUpdateTime < UPDATE_INTERVAL)
        {
            return;
        }

        Debug.Log("Begin Triangle Boss update cycle");

        CheckSafety();
        GetTarget();

        if (state == BossState.Decide)
        {
            // Choose next state randomly based on weights

            state = GetRandomWeightedState();
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

    BossState GetRandomWeightedState()
    {
        var choices = actionWeights.ToList();
        float totalWeight = 0.0f;

        // Sort in ascending weights
        choices.Sort((x, y) => x.Value.CompareTo(y.Value));

        // Get sum of weights
        foreach (var pair in choices)
        {
            if (pair.Key == BossState.Decide)
            {
                Debug.LogWarning("Decide is a possible outcome of Decide state. Is this intentional?");
            }

            totalWeight += pair.Value;
        }

        // Choosed weighted random
        float r = Random.Range(0.0f, totalWeight);

        foreach (var pair in choices)
        {
            if (r < pair.Value)
            {
                return pair.Key;
            }
        }

        // Fallback to choose random choice
        return choices[(int)Random.Range(0.0f, choices.Count)].Key;
    }

    public override void Kill()
    {
        if (dead)
        {
            return;
        }

        dead = true;

        StopAllCoroutines();

        Debug.Log("Death throes");

        StartCoroutine(KillCoroutine());
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
        Vector3 nextPosition = center + (direction * distance) + new Vector3(0.0f, transform.localScale.y / 2.0f, 0.0f);

        Vector3 startPosition = transform.position;

        for (float alpha = 0.0f; alpha < movementDelay; alpha += Time.fixedDeltaTime)
        {
            transform.position = Vector3.Lerp(startPosition, nextPosition, alpha);
            transform.LookAt(target.transform.position);

            yield return new WaitForFixedUpdate();
        }

        transform.position = nextPosition;
        transform.LookAt(target.transform.position);

        //yield return new WaitForSeconds(1.0f);

        EndAction();
    }

    IEnumerator ProjectileLoop(float delay)
    {
        float[] angles =
        {
            0.0f, 90.0f, 180.0f, 270.0f
        };

        GameObject projectileInstance;
        Projectile projectile;
        ContactDamagePlayer contactDamage;

        Vector3 dir;

        while (true)
        {
            float currentAngle = transform.rotation.eulerAngles.y;

            foreach (float angle in angles)
            {
                dir = Quaternion.Euler(0.0f, angle + currentAngle, 0.0f) * Vector3.forward;

                projectileInstance = Instantiate(projectilePrefab);
                projectile = projectileInstance.GetComponent<Projectile>();
                contactDamage = projectileInstance.GetComponent<ContactDamagePlayer>();

                contactDamage.damageAmount = projectileDamage;

                projectile.position = transform.position + (dir * projectileInstance.transform.localScale.x / 2.0f);
                projectile.velocity = dir * projectileSpeed;
                projectile.lifeTime = projectileLifetime;
            }

            yield return new WaitForSeconds(delay);
        }
    }

    IEnumerator Attack()
    {
        if (!target)
        {
            yield return null;
        }

        Vector3 currentPosition;
        Vector3 targetPosition;

        Vector3 startPosition = transform.position;
        Vector3 startRotation = transform.rotation.eulerAngles;

        // Ease into start position

        currentPosition = transform.position;
        targetPosition = startPosition + new Vector3(0.0f, 2.0f, 0.0f);

        for (float i = 0.0f; i < 1.0f; i += Time.fixedDeltaTime)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, i);

            yield return new WaitForFixedUpdate();
        }

        transform.position = startPosition;

        // Attack animation and projectiles

        Vector3 pos;
        Vector3 rot;

        Coroutine projectileLoop = null;

        for (float i = 0.0f; i < 1.0f; i += Time.fixedDeltaTime / attackDuration)
        {
            pos = startPosition + new Vector3(0.0f, Mathf.Cos(i * Mathf.PI * attackWaves) * 2.0f, 0.0f);
            rot = startRotation + new Vector3(0.0f, i * 360.0f, 0.0f);

            // Starts projectiles after the first wave
            if (projectileLoop == null && i > 1.0f / (float)attackWaves)
            {
                projectileLoop = StartCoroutine("ProjectileLoop", projectileDelay);
            }

            transform.position = pos;
            transform.rotation = Quaternion.Euler(rot);

            yield return new WaitForFixedUpdate();
        }

        if (projectileLoop != null)
        {
            StopCoroutine(projectileLoop);
        }

        // Ease back to start position

        currentPosition = transform.position;
        targetPosition = startPosition;

        for (float i = 0.0f; i < 1.0f; i += Time.fixedDeltaTime)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, i);

            yield return new WaitForFixedUpdate();
        }

        transform.position = startPosition;

        // Brief pause before changing state

        yield return new WaitForSeconds(0.5f);

        EndAction();
    }

    // TODO: Coroutine wrapper, the kill function really should be a coroutine by default
    IEnumerator KillCoroutine()
    {
        // Rotate boss to face random angle

        transform.rotation = Quaternion.Euler(
            Random.Range(0, 360),
            Random.Range(0, 360),
            Random.Range(0, 360)
        );

        // Play shaking and sinking animation

        Transform startTransform = transform;
        Transform endTransform = transform;

        endTransform.position += new Vector3(0.0f, DEATH_ANIMATION_TARGET_HEIGHT, 0.0f);

        Vector3 v;

        for (float i = 0.0f; i < 1.0f; i += Time.fixedDeltaTime / DEATH_ANIMATION_TIME)
        {
            v = Vector3.Lerp(startTransform.position, endTransform.position, i);
            
            transform.position = v + (Random.insideUnitSphere * DEATH_ANIMATION_INTENSITY);
            
            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
    }
}
