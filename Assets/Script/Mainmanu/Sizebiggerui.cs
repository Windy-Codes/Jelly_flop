using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sizebiggerui : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float SizeDefference = 1.05f;

    private Vector2 originalSize;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalSize = rectTransform.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.localScale = originalSize * SizeDefference;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.localScale = originalSize;
    }
}
