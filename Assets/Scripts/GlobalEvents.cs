

using System;

namespace FixItGame
{
    public class GlobalEvents
    {
        public static event Action OnPhantomFilled;
        public static event Action OnPhantomEmpty;
        public static event Action<PuzzleController> OnCreateNewPuzzle;
        public static event Action OnPuzzleHits;
        public static event Action<float> OnSoundVolumeChanged;
        public static event Action<float> OnMusicVolumeChanged;
        public static event Action OnLevelComplete;

        public static void RaisePhantomFilled()
        {
            OnPhantomFilled?.Invoke();
        }
        public static void RaisePhantomEmpty()
        {
            OnPhantomEmpty?.Invoke();
        }

        public static void RaiseCreateNewPuzzle(PuzzleController puzzle)
        {
            OnCreateNewPuzzle?.Invoke(puzzle);
        }

        public static void RaisePuzzleHits()
        {
            OnPuzzleHits?.Invoke();
        }

        public static void RaiseSoundValueChanged(float value)
        {
            OnSoundVolumeChanged?.Invoke(value);
        }

        public static void RaiseMusicValueChanged(float value)
        {
            OnMusicVolumeChanged?.Invoke(value);
        }

        public static void RaiseLevelComplete()
        {
            OnLevelComplete?.Invoke();
        }
    }
}
