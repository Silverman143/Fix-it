using Firebase.Analytics;

public static class Analytics
{
    public static void LevelStart(int levelNumber)
    {
        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventLevelStart,
            new Parameter(FirebaseAnalytics.ParameterLevel, levelNumber)
        );
    }

    public static void LevelComplete(int levelNumber)
    {
        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventLevelEnd,
            new Parameter(FirebaseAnalytics.ParameterLevel, levelNumber),
            new Parameter("result", "complete")
        );
    }

    public static void LevelFailed(int levelNumber)
    {
        FirebaseAnalytics.LogEvent(
            FirebaseAnalytics.EventLevelEnd,
            new Parameter(FirebaseAnalytics.ParameterLevel, levelNumber),
            new Parameter("result", "failed")
        );
    }

    public static void GetExtraTime(int levelNumber)
    {
        FirebaseAnalytics.LogEvent(
            "extra_time",
            new Parameter(FirebaseAnalytics.ParameterLevel, levelNumber)
            );
    }

    public static void InterstitialShown(int id)
    {
        FirebaseAnalytics.LogEvent(
            "interstitial_shown",
            new Parameter("ad_id", id)
        );
    }

    public static void RewardVideoShown(int id)
    {
        FirebaseAnalytics.LogEvent(
            "reward_video_shown",
            new Parameter("ad_id", id)
        );
    }
}
