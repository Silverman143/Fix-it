using UnityEngine;

namespace FixItGame
{
    public class PuzzlePart : PuzzleController
    {
        [SerializeField] private PuzzleController _parrent;

        private void Awake()
        {
            _parrent = transform.parent.GetComponent<PuzzleController>();
        }

        public override void AddActivePart()
        {
            _parrent.AddActivePart();
        }

        public override void RemovePart()
        {
            _parrent.RemovePart();
        }
    }
}
