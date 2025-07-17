using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class SAVE
{
    public const string LEVEL_UNLOCK_KEY = "K_Level_Unlock";
    public const string MUSIC_KEY = "K_Music";
    public const string SOUND_KEY = "K_Sound";
    public const string HIGH_SCORE_KEY = "K_HighScore";
    public const string CHARACTER_UNLOCK_KEY = "K_CUnlock";
    public const string CHARACTER_SELECTED_ID = "K_CSelectedId";
    public const string COINS = "K_Coins";
    public const string HEARTS = "K_Hearts";
    public const string LAST_TIME_ADD_HEART = "K_LastTimeAddHeart";
    public const string DAILY_QUEST_KEY = "K_DailyQuest";
    public const string ACHIEVEMENT_QUEST_KEY = "K_AchievementQuest";
    public const string DATE_QUEST = "K_DateQuest";
    public const string ITEM_KEY = "K_Item";


    #region Level
    public static void SaveLevel(string level)
    {
        if (int.TryParse(level, out var levelValue))
        {
            int currenLevel = PlayerPrefs.GetInt(LEVEL_UNLOCK_KEY, 1);
            if (levelValue > currenLevel)
            {

                PlayerPrefs.SetInt(LEVEL_UNLOCK_KEY, levelValue);
                Observer.Instance.Broadcast(EventId.OnUnlockNewLevel, currenLevel);
            }
        }
    }
    public static int GetUnlockLevel()
    {
        return PlayerPrefs.GetInt(LEVEL_UNLOCK_KEY, 1);
    }
    #endregion

    #region Audio
    public static void SaveMusic(float volume)
    {
        PlayerPrefs.SetFloat(MUSIC_KEY, volume);
    }
    public static float GetCurrentMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_KEY, -1);
    }

    public static void SaveSound(float volume)
    {
        PlayerPrefs.SetFloat(SOUND_KEY, volume);
    }
    public static float GetCurrentSoundVolume()
    {
        return PlayerPrefs.GetFloat(SOUND_KEY, -1);
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
        if (json != null)
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

    #region Currency
    public static void SaveCoins(int coins)
    {
        PlayerPrefs.SetInt(COINS, coins);
    }
    public static int GetCoins()
    {
        return PlayerPrefs.GetInt(COINS, 0);
    }
    public static void SaveHearts(int hearts)
    {
        PlayerPrefs.SetInt(HEARTS, hearts);
    }
    public static int GetHearts(int heartDeafault)
    {
        return PlayerPrefs.GetInt(HEARTS, heartDeafault);
    }
    public static void SaveLastTimeAddHeart(string time)
    {
        PlayerPrefs.SetString(LAST_TIME_ADD_HEART, time);
    }
    public static string GetLastTimeAddHeart()
    {
        return PlayerPrefs.GetString(LAST_TIME_ADD_HEART, "");
    }
    #endregion


    #region Quest
    public static void SaveDailyQuest(List<QuestSave> dailyQuestSave)
    {
        string json = JsonConvert.SerializeObject(dailyQuestSave);
        PlayerPrefs.SetString(DAILY_QUEST_KEY, json);
    }
    public static List<QuestSave> LoadDailyQuest()
    {
        List<QuestSave> dailyQuestSave = new();
        string json = PlayerPrefs.GetString(DAILY_QUEST_KEY, null);
        if (json != null)
        {
            dailyQuestSave = JsonConvert.DeserializeObject<List<QuestSave>>(json);
        }
        return dailyQuestSave;
    }
    public static void SaveAchievementQuest(List<QuestSave> dailyQuestSave)
    {
        string json = JsonConvert.SerializeObject(dailyQuestSave);
        PlayerPrefs.SetString(ACHIEVEMENT_QUEST_KEY, json);
    }
    public static List<QuestSave> LoadAchievementQuest()
    {
        List<QuestSave> dailyQuestSave = new();
        string json = PlayerPrefs.GetString(ACHIEVEMENT_QUEST_KEY, null);
        if (json != null)
        {
            dailyQuestSave = JsonConvert.DeserializeObject<List<QuestSave>>(json);
        }
        return dailyQuestSave;
    }

    public static void SaveDateLastTime(string dateTimeLastStr)
    {
        PlayerPrefs.SetString(DATE_QUEST, dateTimeLastStr);
    }
    public static string GetDateLastTime()
    {
        return PlayerPrefs.GetString(DATE_QUEST, "");
    }

    #endregion

    #region Item
    public static void SaveItem(List<ItemSave> itemSaves)
    {
        string json = JsonConvert.SerializeObject(itemSaves);
        PlayerPrefs.SetString(ITEM_KEY, json);
    }

     public static List<ItemSave> LoadItem()
    {
        List<ItemSave> items = new();
        string json = PlayerPrefs.GetString(ITEM_KEY, null);
        if(json != null)
        {
           items = JsonConvert.DeserializeObject<List<ItemSave>>(json);
        }
        
        return items;
    }

    #endregion
}
