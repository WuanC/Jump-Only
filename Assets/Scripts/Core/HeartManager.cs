using UnityEngine;
using System;
using System.Collections;
public class HeartManager : Singleton<HeartManager>
{
    public float timeToAddHeart = 300f;
    public int maxHearts = 5;
    public int currentHearts;
    DateTime lastTime;
    private void Start()
    {
        LoadHeart();
        StartCoroutine(UpdateCountdownDisplay());
       // Observer.Instance.Register(EventId.OnPlayerLoseInAdventure, UseHeart);
    }
    void LoadHeart()
    {
        currentHearts = SAVE.GetHearts(maxHearts);
        string lastTimeStr = SAVE.GetLastTimeAddHeart();
        if (string.IsNullOrEmpty(lastTimeStr))
        {
            string text_ = string.Format("Full");
            Observer.Instance.Broadcast(EventId.OnUpdateHearts, Tuple.Create(text_, currentHearts));
            lastTime = DateTime.UtcNow;
            SAVE.SaveLastTimeAddHeart(lastTime.ToString());
        }
        else
        {

            lastTime = DateTime.Parse(lastTimeStr);
            TimeSpan timePassed = DateTime.UtcNow - lastTime;
            string text_ = string.Format("{0:D2}:{1:D2}",
                   Mathf.Max(0, timePassed.Minutes), Mathf.Max(0, timePassed.Seconds));
            int extraPlays = (int)(timePassed.TotalSeconds / timeToAddHeart);
            if (extraPlays > 0)
            {
                currentHearts = Mathf.Min(currentHearts + extraPlays, maxHearts);
                lastTime = DateTime.UtcNow; 
                SAVE.SaveLastTimeAddHeart(lastTime.ToString());

            }
            Observer.Instance.Broadcast(EventId.OnUpdateHearts, Tuple.Create(text_, currentHearts));
        }

        SAVE.SaveHearts(currentHearts);
    }
    public void UseHeart()
    {
            if (currentHearts == maxHearts)
            {
                lastTime = DateTime.UtcNow;
                SAVE.SaveLastTimeAddHeart(lastTime.ToString());
            }
            currentHearts--;
            SAVE.SaveHearts(currentHearts);

    }
    public bool IsRemainingHearts()
    {
        return currentHearts > 0;
    }
    IEnumerator UpdateCountdownDisplay()
    {
        while (true)
        {
            if (currentHearts >= maxHearts)
            {
                string text_ = string.Format("Full");
                Observer.Instance.Broadcast(EventId.OnUpdateHearts, Tuple.Create(text_, currentHearts));
                yield return new WaitForSeconds(1);


                continue;
            }

            TimeSpan remaining = TimeSpan.FromSeconds(timeToAddHeart) - (DateTime.UtcNow - lastTime);
            if (remaining.TotalSeconds <= 0)
            {
                currentHearts++;
                currentHearts = Mathf.Min(currentHearts, maxHearts);
                lastTime = DateTime.UtcNow;
                SAVE.SaveLastTimeAddHeart(lastTime.ToString());
                SAVE.SaveHearts(currentHearts);
            }
            string text = string.Format("{0:D2}:{1:D2}",
                               Mathf.Max(0, remaining.Minutes), Mathf.Max(0, remaining.Seconds));
            Observer.Instance.Broadcast(EventId.OnUpdateHearts, Tuple.Create(text,  currentHearts));
            yield return new WaitForSeconds(1f);
        }
    }
}
