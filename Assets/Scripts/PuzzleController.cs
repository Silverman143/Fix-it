using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FixItGame
{
    public class PuzzleController : MonoBehaviour
    {
        [SerializeField] private PuzzleType _type;
        [SerializeField] private GameObject _outline;
        [SerializeField] private int _partsCount;
        [SerializeField] private float _hitEffectFrequency = 0.5f;

        private float _hitEffectTimer = 0;

        private int _activeParts = 0;
        private Sprite _sprite;
        private Rigidbody2D _rigidBody;
        private bool _isActive = false;
        public UnityEvent OnPuzzleActivate;

        private LevelManager _levelManager;

        private List<PuzzleController> _contacts = new List<PuzzleController>();

        private void Freeze() => _rigidBody.constraints = RigidbodyConstraints2D.FreezePosition;

        public PuzzleType Type => _type;

        private void Awake()
        {
            _sprite = GetComponent<Sprite>();
            _rigidBody = GetComponent<Rigidbody2D>();
            CountParts();

            _levelManager = FindObjectOfType<LevelManager>();
        }

        private void OnEnable()
        {
            if (_levelManager) {

                _levelManager.OnLevelComplete.AddListener(Freeze);

            }
            
        }

        private void OnDisable()
        {
            if (_levelManager)
            {

                _levelManager.OnLevelComplete.RemoveListener(Freeze);

            }
        }

        private void Update()
        {
            if (_hitEffectTimer < _hitEffectFrequency)
            {
                _hitEffectTimer += Time.deltaTime;
            }
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

        public void Init(Sprite sprite, PuzzleType type, Vector2 position, Vector2 scale)
        {
            _type = type;
            _sprite = sprite;
            _rigidBody.constraints = RigidbodyConstraints2D.FreezePosition;
            _isActive = false;
            transform.position = position;
            transform.localScale = scale;

        }

        private void OnMouseDown()
        {
            if (!_isActive)
            {
                _rigidBody.constraints = RigidbodyConstraints2D.None;
                _isActive = true;
                OnPuzzleActivate.Invoke();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.TryGetComponent<PuzzleController>(out PuzzleController puzzle))
            {
                if (!_contacts.Contains(puzzle))
                {
                    GlobalEvents.RaisePuzzleHits();
                    _contacts.Add(puzzle);
                }
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.transform.TryGetComponent<PuzzleController>(out PuzzleController puzzle))
            {
                if (_contacts.Contains(puzzle))
                {
                    _contacts.Remove(puzzle);
                }
            }
        }
    }
}
