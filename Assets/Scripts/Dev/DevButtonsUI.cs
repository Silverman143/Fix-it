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
            _spawner.AddNewPuzzle(PuzzleType.Box, GetScale());
        }

        public void CreateComposit0()
        {
            _spawner.AddNewPuzzle(PuzzleType.Compozite0, GetScale());
        }
        public void CreateComposit1()
        {
            _spawner.AddNewPuzzle(PuzzleType.Compozite1, GetScale());
        }
        public void CreateComposit2()
        {
            _spawner.AddNewPuzzle(PuzzleType.Compozite2, GetScale());
        }
        public void CreateComposit3()
        {
            _spawner.AddNewPuzzle(PuzzleType.Compozite3, GetScale());
        }
        public void CreateComposit4()
        {
            _spawner.AddNewPuzzle(PuzzleType.Compozite4, GetScale());
        }
        public void CreateComposit5()
        {
            _spawner.AddNewPuzzle(PuzzleType.Compozite5, GetScale());
        }

        private Vector2 GetScale() => new Vector2(GameManager.Instance.PuzzlesScale, GameManager.Instance.PuzzlesScale);

        public void CleanData()
        {
            DBController.CleanData();
        }

        public void IncreaseLevel()
        {
            GameManager.Instance.LevelUp();
        }

        public void DecreaseLevel()
        {
            GameManager.Instance.LevelDown();
        }

    }
}
