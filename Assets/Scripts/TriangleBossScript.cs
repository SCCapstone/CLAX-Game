using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Controls the triangle boss and their attacks
 */
public class TriangleBossScript : AliveObject
{
    private const float UPDATE_INTERVAL = 1.0f;
    private const float ACTION_TIMEOUT = 10.0f;
    private const float DEATH_ANIMATION_TIME = 5.0f;
    private const float DEATH_ANIMATION_TARGET_HEIGHT = -2.0f;
    private const float DEATH_ANIMATION_INTENSITY = 0.15f;

    public enum BossState
    {
        Decide,
        Move,
        Attack
    }

    public enum AttackType
    {
        Attack1,
        Attack2,
        Attack3
    }

    [Header("ExitDoor")]
    public GameObject exitDoor;
    public float exitDoorStartHeight;
    public float exitDoorEndHeight;

    [Header("Movement")]
    public float movementDelay;
    public float movementDuration;

    [Header("Attack")]
    public GameObject projectilePrefab;
    public float projectileDamage;
    public float projectileSpeed;
    public float projectileLifetime;
    public float projectileDelay;

    public float attack1Duration;
    public float attack1Waves;
    public float attack1Rotations;

    public float attack2Duration;
    public float attack2Distance;
    public float attack2Rotations;

    public float attackIntervalMin;
    public float attackIntervalMax;

    public float minimumRange;
    public float maximumRange;

    [Header("Audio")]
    public AudioSource attack1Sound;
    public AudioSource attack2Sound;

    [Header("Action Weights")]
    public Dictionary<BossState, float> actionWeights = new Dictionary<BossState, float>()
    {
        {BossState.Move, 10},
        {BossState.Attack, 20}
    };
    Dictionary<AttackType, float> attackWeights = new Dictionary<AttackType, float>()
    {
        {AttackType.Attack1, 20.0f},
        {AttackType.Attack2, 10.0f},
        {AttackType.Attack3, 0.0f}
    };

    private GameObject target;

    private BossState state;
    private bool inAction;
    private float actionStartTime;

    private float lastUpdateTime;

    void Awake()
    {
        onDeath += OnDeath;
    }

    void Start()
    {
        Globals.boss = true;

        SetExitDoorHeight(exitDoorStartHeight);
        exitDoor.SetActive(false);

        state = BossState.Decide;
        lastUpdateTime = Time.time - UPDATE_INTERVAL + 1.5f;
    }

