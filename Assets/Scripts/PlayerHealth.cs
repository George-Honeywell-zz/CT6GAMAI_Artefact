using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public float startHealth = 100.0f;
    public float currentHealth;
    public Slider healthSlider;

    void Awake()
    {
        currentHealth = startHealth;
    }

    void Update()
    {
        healthSlider.value = currentHealth;
        if (Input.GetKey(KeyCode.A))
        {
            currentHealth -= 10;
        }

        //if (Input.GetKey(KeyCode.S))
        //{
        //    currentHealth += 10;
        //}
        Die();
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Enemy")
        {
            currentHealth -= 0.1f;
        }
    }

    void Die()
    {
        if(currentHealth <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
