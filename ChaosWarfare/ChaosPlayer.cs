using BattleBitAPI;
using CommunityServerAPI.ChaosWarfare.Services;

namespace CommunityServerAPI.ChaosWarfare
{
    public class ChaosPlayer : Player<ChaosPlayer>
    {
        public int Kills;
        public int Deaths;
        public int Headshots;
        public int MeleeKills;
        public float LongestRangeKill;

        // player based modifiers (perks)
        public List<IPerk> Perks;

        public ChaosPlayer()
        {
            Kills = 0;
            Deaths = 0;
            Headshots = 0;
            MeleeKills = 0;
            LongestRangeKill = 0;
            Perks = new List<IPerk>();
        }

        public override async Task OnConnected()
        {
            Modifications.RespawnTime = 0.1f;
            return;
        }

        public override async Task OnSpawned()
        {
            // set assigned perk effects
            foreach (var perk in Perks)
            {
                perk.PerkEffect(this);
            }
            return;
        }

        
    }
}
