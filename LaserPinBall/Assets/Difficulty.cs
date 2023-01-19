using UnityEngine;

public static class Difficulty
{
    static float secondsToMaxDifficulty = 300;

    public static float GetDifficultyPercent() {
        if (!CameraBasedPosition.Instance.isGameStarted) return 0;
        return Mathf.Clamp01(Time.timeSinceLevelLoad / secondsToMaxDifficulty);
    }
}