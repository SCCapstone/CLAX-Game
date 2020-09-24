using UnityEngine;
using UnityEngine.InputSystem;

public class FallingPlatform : MonoBehaviour
{
    [Header("Falling")]
    public bool collisionFall = true;

    public float timingFall = 600;
    bool isFalling = false;
    private float downSpeed = 0;


    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            isFalling = true;
    }


    //private void OnTriggerStay(Collider collision)
    //{

    //}
    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            downSpeed += Time.deltaTime;
            transform.parent.position = new Vector3(transform.position.x,
                transform.position.y - downSpeed,
                transform.position.z);
        }
        //timingFall -= Time.deltaTime;
    }
}
