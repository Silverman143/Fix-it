#if !UNITY_WEBGL
using Firebase.Analytics;
#endif
public static class Analytics
{
    public static void LevelStart(int levelNumber)
    {
#if !UNITY_WEBGL
        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventLevelStart,
            new Parameter(FirebaseAnalytics.ParameterLevel, levelNumber)
        );
#endif
    }

    public static void LevelComplete(int levelNumber)
    {
#if !UNITY_WEBGL
        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventLevelEnd,
            new Parameter(FirebaseAnalytics.ParameterLevel, levelNumber),
            new Parameter("result", "complete")
        );
#endif
    }

    public static void LevelFailed(int levelNumber)
    {
#if !UNITY_WEBGL
        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventLevelEnd,
            new Parameter(FirebaseAnalytics.ParameterLevel, levelNumber),
            new Parameter("result", "failed")
        );
#endif
    }

    public static void GetExtraTime(int levelNumber)
    {
#if !UNITY_WEBGL
        FirebaseAnalytics.LogEvent(
            "extra_time",
            new Parameter(FirebaseAnalytics.ParameterLevel, levelNumber)
            );
#endif
    }

    public static void InterstitialShown(int id)
    {
#if !UNITY_WEBGL
        FirebaseAnalytics.LogEvent(
            "interstitial_shown",
            new Parameter("ad_id", id)
        );
#endif
    }

    public static void RewardVideoShown(int id)
    {
#if !UNITY_WEBGL
        FirebaseAnalytics.LogEvent(
            "reward_video_shown",
            new Parameter("ad_id", id)
        );
#endif
    }
}
