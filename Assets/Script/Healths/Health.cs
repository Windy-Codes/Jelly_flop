using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 3f;
    public float maxHealth = 3f;
    public Playermovement movement;
    public GameObject panel;

    public void TakeDamage(float damage)
    {
        health -= damage; 

        if (health <= 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            movement.enabled = false;
            panel.SetActive(true);
        }
    }
}
