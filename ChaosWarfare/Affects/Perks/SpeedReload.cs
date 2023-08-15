using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityServerAPI.ChaosWarfare.Affects.Perks
{
    public class SpeedReload : IPerk
    {
        public string Name { get; set; } = "speed_reload";
        public string FriendlyName { get; set; } = "Speed Reload";

        public ChaosPlayer PerkEffect(ChaosPlayer player)
        {
            return player;
        }


    }
}
