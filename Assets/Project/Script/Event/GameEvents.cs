using System;

public static class GameEvents
{
    public static event Action<Health> OnPlayerDied;

    public static event Action OnPlayerWin;

    public static event Action OnScoreChanged;

    public static void PlayerDied(Health health)
    {
        OnPlayerDied?.Invoke(health);
    }

    public static void PlayerIsWin()
    {
        OnPlayerWin?.Invoke();
    }

    public static void ScoreChanged()
    {
        OnScoreChanged?.Invoke();
    }
}