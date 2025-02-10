using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : MonoBehaviour
{
    public float damage;
    public float lifeTime = 3;
    public float raycastDistance = 10f; // Adjust distance based on bullet speed

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
            Destroy(gameObject);

        // Perform a Raycast on each frame to detect collisions
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            if (hit.collider.GetComponent<EnemyAi>() != null)
            {
                hit.collider.GetComponent<EnemyAi>().TakeDamage(damage);
                Destroy(gameObject); // Destroy bullet when it hits
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WhatIsWall"))
        {
            Destroy(gameObject); // Destroy bullet on impact with walls
        }
        else if (other.GetComponent<EnemyAi>() != null)
        {
            other.GetComponent<EnemyAi>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.GetComponent<PlayerHealth>() != null)
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}