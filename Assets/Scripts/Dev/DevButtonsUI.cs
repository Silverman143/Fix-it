using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FixItGame
{
    public class DevButtonsUI : MonoBehaviour
    {
        [SerializeField] private Slider _scalerSlider;

        private PuzzlesSpawner _spawner;

        private void Awake()
        {
            _spawner = FindObjectOfType<PuzzlesSpawner>();
            _scalerSlider.onValueChanged.AddListener(GameManager.Instance.SetPuzzlesScale);
        }

        public void CreateBox()
        {
            _spawner.AddNewPuzzle(PuzzleType.Box, GameManager.Instance.PuzzlesScale);
        }

        public void CreateComposit0()
        {
            _spawner.AddNewPuzzle(PuzzleType.Compozite0, GameManager.Instance.PuzzlesScale);
        }
        public void CreateComposit1()
        {
            _spawner.AddNewPuzzle(PuzzleType.Compozite1, GameManager.Instance.PuzzlesScale);
        }
        public void CreateComposit2()
        {
            _spawner.AddNewPuzzle(PuzzleType.Compozite2, GameManager.Instance.PuzzlesScale);
        }
        public void CreateComposit3()
        {
            _spawner.AddNewPuzzle(PuzzleType.Compozite3, GameManager.Instance.PuzzlesScale);
        }
        public void CreateComposit4()
        {
            _spawner.AddNewPuzzle(PuzzleType.Compozite4, GameManager.Instance.PuzzlesScale);
        }
        public void CreateComposit5()
        {
            _spawner.AddNewPuzzle(PuzzleType.Compozite5, GameManager.Instance.PuzzlesScale);
        }
    }
}
