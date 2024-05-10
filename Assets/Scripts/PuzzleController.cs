using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private PuzzleType _type;
    [SerializeField] private GameObject _outline;
    [SerializeField] private int _partsCount;
    private int _activeParts = 0;
    private Sprite _sprite;

    public PuzzleType Type => _type;

    private void Awake()
    {
        _sprite = GetComponent<Sprite>();
        CountParts();
    }

    private void CountParts()
    {
        _partsCount = GetComponentsInChildren<PuzzlePart>().Length;
        if (_partsCount == 0)
        {
            _partsCount++;
        }
    }

    private void CheckOutline()
    {
        Debug.Log(string.Format("Parts = {0}, ActiveParts = {1}", _partsCount, _activeParts));
        if (_activeParts == _partsCount)
        {
            _outline.SetActive(true);
        }
        else
        {
            _outline.SetActive(false);
        }
    }

    public virtual void AddActivePart()
    {
        _activeParts++;
        CheckOutline();
    }
    public virtual void RemovePart()
    {
        _activeParts--;
        CheckOutline();
    }

    public void Init(Sprite sprite, PuzzleType type, Vector2 position)
    {
        _type = type;
        _sprite = sprite;
        transform.position = position;
    }
}
