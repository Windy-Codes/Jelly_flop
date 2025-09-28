using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 3f;
    public float maxHealth = 3f;

    public void TakeDamage(float damage)
    {
        health -= damage; 

        if (health <= 0)
        {
            Destroy(gameObject); 
        }
    }
}
