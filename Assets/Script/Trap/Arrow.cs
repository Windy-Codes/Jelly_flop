using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Launch(Vector2 direction)
    {
        rb.velocity = direction.normalized * speed;

        // Rotate arrow sprite to face movement
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
