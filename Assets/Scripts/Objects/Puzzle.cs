using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Puzzle", menuName = "Puzzle")]
public class Puzzle : ScriptableObject
{
    public PuzzleType type;
    public Sprite puzzleSprite;
    public Sprite phantomSprite;
}
