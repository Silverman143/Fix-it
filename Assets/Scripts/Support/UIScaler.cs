using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaler : MonoBehaviour
{
    [SerializeField] private float _mainHeight;
    [SerializeField] private RectTransform _mainCanvasRect;
    [SerializeField] private float _offset;
    private RectTransform _rectTransform;

    private void OnEnable()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float targetHeight = _mainCanvasRect.sizeDelta.y - _offset;
        float currentHeight = _mainHeight * _rectTransform.localScale.x;

        if (!Mathf.Approximately(currentHeight, targetHeight))
        {
            float scale = targetHeight / _mainHeight;
            scale = Mathf.Clamp(scale, 0, 1);

            _rectTransform.localScale = new Vector3(scale, scale, 1);
        }
    }
}
