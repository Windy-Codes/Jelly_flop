using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterimageFade : MonoBehaviour
{
    public float fadeSpeed = 5f;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Color c = sr.color;
        c.a -= fadeSpeed * Time.deltaTime;
        sr.color = c;

        if (c.a <= 0)
            Destroy(gameObject);
    }
}
