using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Vector3 position = Vector3.zero;

    public float timer = 0f;
    public float growTime = 4.0f;

    public float maxSize = 6.0f;

    public bool isMaxSize = false;
    public bool isShrinking = false;

    public float damage = 0;

    public int enemyLayerNum;
    //bool wasInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!isMaxSize)
        {
            StartCoroutine(Grow());
        }

        Destroy(gameObject, growTime * 2);
    }

    private IEnumerator Grow()
    {
        Vector3 startScale = transform.localScale;
        Vector3 maxScale = new Vector3(maxSize, maxSize, maxSize);
        do
        {
            transform.localScale = Vector4.Lerp(startScale, maxScale, timer / growTime);
            timer += Time.deltaTime;
            yield return null;
        }
        while (timer < growTime);

        isMaxSize = true;
    }
    private IEnumerator Shrink()
    {
        isShrinking = true;
        isMaxSize = false;
        Vector3 startScale = transform.localScale;
        Vector3 minScale = new Vector3(0, 0, 0);
        timer = 0;
        do
        {
            transform.localScale = Vector4.Lerp(startScale, minScale, timer / growTime);
            timer += Time.deltaTime;
            yield return null;
        }
        while (timer < growTime);
    }

    public void Initialize(int enemyLayerNum, float damage = 20)
    {
        //wasInitialized = true;
        this.enemyLayerNum = enemyLayerNum;
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Triggered");

        // 9 is the layer id for enemy
        if (other.gameObject.layer == enemyLayerNum)
        {
            AliveObject enemy = other.gameObject.GetComponent<AliveObject>();

            enemy.Damage(damage);

            Destroy(gameObject);
        }

        if (other.transform.name.Contains("cubeAttack") || other.transform.name.Contains("TriangleBossAttack"))
        {
            Destroy(other.gameObject);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = position;
        if (isMaxSize)
        {
            StartCoroutine(Shrink());
        }
    }
}
