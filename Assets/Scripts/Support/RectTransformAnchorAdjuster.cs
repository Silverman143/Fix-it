using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformAnchorAdjuster : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private float _portraitYAnchor = 0.5f; // Значение для портретного режима
    [SerializeField] private float _landscapeYAnchor = 0.5f; // Значение для альбомного режима

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
            SetAnchorY(_landscapeYAnchor);
        }
        else
        {
            SetAnchorY(_portraitYAnchor);
        }
    }

    private void SetAnchorY(float yAnchorPos)
    {
        _rectTransform.anchoredPosition = new Vector2 (_rectTransform.anchoredPosition.x, yAnchorPos);
    }
}
