using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhantomController : MonoBehaviour
{
    [SerializeField] private float _possibleOffset = 0.1f;

    private SpriteRenderer _spriteRenderer;
    private PuzzleType _type;
    private List<PuzzleController> _viewingObjects = new List<PuzzleController>();
    private PuzzleController _activeObject = null;
    private bool _isFilled = false;

    private void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Sprite sprite, PuzzleType type, Vector2 position) {
        _spriteRenderer.sprite = sprite;
        _type = type;
        transform.position = position;
    }

    private void FixedUpdate()
    {
        CheckFilled();
    }

    private void CheckFilled()
    {
        bool found = false;
        float squaredOffset = _possibleOffset * _possibleOffset;
        foreach (PuzzleController obj in _viewingObjects)
        {
            if ((transform.position - obj.transform.position).sqrMagnitude < squaredOffset)
            {
                found = true;
                _activeObject = obj;
                break;
            }
        }

        if (found && !_isFilled)
        {
            _isFilled = true;
            _activeObject.AddActivePart();
            GlobalEvents.OnPhantomFilledInvoke();
        }
        if (!found && _isFilled)
        {
            _isFilled = false;
            _activeObject.RemovePart();
            GlobalEvents.OnPhantomEmptyInvoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PuzzleController>(out PuzzleController puzzleObject))
        {
            if (puzzleObject.Type == _type)
            {
                _viewingObjects.Add(puzzleObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PuzzleController>(out PuzzleController puzzleObject))
        {
            if (puzzleObject.Type == _type)
            {
                _viewingObjects.Remove(puzzleObject);
            }
        }
    }
}
