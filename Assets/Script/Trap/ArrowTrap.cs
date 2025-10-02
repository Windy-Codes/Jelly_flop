using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [Header("Arrow Settings")]
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float fireInterval = 2f;
    public bool autoFire = true;

    private float fireTimer;

    void Update()
    {
        if (!autoFire) return;

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireInterval)
        {
            FireArrow();
            fireTimer = 0f;
        }
    }

    public void FireArrow()
    {
        if (arrowPrefab == null || firePoint == null) return;

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);

        // Use firePoint's up vector for 2D shooting
        Vector2 shootDirection = firePoint.up; // ← this ensures velocity follows rotation

        arrow.GetComponent<Arrow>().Launch(shootDirection);
    }
}
