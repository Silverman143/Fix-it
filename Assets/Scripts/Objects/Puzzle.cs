using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FixItGame
{
    [CreateAssetMenu(fileName = "New Puzzle", menuName = "Puzzle")]
    public class Puzzle : ScriptableObject
    {
        public PuzzleType type;
        public Sprite puzzleSprite;
        public Sprite phantomSprite;
        public GameObject prefab;
    }
}
