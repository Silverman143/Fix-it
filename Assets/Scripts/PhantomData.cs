using System;
using UnityEngine;

namespace FixItGame
{
    [Serializable]
    public class PhantomData
    {
        public PuzzleType type;
        public Vector2 position;
        public Quaternion rotation;
        public Vector2 scale;
    }
}
