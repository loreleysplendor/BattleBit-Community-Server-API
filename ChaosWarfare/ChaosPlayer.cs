using BattleBitAPI;
using CommunityServerAPI.ChaosWarfare.Affects;

namespace CommunityServerAPI.ChaosWarfare
{
    public class ChaosPlayer : Player<ChaosPlayer>
    {
        public int Kills;
        public int Deaths;
        public int Headshots;
        public int MeleeKills;
        public float LongestRangeKill;
        public List<ChaosPlayer> Players;

        // player based modifiers (perks)
        public List<IPerk> Perks;

        public override Task OnSpawned()
        {
            // set assigned perk effects
            foreach (var perk in Perks)
            {
                perk.PerkEffect(this);
            }
            return base.OnSpawned();
        }
    }
}
