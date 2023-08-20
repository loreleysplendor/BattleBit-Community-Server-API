using BattleBitAPI;
using BattleBitAPI.Common;
using BattleBitAPI.Server;
using CommunityServerAPI.ChaosWarfare;
using CommunityServerAPI.ChaosWarfare.Services;
using CommunityServerAPI.ChaosWarfare.Services.Effects;
using System.Net;

class Program
{
    static void Main(string[] args)
    {
        var port = 29294;
        var listener = new ServerListener<ChaosPlayer, GameServer>();

        listener.OnCreatingGameServerInstance = OnCreateGameServerInstance;
        listener.OnGameServerConnecting = OnGameServerConnecting;
        listener.OnValidateGameServerToken = OnValidateGameServerToken;

        listener.Start(port);

        Console.WriteLine("Started listener on port: " + port);

        Thread.Sleep(-1);
    }

    private static async Task<bool> OnValidateGameServerToken(IPAddress ip, ushort gameport, string sentToken)
    {
        await Console.Out.WriteLineAsync(ip + ":" + gameport + " sent " + sentToken);
        return sentToken == "test_token";
    }

    private static async Task<bool> OnGameServerConnecting(IPAddress arg)
    {
        await Console.Out.WriteLineAsync(arg.ToString() + " connecting...");
        return true;
    }

    private static GameServer OnCreateGameServerInstance(IPAddress ip, ushort port)
    {
        //await Console.Out.WriteLineAsync(ip + ":" + port + " Created Instance");
        var gameServer = new GameServer();
        return gameServer;
    }
}

class GameServer : GameServer<ChaosPlayer>
{
    private List<ChaosPlayer> Players;

    private static readonly List<Gadget> GadgetBlacklist = new()
    {
        Gadgets.ImpactGrenade,
        Gadgets.FragGrenade,
        Gadgets.AntiPersonnelMine,
        Gadgets.C4,
        Gadgets.Claymore,
        Gadgets.AntiVehicleGrenade,
        Gadgets.AntiVehicleMine,
        Gadgets.Rpg7Fragmentation,
        Gadgets.Rpg7HeatExplosive,
        Gadgets.Rpg7Pgo7Fragmentation,
        Gadgets.Rpg7Pgo7HeatExplosive,
        Gadgets.Rpg7Pgo7Tandem,
        Gadgets.SuicideC4
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
        MapRotation.SetRotation("DustyDew", "Oildunes", "Isle", "District", "Construction");
        GamemodeRotation.SetRotation("Domination", "Conquest", "CaptureTheFlag");

        // Server Settings
        //ServerSettings.BleedingEnabled = false;
        
    }

    public override async Task OnTick()
    {
        // rich text http://digitalnativestudios.com/textmeshpro/docs/rich-text/
        // command handling
        // TODO: Create leaderboard which includes stats like, headshots, long range, etc...
        //AnnounceLong("INSERT CURRENT GAME LEADER BOARD FOR INTERESTING STATS");
        if (RoundSettings.State == GameState.WaitingForPlayers)
        {
            ForceStartGame();
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
    }

    public override async Task OnRoundEnded()
    {
        //MapRotation.SetRotation("Wakistan", "Azagor", "District");
        //GamemodeRotation.SetRotation("Conquest", "Domination");
    }

    public override async Task OnPlayerConnected(ChaosPlayer player)
    {
        await Console.Out.WriteAsync(player.SteamID + " " + player.Name);
    }

    public override async Task OnAPlayerDownedAnotherPlayer(OnPlayerKillArguments<ChaosPlayer> args)
    {
        // TODO lots of stats capturing...
        // record stats for leaderboard
        if (args.BodyPart > 0 && args.BodyPart < PlayerBody.Shoulder)
        {
            await Console.Out.WriteAsync("Headshot");
            // record headshot
        }

        // record tool stats
        // args.KillerTool
    }
    public override async Task<OnPlayerSpawnArguments?> OnPlayerSpawning(ChaosPlayer player, OnPlayerSpawnArguments request)
    {
        // Warn user for having lethal gadget and tell them it got changed.
        if (GadgetBlacklist.Contains(request.Loadout.Throwable))
        {
            player.Message("You are using illegal gadgets, they've been replaced with non-lethals");
        }

        request.Loadout.LightGadget = GadgetBlacklist.Contains(request.Loadout.Throwable) ? default : request.Loadout.LightGadget;
        request.Loadout.HeavyGadget = GadgetBlacklist.Contains(request.Loadout.Throwable) ? default : request.Loadout.HeavyGadget;
        request.Loadout.Throwable = GadgetBlacklist.Contains(request.Loadout.Throwable) ? default : request.Loadout.Throwable;
        //request.Wearings.Chest = default;

        await Task.CompletedTask;

        // can disallow spawn by returning null

        return request;
    }

    public override async Task OnPlayerSpawned(ChaosPlayer player)
    {
        // Apply Player Perks 
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
            var commandArgs = msg.Split(' ').Skip(1);

            IPerk perk;
            if (Perks.TryFind(commandArgs.First(), out perk))
            {
                player.Perks.Add(perk);
                player.EnableAssignedPerks();
            }
            else
            {
                player.Message("Unable to find that perk...");
            }
            return false;
            // command handling
        } else if(msg.StartsWith("/endRound"))
        {
            ForceEndGame();
            return false;
        }
        return true;
    }
}
