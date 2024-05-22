using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaleFormats : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private float _portraitScale = 1.0f; // Значение для портретного режима
    [SerializeField] private float _landscapeScale = 1.0f; // Значение для альбомного режима

    private Vector2 lastScreenSize;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        AdjustAnchor();
        lastScreenSize = new Vector2(Screen.width, Screen.height);
    }

    private void Update()
    {
        if (Screen.width != lastScreenSize.x || Screen.height != lastScreenSize.y)
        {
            AdjustAnchor();
            lastScreenSize = new Vector2(Screen.width, Screen.height);
        }
    }

    private void AdjustAnchor()
    {
        if (Screen.width > Screen.height)
        {
            SetAnchorY(_landscapeScale);
        }
        else
        {
            SetAnchorY(_portraitScale);
        }
    }

    private void SetAnchorY(float scale)
    {
        _rectTransform.localScale = new Vector2(scale, scale);
    }
}
