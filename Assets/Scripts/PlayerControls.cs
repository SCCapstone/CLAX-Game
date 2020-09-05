﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject normalBulletPrefab;
    // Start is called before the first frame update
    float bulletShootCooldown = 0;
    float maxBulletCooldown = 2;
    void Start()
    {

    }

    void playerMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space) && bulletShootCooldown == 0)
        {
            Instantiate(normalBulletPrefab, gameObject.transform.position, Quaternion.identity);
            bulletShootCooldown = maxBulletCooldown;

        }

        if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.position -= new Vector3(0, 0, 3) * Time.deltaTime;

        }
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
