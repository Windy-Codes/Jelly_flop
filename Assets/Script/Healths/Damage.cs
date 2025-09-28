using UnityEngine;

public class Damage : MonoBehaviour
{
    public float damage = 0.5f;
    public float knockbackForceX = 10f;  // horizontal force
    public float knockbackForceY = 5f;   // vertical force
    public float knockbackDuration = 0.2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);

                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                Playermovement movement = collision.GetComponent<Playermovement>(); // your movement script

                if (rb != null)
                {
                    // figure out direction
                    float directionX = (collision.transform.position.x > transform.position.x) ? 1f : -1f;

                    // reset velocity so it's consistent
                    rb.velocity = Vector2.zero;

                    // apply knockback as velocity instead of force
                    rb.velocity = new Vector2(directionX * knockbackForceX, knockbackForceY);

                    // temporarily disable movement so knockback isn't overridden
                    if (movement != null)
                        movement.enabled = false;

                    // re-enable after short delay
                    collision.gameObject.GetComponent<MonoBehaviour>()
                        .StartCoroutine(ReenableMovement(movement));
                }
            }
        }
    }

    private System.Collections.IEnumerator ReenableMovement(Playermovement movement)
    {
        yield return new WaitForSeconds(knockbackDuration);
        if (movement != null)
            movement.enabled = true;
    }
}
