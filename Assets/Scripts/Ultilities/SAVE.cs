using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;

public static class SAVE
{
    public const string LEVEL_KEY = "K_Level";
    public const string AUDIO_KEY = "K_Audio";
    public const string HIGH_SCORE_KEY = "K_HighScore";
    public const string CHARACTER_UNLOCK_KEY = "K_CUnlock";
    public const string CHARACTER_SELECTED_ID = "K_CSelectedId";

    #region Levle
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
    #endregion

    #region Audio
    public static void SaveAudio(float volume)
    {
        PlayerPrefs.SetFloat(AUDIO_KEY, volume);
    }
    public static float GetCurrentVolume()
    {
        return PlayerPrefs.GetFloat(AUDIO_KEY, 0.5f);
    }

    #endregion

    #region Score
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
    #endregion

    #region Character Skin
    public static void SaveUnlockCharacter(List<int> ids)
    {
        string json = JsonConvert.SerializeObject(ids);
        PlayerPrefs.SetString(CHARACTER_UNLOCK_KEY, json);
    }
    public static List<int> LoadUnlockCharacterIds()
    {
        List<int> ids = null;
        string json = PlayerPrefs.GetString(CHARACTER_UNLOCK_KEY, null);
        if(json != null)
        {
            ids = JsonConvert.DeserializeObject<List<int>>(json);
        }
        return ids;

    }
    public static void SaveSelectedCharacterId(int id)
    {
        PlayerPrefs.SetInt(CHARACTER_SELECTED_ID, id);
    }
    public static int GetCharacterSelectedId()
    {
        return PlayerPrefs.GetInt(CHARACTER_SELECTED_ID, -1);
    }
    #endregion
}
