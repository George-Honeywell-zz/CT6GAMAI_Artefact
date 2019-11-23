using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damageAmount = 5;
    public GameObject Player;
    PlayerHealth pHealth;
    bool inAttackRange;

    void OnCollisionEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Colliding!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            inAttackRange = false;
        }
    }

    void Update()
    {
        
    }
}
