using BattleBitAPI;
using BattleBitAPI.Common;
using BattleBitAPI.Server;
using CommunityServerAPI.ChaosWarfare;
using CommunityServerAPI.ChaosWarfare.Affects;
using CommunityServerAPI.ChaosWarfare.Affects.Perks;

class Program
{
    static void Main(string[] args)
    {
        var port = 29294;
        var listener = new ServerListener<ChaosPlayer, GameServer>();
        listener.Start(port);

        Console.WriteLine("Started listener on port: " + port);

        Thread.Sleep(-1);
    }
}

class GameServer : GameServer<ChaosPlayer>
{
    private List<ChaosPlayer> Players;

    private static readonly List<Gadget> GadgetWhitelist = new()
    {
        Gadgets.Binoculars,
        Gadgets.BinoSoflam,
        Gadgets.Bandage,
        Gadgets.GrapplingHook,
        Gadgets.AirDrone,
        Gadgets.Flare,
        Gadgets.Flashbang,
        Gadgets.RiotShield,
        // other non-lethal gadgets + throwables.
    };

    private static readonly List<string> WearablesBlacklist = new()
    {
        // disable wearables which increase armour over other classes
    };

    private void Setup()
    {
        // TODO: Curate map rotation.
        // OR only have a select few maps + gamemodes available.
        MapRotation.SetRotation("DustyDew", "Wakistan", "Isle", "District");
        GamemodeRotation.SetRotation("Domination", "FrontLine", "CaptureTheFlag");

        // Server Settings
        ServerSettings.BleedingEnabled = false;
        
    }

    public override async Task OnTick()
    {
        // rich text http://digitalnativestudios.com/textmeshpro/docs/rich-text/
        // command handling
        // TODO: Create leaderboard which includes stats like, headshots, long range, etc...
        AnnounceLong("INSERT CURRENT GAME LEADER BOARD FOR INTERESTING STATS");

        if (RoundSettings.State == 0)
        {
            await Console.Out.WriteAsync("test");
        }

        return;
    }

    public override async Task OnConnected()
    {
        await Console.Out.WriteLineAsync(GameIP + " Connected");
        Setup();
    }

    public override async Task OnDisconnected()
    {
        await Console.Out.WriteLineAsync(GameIP + " Disconnected");
    }

    public override async Task OnRoundStarted()
    {
        // Long matches.
        RoundSettings.MaxTickets = 1000000;

        Players = AllPlayers.ToList();
        foreach (var p in Players)
        {
            p.Kills = 0;
            p.Deaths = 0;
            p.Headshots = 0;
            p.MeleeKills = 0;
            p.LongestRangeKill = 0;
        }
    }

    public override async Task OnRoundEnded()
    {
    }

    public override async Task OnPlayerConnected(ChaosPlayer player)
    {
    }

    public override async Task OnAPlayerDownedAnotherPlayer(OnPlayerKillArguments<ChaosPlayer> args)
    {
        // TODO lots of stats capturing...
        // record stats for leaderboard
        if (args.BodyPart > 0 && args.BodyPart < PlayerBody.Shoulder)
        {
            // record headshot
        }

        // record tool stats
        // args.KillerTool
    }
    public override async Task<OnPlayerSpawnArguments> OnPlayerSpawning(ChaosPlayer player, OnPlayerSpawnArguments request)
    {
        // Warn user for having lethal gadget and tell them it got changed.
        if (!GadgetWhitelist.Contains(request.Loadout.Throwable))
        {
            player.WarnPlayer("You are using illegal gadgets, they've been replaced with non-lethals");
        }

        request.Loadout.LightGadget = GadgetWhitelist.Contains(request.Loadout.Throwable) ? default : null;
        request.Loadout.HeavyGadget = GadgetWhitelist.Contains(request.Loadout.Throwable) ? default : null;
        request.Loadout.Throwable = GadgetWhitelist.Contains(request.Loadout.Throwable) ? default : null;
        request.Wearings.Chest = default;

        await Task.CompletedTask;

        return request;
    }

    public override async Task OnPlayerSpawned(ChaosPlayer player)
    {
    }

    public override async Task OnPlayerJoiningToServer(ulong steamID, PlayerJoiningArguments args)
    {
        // fetch player stats from local server.
        // Use official stats as base, return whatever level is highest.
    }

    public override async Task OnGameStateChanged(GameState oldState, GameState newState)
    {
    }

    public override async Task<bool> OnPlayerTypedMessage(ChaosPlayer player, ChatChannel channel, string msg)
    {
        // TODO: profanity filtering
        if (msg.StartsWith("/addPerk"))
        {
            player.Perks.Add(PerkDataset.PerkDictionary.GetValueOrDefault("speedReload"));
            // command handling
        }
        return true;
    }
}
