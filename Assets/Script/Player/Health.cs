using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 3f;
    
    protected void _Health()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
