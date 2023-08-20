
namespace CommunityServerAPI.ChaosWarfare.Services.Effects
{
    public class SpeedReload : IPerk
    {
        public string Name { get; set; } = "SpeedReload";

        public ChaosPlayer PerkEffect(ChaosPlayer player)
        {
            player.Modifications.ReloadSpeedMultiplier = 2;
            return player;
        }


    }
}
