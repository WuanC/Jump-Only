using UnityEngine;

public class SlowMotion : TimedBoost
{
    [SerializeField] float timeScale;
    float tempTimeScale;
    public override void Excute()
    {
        tempTimeScale = Time.timeScale;
        Time.timeScale = timeScale;
    }
    public override void Deactive()
    {
        base.Deactive();
        Time.timeScale = tempTimeScale;
    }




}
