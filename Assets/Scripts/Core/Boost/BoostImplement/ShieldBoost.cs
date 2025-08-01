public class ShieldBoost : UsageBoost
{
    public override void Excute()
    {
        base.Excute();
        playerBoost.SetInvicibility(true);
        playerBoost.OnPlayerDied += PlayerBoost_OnPlayerDied;
    }

    private void PlayerBoost_OnPlayerDied()
    {
        Use();

    }

    public override void Deactive()
    {
        base.Deactive();
        playerBoost.SetInvicibility(false);
        playerBoost.OnPlayerDied -= PlayerBoost_OnPlayerDied;
    }
}
