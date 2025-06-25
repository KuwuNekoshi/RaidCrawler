using PKHeX.Core;
using pkNX.Structures.FlatBuffers.Gen9;
using System.Text.Json;

namespace RaidCrawler.Core.Structures;

public record RaidContainer
{
    public readonly TeraEncounter[]? GemTeraRaidsBase;
    public readonly TeraEncounter[]? GemTeraRaidsKitakami;
    public readonly TeraEncounter[]? GemTeraRaidsBlueberry;
    public TeraDistribution[]? DistTeraRaids;
    public TeraMight[]? MightTeraRaids;
    public readonly IReadOnlyList<RaidFixedRewards>? BaseFixedRewards;
    public readonly IReadOnlyList<RaidLotteryRewards>? BaseLotteryRewards;
    public IReadOnlyList<DeliveryRaidFixedRewardItem>? DeliveryRaidFixedRewards;
    public IReadOnlyList<DeliveryRaidLotteryRewardItem>? DeliveryRaidLotteryRewards;
    public DeliveryGroupID DeliveryRaidPriority = new() { GroupID = new() };

    public IReadOnlyList<Raid> Raids { get; private set; } = new List<Raid>();
    public IReadOnlyList<ITeraRaid> Encounters { get; private set; } = new List<ITeraRaid>();
    public IReadOnlyList<IReadOnlyList<(int, int, int)>> Rewards { get; private set; } =
        new List<List<(int, int, int)>>();
    public string Game { get; private set; } = "Scarlet";
    public GameStrings Strings { get; private set; }

    // Files containing serialized data for all possible 1 through 6 star raids
    private readonly string[] RaidDataBase =
    [
        "raid_enemy_01_array.bin",
        "raid_enemy_02_array.bin",
        "raid_enemy_03_array.bin",
        "raid_enemy_04_array.bin",
        "raid_enemy_05_array.bin",
        "raid_enemy_06_array.bin",
    ];

    private readonly string[] RaidDataKitakami =
    [
        "su1_raid_enemy_01_array.bin",
        "su1_raid_enemy_02_array.bin",
        "su1_raid_enemy_03_array.bin",
        "su1_raid_enemy_04_array.bin",
        "su1_raid_enemy_05_array.bin",
        "su1_raid_enemy_06_array.bin",
    ];

    private readonly string[] RaidDataBlueberry =
    [
        "su2_raid_enemy_01_array.bin",
        "su2_raid_enemy_02_array.bin",
        "su2_raid_enemy_03_array.bin",
        "su2_raid_enemy_04_array.bin",
        "su2_raid_enemy_05_array.bin",
        "su2_raid_enemy_06_array.bin",
    ];

    public RaidContainer(string game)
    {
        Game = game;
        Strings = GameInfo.GetStrings("en");
        GemTeraRaidsBase = TeraEncounter.GetAllEncounters(RaidDataBase, TeraRaidMapParent.Paldea);
        GemTeraRaidsKitakami = TeraEncounter.GetAllEncounters(RaidDataKitakami, TeraRaidMapParent.Kitakami);
        GemTeraRaidsBlueberry = TeraEncounter.GetAllEncounters(RaidDataBlueberry, TeraRaidMapParent.Blueberry);
        BaseFixedRewards = JsonSerializer.Deserialize<IReadOnlyList<RaidFixedRewards>>(Utils.GetStringResource("raid_fixed_reward_item_array.json") ?? "[]");
        BaseLotteryRewards = JsonSerializer.Deserialize<IReadOnlyList<RaidLotteryRewards>>(Utils.GetStringResource("raid_lottery_reward_item_array.json") ?? "[]");
    }

    public int GetRaidCount() => Raids.Count;
    public void ClearRaids() => Raids = new List<Raid>();
    public void SetRaids(IReadOnlyList<Raid> raids) => Raids = raids;
    public int GetEncounterCount() => Encounters.Count;
    public void ClearEncounters() => Encounters = new List<ITeraRaid>();
    public void SetEncounters(IReadOnlyList<ITeraRaid> encs) => Encounters = encs;
    public int GetRewardsCount() => Rewards.Count;
    public void ClearRewards() => Rewards = new List<List<(int, int, int)>>();
    public void SetRewards(IReadOnlyList<IReadOnlyList<(int, int, int)>> rewards) => Rewards = rewards;
    public void SetGame(string game) => Game = game;
}
