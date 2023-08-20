
namespace CommunityServerAPI.ChaosWarfare.Services.Effects
{
    public class DoubleTime : IPerk
    {
        public string Name { get; set; } = "DoubleTime";

        public ChaosPlayer PerkEffect(ChaosPlayer player)
        {
            player.Modifications.RunningSpeedMultiplier = 3;
            return player;
        }


    }
}
