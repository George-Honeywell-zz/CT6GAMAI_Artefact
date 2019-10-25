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

        if (Input.GetKey(KeyCode.S))
        {
            currentHealth += 10;
        }
    }
}
