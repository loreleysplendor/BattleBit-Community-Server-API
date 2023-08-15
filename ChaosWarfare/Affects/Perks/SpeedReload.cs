
namespace CommunityServerAPI.ChaosWarfare.Affects.Perks
{
    public class SpeedReload : IPerk
    {
        public string Name { get; set; } = "speed_reload";
        public string FriendlyName { get; set; } = "Speed Reload";

        public ChaosPlayer PerkEffect(ChaosPlayer player)
        {
            player.Modifications.ReloadSpeedMultiplier = 3;
            return player;
        }


    }
}
