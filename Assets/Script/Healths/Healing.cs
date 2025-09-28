using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    [SerializeField] private int healAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.health = Mathf.Min(playerHealth.health + healAmount, playerHealth.maxHealth);
                Destroy(gameObject); // Destroy the healing item after use
            }
        }
    }
}
