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

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthSlider.value = currentHealth;
    }
}
