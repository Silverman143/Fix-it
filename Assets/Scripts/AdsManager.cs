using UnityEngine;
using YG;


namespace FixItGame
{
    public class AdsManager : MonoBehaviour
    {
        public static AdsManager Instance { get; private set; }

        [SerializeField] private int _fromLevel;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }


        private void OnEnable()
        {
            GlobalEvents.OnLoadingNewLevel += ShowAd;
        }

        private void OnDisable()
        {
            GlobalEvents.OnLoadingNewLevel += ShowAd;
        }

        private void ShowAd(int level)
        {
            if (level < _fromLevel)
                return;

            YandexGame.FullscreenShow();
        }
    }
}