    void OnDestroy()
    {
        // Set exit door active if it isn't already

        SetExitDoorHeight(exitDoorEndHeight);
        if (exitDoor != null)
            exitDoor.SetActive(true);

        // Cleanup objects if necessary

        Globals.pyramid = true;
        Globals.boss = false;
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

        if (target == null)
        {
            return;
        }

        if (state == BossState.Decide)
        {
            // Choose next state randomly based on weights

            state = GetRandomWeighted(actionWeights);

            if (state == BossState.Decide)
            {
                Debug.LogWarning("Decide state did not transition to different state, is this intentional?");
            }
        }

        switch (state)
        {
            case BossState.Move:
                StartAction(Move());

                break;
            case BossState.Attack:
                AttackType attack = GetRandomWeighted(attackWeights);

                switch (attack)
                {
                    case AttackType.Attack1:
                        StartAction(Attack1());

                        break;
                    case AttackType.Attack2:
                        StartAction(Attack2());

                        break;
                    case AttackType.Attack3:
                        StartAction(Attack3());

                        break;
                    default:
                        // Should never happen
                        Debug.Log("Triangle boss has encountered an unknown attack state");

                        break;
                }

                break;
            default:
                // Should never happen
                Debug.Log("Triangle boss encountered an unknown boss state");

                break;
        }

        lastUpdateTime = Time.time;
    }
    /*
     * Give each phase a random weight for how often they occur
     */
    T GetRandomWeighted<T>(Dictionary<T, float> weightedChoices)
    {
        var choices = weightedChoices.ToList();
        float totalWeight = 0.0f;

        // Sort in ascending weights
        choices.Sort((x, y) => x.Value.CompareTo(y.Value));

        // Get sum of weights
        foreach (var pair in choices)
        {
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

    void OnDeath()
    {
        StopAllCoroutines();

        SetExitDoorHeight(exitDoorStartHeight);
        exitDoor.SetActive(true);

        Debug.Log("Death throes");

        StartCoroutine(KillCoroutine());
    }

    void SetExitDoorHeight(float y)
    {
        if (exitDoor != null)
        {

            Vector3 position = exitDoor.transform.position;

            exitDoor.transform.position = new Vector3(position.x, y, position.z);
        }
    }

    void GetTarget()
    {
        // In case there are >1 objects tagged with player, we want to select them randomly
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        if (targets.Length > 0)
        {
            int i = Random.Range(0, targets.Length);

            target = targets[i];
        }
    }

    void StartAction(IEnumerator routine)
    {
        if (inAction)
        {
            return;
        }

        inAction = true;
        actionStartTime = Time.time;

        StartCoroutine(routine);
    }

    void EndAction()
    {
        inAction = false;
        state = BossState.Decide;

        StopAllCoroutines();
    }
    /*
    * Safety checks to ensure the show goes on in the case of exceptional circumstances
    */
    void CheckSafety()
    {
        // If not performing action, or in a deciding state, or action has timed out, then end all actions
        if (!inAction || state == BossState.Decide || Time.time - actionStartTime > ACTION_TIMEOUT)
        {
            EndAction();
        }
    }
    /*
     * Generic way for triangle boss to fire projectile
     */
    void FireProjectile(Vector3 position, Vector3 velocity)
    {
        GameObject projectileInstance = Instantiate(projectilePrefab);
        Projectile projectile = projectileInstance.GetComponent<Projectile>();
        ContactDamagePlayer contactDamage = projectileInstance.GetComponent<ContactDamagePlayer>(); ;

        contactDamage.damageAmount = projectileDamage;

        projectile.position = position;
        projectile.velocity = velocity;
        projectile.lifeTime = projectileLifetime;
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

        for (float alpha = 0.0f; alpha < movementDelay; alpha += Time.fixedDeltaTime / movementDuration)
        {
            if (!attack2Sound.isPlaying)
            {
                attack2Sound.Play();
            }

            transform.position = Vector3.Lerp(startPosition, nextPosition, alpha);
            transform.LookAt(target.transform.position);

            yield return new WaitForFixedUpdate();
        }

        transform.position = nextPosition;
        transform.LookAt(target.transform.position);

        //yield return new WaitForSeconds(1.0f);

        EndAction();
    }
    /*
     * fire projectiles in all directions
     */
    IEnumerator CardinalProjectiles()
    {
        float[] angles =
        {
            0.0f, 90.0f, 180.0f, 270.0f
        };

        Vector3 currentRotation;
        Vector3 dir;

        while (true)
        {
            currentRotation = transform.rotation.eulerAngles;

            if (!attack1Sound.isPlaying)
            {
                attack1Sound.Play();
            }

            foreach (float angle in angles)
            {
                dir = Quaternion.Euler(currentRotation.x, currentRotation.y + angle, currentRotation.z) * Vector3.forward;

                FireProjectile(transform.position + (dir * 0.5f), dir * projectileSpeed);
            }

            yield return new WaitForSeconds(projectileDelay);
        }
    }
    /*
     * fire projectiles in toward player
     */
    IEnumerator ForwardProjectiles()
    {
        Vector3 dir;
        float speed;

        while (true)
        {
            if (!attack2Sound.isPlaying)
            {
                attack2Sound.Play();
            }

            dir = target.transform.position - transform.position;
            speed = Mathf.Max(projectileSpeed, dir.magnitude);

            FireProjectile(transform.position, dir.normalized * speed);

            yield return new WaitForSeconds(projectileDelay);
        }
    }
    /*
     * Move around
     */
    IEnumerator InterpolateTo(Vector3 endPosition, float time)
    {
        Vector3 startPosition = transform.position;

        for (float i = 0.0f; i < 1.0f; i += Time.fixedDeltaTime / time)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, i);

            yield return new WaitForFixedUpdate();
        }

        transform.position = endPosition;
    }
    /*
     * Fire in all directions
     */
    IEnumerator Attack1()
    {
        if (!target)
        {
            yield return null;
        }

        transform.LookAt(target.transform);

        Vector3 startPosition = transform.position;
        Vector3 startRotation = transform.rotation.eulerAngles;

        // Ease into start position

        yield return InterpolateTo(startPosition + new Vector3(0.0f, 2.0f, 0.0f), 0.5f);

        // Attack animation and projectiles

        Vector3 pos;

        float pitch;
        Vector3 rot;

        Coroutine projectileLoop = null;

        for (float i = 0.0f; i < 1.0f; i += Time.fixedDeltaTime / attack1Duration)
        {
            if (!attack1Sound.isPlaying)
            {
                attack1Sound.Play();
            }

            pos = startPosition + new Vector3(0.0f, Mathf.Cos(i * Mathf.PI * attack1Waves) * 2.0f, 0.0f);

            pitch = Quaternion.LookRotation(target.transform.position - pos).eulerAngles.x;
            pitch -= startRotation.x;
            rot = startRotation + new Vector3(pitch, i * 360.0f * attack1Rotations, 0.0f);

            // Starts projectiles after the first wave
            if (projectileLoop == null && i > 1.0f / attack1Waves)
            {
                projectileLoop = StartCoroutine(CardinalProjectiles());
            }

            transform.position = pos;
            transform.LookAt(target.transform);
            transform.rotation = Quaternion.Euler(rot);

            yield return new WaitForFixedUpdate();
        }

        if (projectileLoop != null)
        {
            StopCoroutine(projectileLoop);
        }

        // Ease back to start position

        yield return InterpolateTo(startPosition, 0.5f);

        // Brief pause before changing state

        yield return new WaitForSeconds(0.5f);

        EndAction();
    }
    /*
     * fire straight at player
     */
    IEnumerator Attack2()
    {
        if (!target)
        {
            yield return null;
        }


        Vector3 targetPosition = target.transform.position;

        yield return InterpolateTo(targetPosition + Vector3.forward * attack2Distance, 1.0f);

        Coroutine projectileLoop = StartCoroutine(ForwardProjectiles());

        for (float i = 0.0f; i < 1.0f; i += Time.fixedDeltaTime / attack2Duration)
        {
            if (!attack2Sound.isPlaying)
            {
                attack2Sound.Play();
            }

            Quaternion offsetRotation = Quaternion.Euler(0.0f, i * 360.0f * attack2Rotations, 0.0f);
            Vector3 offset = offsetRotation * Vector3.forward * attack2Distance;

            transform.position = targetPosition + offset;

            transform.LookAt(targetPosition);

            yield return new WaitForFixedUpdate();
        }

        if (projectileLoop != null)
        {
            StopCoroutine(projectileLoop);
        }

        EndAction();
    }

