using CommunityServerAPI.ChaosWarfare.Affects.Perks;

namespace CommunityServerAPI.ChaosWarfare.Affects
{
    public interface IPerk
    {
        string Name { get; set; }
        string FriendlyName { get; set; }

        public ChaosPlayer PerkEffect(ChaosPlayer player);
    }

    public class PerkDataset
    {
        // Create string to perk mapping
        public static Dictionary<string, IPerk> PerkDictionary = new()
        {
            { "speedReload", PerkSpeedReload },
        };

        public static readonly IPerk PerkSpeedReload = new SpeedReload();
    }
}
