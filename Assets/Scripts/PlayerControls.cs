using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject normalBulletPrefab;
    // Start is called before the first frame update
    float fixedFramesPerSecond = 50;
    float bulletShootCooldown = 0;
    float maxBulletCooldown = 1.5f;
    void Start()
    {
        maxBulletCooldown *= fixedFramesPerSecond;
    }

    void playerMovement()
    {
        if (Input.GetKey(KeyCode.Space) && bulletShootCooldown <= 0)
        {
            var bullet = Instantiate(normalBulletPrefab, gameObject.transform.position, Quaternion.identity);
            bulletShootCooldown = maxBulletCooldown;
            Destroy(bullet.gameObject, 5);



        }

        if (Input.GetKey(KeyCode.W))
        {
            gameObject.transform.position -= new Vector3(0, 0, 3) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.position += new Vector3(0, 0, 3) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.position += new Vector3(3, 0, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.position -= new Vector3(3, 0, 0) * Time.deltaTime;
        }

    }

    private void FixedUpdate()
    {


        cooldowns();
    }

    void cooldowns()
    {
        bulletShootCooldown = Mathf.Max(0, bulletShootCooldown - 1);

    }

    void fireBullet()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerMovement();


        //if (bulletShootCooldown)
        //    bulletShootCooldown
    }
}
