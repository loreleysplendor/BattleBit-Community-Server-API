using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityServerAPI.ChaosWarfare.Affects
{
    public interface IPerk
    {
        string Name { get; set; }
        string FriendlyName { get; set; }

        public ChaosPlayer PerkEffect(ChaosPlayer player);
    }
}
