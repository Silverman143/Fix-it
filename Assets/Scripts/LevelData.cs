using System;
using System.Collections.Generic;
using UnityEngine;

namespace FixItGame
{
    [Serializable]
    public class LevelData
    {
        public List<PhantomData> phantoms = new List<PhantomData>();
        public List<PuzzleType> puzzles = new List<PuzzleType>();
        public Vector2 puzzlesScale;
    }
}
