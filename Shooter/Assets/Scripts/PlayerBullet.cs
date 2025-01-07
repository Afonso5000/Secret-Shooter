using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision){

        Transform hitTransform = collision.transform;
        if(hitTransform.CompareTag("Enemy")){

            Debug.Log("Enemy Hit");
            //hitTransform.GetComponent<PlayerHealth>().TakeDamage(10);
        }
        Destroy(gameObject);
    }
}