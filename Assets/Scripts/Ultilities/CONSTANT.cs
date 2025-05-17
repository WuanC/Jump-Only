using UnityEngine;

public static class CONSTANT
{
    public const string LEVEL_KEY = "K_Level";
    public const string AUDIO_KEY = "K_Audio";
    public const string HIGH_SCORE_KEY = "K_HighScore";


    public static void SaveLevel(string level)
    {
        if (int.TryParse(level, out var levelValue))
        {
            int currenLevel = PlayerPrefs.GetInt(LEVEL_KEY, 1);
            if (levelValue > currenLevel)
            {
                PlayerPrefs.SetInt(LEVEL_KEY, levelValue);
            }
        }
    }
    public static int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt(LEVEL_KEY, 1);
    }

    public static void SaveAudio(float volume)
    {
        PlayerPrefs.SetFloat(AUDIO_KEY, volume);
    }
    public static float GetCurrentVolume()
    {
        return PlayerPrefs.GetFloat(AUDIO_KEY, 0.5f);
    }


    public static void SaveHighScore(float highScore)
    {
        if (highScore > GetHighScore())
        {
            PlayerPrefs.SetFloat(HIGH_SCORE_KEY, highScore);
        }
    }
    public static float GetHighScore()
    {
        return PlayerPrefs.GetFloat(HIGH_SCORE_KEY);
    }

}
