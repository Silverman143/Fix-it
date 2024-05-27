using System;
using UnityEngine;
#if UNITY_WEBGL
using YG;
#else
using GoogleMobileAds.Api;
#endif

namespace FixItGame
{
    public class AdsManager : MonoBehaviour
    {
        //Events
        public static event Action OnRewardConfirmed;
        public static event Action OnRewardRejected;


        public static AdsManager Instance { get; private set; }

        // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
        private string _adUnitIdInterstitial = "ca-app-pub-4373382308292522/5831030034";
        // prod: ca-app-pub-4373382308292522/5831030034
        // test: ca-app-pub-3940256099942544/1033173712
        private string _adUnitIdRewardAdd = "ca-app-pub-4373382308292522/3806334632";
        //prod: ca-app-pub-4373382308292522/3806334632
        //test: ca-app-pub-3940256099942544/1033173712

#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
  private string _adUnitId = "unused";
#endif


#if !UNITY_WEBGL
        private InterstitialAd _interstitialAd;
        private RewardedAd _rewardedAd;
#endif

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

        private void Start()
        {
#if !UNITY_WEBGL
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                // This callback is called once the MobileAds SDK is initialized.
            });
            LoadInterstitialAd();
            LoadRewardedAd();
#endif

        }

        private void OnEnable()
        {
#if UNITY_WEBGL
            YandexGame.OpenVideoEvent += HandleGamePaused;
            YandexGame.CloseVideoEvent += HandleRewardConfirmed;
            YandexGame.ErrorVideoEvent += HandleRewardRejected;

            YandexGame.OpenFullAdEvent += HandleGamePaused;
            YandexGame.CloseFullAdEvent += HandleGameContinue;
            YandexGame.ErrorFullAdEvent += HandleGameContinue;
#endif
        }

        private void OnDisable()
        {
#if UNITY_WEBGL
            YandexGame.OpenVideoEvent -= HandleGamePaused;
            YandexGame.CloseVideoEvent -= HandleRewardConfirmed;
            YandexGame.ErrorVideoEvent -= HandleRewardRejected;

            YandexGame.OpenFullAdEvent -= HandleGamePaused;
            YandexGame.CloseFullAdEvent -= HandleGameContinue;
            YandexGame.ErrorFullAdEvent -= HandleGameContinue;
#endif
        }

        private void HandleRewardConfirmed()
        {
            OnRewardConfirmed.Invoke();
            HandleGameContinue();
        }

        private void HandleRewardRejected()
        {
            OnRewardRejected.Invoke();
            HandleGameContinue();
        }

        private void HandleGameContinue()
        {
            GlobalEvents.RaiseGameContinue();
        }
        private void HandleGamePaused()
        {
            GlobalEvents.RaiseGamePaused();
        }

        public void ShowInterstitial(int level, Action action)
        { 
            if (level < _fromLevel)
            {
                action();
                return;
            }
#if UNITY_WEBGL
            YandexGame.FullscreenShow();
            action();
#else
            ShowInterstitialAd(action);
#endif
        }

        public void ShowRewardAd()
        {
#if UNITY_WEBGL
            YandexGame.RewVideoShow(1);
#else
            ShowRewardedAd();
#endif
        }

        public bool IsRewardAddReady()
        {
#if UNITY_WEBGL
            return true;
#else
            return _rewardedAd.CanShowAd();
#endif
        }

#if !UNITY_WEBGL

        //_________________________________________Interstitial Add_________________________________________________
        public void LoadInterstitialAd()
        {
            // Clean up the old ad before loading a new one.
            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }

            Debug.Log("Loading the interstitial ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            InterstitialAd.Load(_adUnitIdInterstitial, adRequest,
                (InterstitialAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("interstitial ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Interstitial ad loaded with response : "
                              + ad.GetResponseInfo());

                    _interstitialAd = ad;
                });
        }

        private void RegisterEventHandlers(InterstitialAd interstitialAd, Action action = null)
        {
            interstitialAd.OnAdFullScreenContentOpened += () =>
            {
                GlobalEvents.RaiseGamePaused();
                Debug.Log("____Add Shown____");
            };



            // Raised when the ad closed full screen content.
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("____Add Closed____");
                GlobalEvents.RaiseGameContinue();
                action();
                // Reload the ad so that we can show another as soon as possible.
                LoadInterstitialAd();
            };
            // Raised when the ad failed to open full screen content.
            interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Interstitial ad failed to open full screen content " +
                               "with error : " + error);
                GlobalEvents.RaiseGameContinue();
                action();
                // Reload the ad so that we can show another as soon as possible.
                LoadInterstitialAd();
            };
        }

        public void ShowInterstitialAd(Action action)
        {
            if (_interstitialAd != null && _interstitialAd.CanShowAd())
            {
                Debug.Log("Showing interstitial ad.");
                RegisterEventHandlers(_interstitialAd, action);
                _interstitialAd.Show();
            }
            else
            {
                LoadInterstitialAd();
            }
        }

        //_________________________________________Reward Add_________________________________________________

        private void LoadRewardedAd()
        {
            // Clean up the old ad before loading a new one.
            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            RewardedAd.Load(_adUnitIdRewardAdd, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());

                    _rewardedAd = ad;
                });
        }

        private void RegisterEventHandlersRewardAd(RewardedAd ad)
        {
            ad.OnAdFullScreenContentOpened += () =>
            {
                GlobalEvents.RaiseGamePaused();
            };
            // Raised when the ad closed full screen content.
            ad.OnAdFullScreenContentClosed += () =>
            {
                GlobalEvents.RaiseGameContinue();
                LoadRewardedAd();
            };
            // Raised when the ad failed to open full screen content.
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                OnRewardRejected?.Invoke();
                GlobalEvents.RaiseGameContinue();
                LoadRewardedAd();
            };
        }

        private void ShowRewardedAd()
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                RegisterEventHandlersRewardAd(_rewardedAd);
                _rewardedAd.Show((Reward reward) =>
                {
                    OnRewardConfirmed?.Invoke();
                });
            }
            else
            {
                OnRewardRejected?.Invoke();
            }
        }

#endif
    }
}
