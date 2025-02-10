using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet Variables")]
    public float bulletSpeed;
    public float fireRate, bulletDamage;
    public bool isAuto;
 
    [Header("Initial Setup")]
    public Transform bulletSpawnTransform;
    public GameObject bulletPrefab;
 
    private float timer;
 
    private void Update()
    {
        if(timer > 0)
            timer -= Time.deltaTime / fireRate;
        
 
        if (isAuto)
        {
            if (Input.GetButton("Fire1") && timer <= 0)
            {
                Shoot();
            }
        }
        else
        {
            if(Input.GetButtonDown("Fire1") && timer <= 0)
            {
                Shoot();
            }
        }
    }
 
    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnTransform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("WorldObjectHolder").transform);
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnTransform.forward * bulletSpeed, ForceMode.Impulse);
        bullet.GetComponent<Bullet1>().damage = bulletDamage;
 
        timer = 1;
    }
}