using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityServerAPI.ChaosWarfare.Services.Effects
{
    public class Jordans : IPerk
    {
        public string Name { get; set; } = "Jordans";

        public ChaosPlayer PerkEffect(ChaosPlayer player)
        {
            player.Modifications.FallDamageMultiplier = 0;
            player.Modifications.JumpHeightMultiplier = 3;
            return player;
        }
    }
}
