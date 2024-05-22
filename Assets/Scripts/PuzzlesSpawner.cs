using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace FixItGame
{
    public class PuzzlesSpawner : MonoBehaviour
    {
        [Range(0.1f, 1f)]
        [SerializeField] private float _speed;
        [SerializeField] private List<PuzzleType> _puzzles;
        [SerializeField] private Puzzle[] _puzzlesDatas;
        private Vector2 _puzzlesScale = new Vector2(1, 1);


        public void Init(List<PuzzleType> puzzles, Vector2 scale)
        {
            _puzzles = puzzles;
            _puzzlesScale = scale;
            StartCoroutine(SpawnNewPuzzle());
        }

        public void AddNewPuzzle(PuzzleType type, Vector2 scale)
        {
            Puzzle puzzle = _puzzlesDatas.FirstOrDefault(p => p.type == type);
            Vector2 newPos = transform.position;
            GameObject newPuzzleObj = Instantiate(puzzle.prefab, null);
            PuzzleController newPuzzle = newPuzzleObj.GetComponent<PuzzleController>();
            newPuzzle.Init(sprite: puzzle.puzzleSprite, type: type, position: newPos, scale:scale);
            newPuzzle.OnPuzzleActivate.AddListener(HandleActivatonEvent);
            GlobalEvents.RaiseCreateNewPuzzle(newPuzzle);
        }

        private void HandleActivatonEvent()
        {
            StartCoroutine(SpawnNewPuzzle());
        }

        IEnumerator SpawnNewPuzzle()
        {
            if (_puzzles.Count > 0)
            {
                yield return new WaitForSeconds(_speed);

                PuzzleType newType = _puzzles[Random.Range(0, _puzzles.Count - 1)];
                _puzzles.Remove(newType);

                AddNewPuzzle(newType, _puzzlesScale);
            }
        }
    }
}
