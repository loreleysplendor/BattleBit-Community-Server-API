using BattleBitAPI.Common;
using CommunityServerAPI.ChaosWarfare.Services.Effects;
using System.Reflection;

namespace CommunityServerAPI.ChaosWarfare.Services
{
    public interface IPerk
    {
        string Name { get; set; }

        public ChaosPlayer PerkEffect(ChaosPlayer player);
    }

    public static class Perks
    {
        // Create string to perk mapping
        private static Dictionary<string, IPerk> _Perks;

        public static readonly IPerk PerkSpeedReload = new SpeedReload();
        public static readonly IPerk PerkDoubleTime = new DoubleTime();
        public static readonly IPerk PerkJordans = new Jordans();

        public static bool TryFind(string name, out IPerk item)
        {
            return _Perks.TryGetValue(name, out item);
        }
        static Perks()
        {
            var members = typeof(Perks).GetMembers(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            _Perks = new Dictionary<string, IPerk>(members.Length);
            foreach (var memberInfo in members)
            {
                if (memberInfo.MemberType == System.Reflection.MemberTypes.Field)
                {
                    var field = ((FieldInfo)memberInfo);
                    if (field.FieldType == typeof(IPerk))
                    {
                        var att = (IPerk)field.GetValue(null);
                        _Perks.Add(att.Name, att);
                    }
                }
            }
        }
    }
}

// Create a perk enum which can be used for keys...