    IEnumerator Attack3()
    {
        if (!target)
        {
            yield return null;
        }

        Debug.Log("Attack 3 or something");

        EndAction();
    }

    // TODO: Coroutine wrapper, the kill function really should be a coroutine by default
    IEnumerator KillCoroutine()
    {
        // Remove active hitboxes

        foreach (var o in FindObjectsOfType<GameObject>())
        {
            if (o.GetComponent<ContactDamagePlayer>() != null)
            {
                Destroy(o);
            }
        }

        // Rotate boss to face random angle

        transform.rotation = Quaternion.Euler(
            Random.Range(0, 360),
            Random.Range(0, 360),
            Random.Range(0, 360)
        );

        // Play shaking and sinking animation

        Vector3 startPosition = transform.position;

        Vector3 v;
        float h;

        float exitDoorHeight;

        for (float i = 0.0f; i < 1.0f; i += Time.fixedDeltaTime / DEATH_ANIMATION_TIME)
        {
            // Linear interpolation
            h = (1 - i) * startPosition.y + i * DEATH_ANIMATION_TARGET_HEIGHT;
            v = new Vector3(startPosition.x, h, startPosition.z);

            transform.position = v + (Random.insideUnitSphere * DEATH_ANIMATION_INTENSITY);

            exitDoorHeight = (1 - i) * exitDoorStartHeight + i * exitDoorEndHeight;

            SetExitDoorHeight(exitDoorHeight);

            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
    }
}
