using BattleBitAPI;
using CommunityServerAPI.ChaosWarfare.Affects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // set any perk effects...
            return base.OnSpawned();
        }
    }
}
