using System.Reflection;
using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers.Server;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Spt.Tables;
using SPTarkov.Server.Core.Services.Modding.Custom;

namespace makeshotgunsgreatagain;

// Runs after the mod's items and assorts, before quest and trader caches are built.
[Injectable(TypePriority = OnLoadOrder.Preload + 3)]
public class HuntersDreamQuestLoader(
    CustomQuestService customQuestService,
    ModHelper modHelper,
    TemplateTable templateTable,
    TradersTable tradersTable,
    LocaleTable localeTable,
    ISptLogger<HuntersDreamQuestLoader> logger
) : IOnLoad
{
    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        var folder = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        var definitions = modHelper.GetJsonDataFromFile<List<NewQuestDetails>>(folder, "db/Quests/hunters_dream.json");
        var jaeger = tradersTable[Traders.JAEGER];
        if (definitions.Count != 3 || jaeger.Assort == null || jaeger.QuestAssort == null)
            throw new InvalidOperationException("Hunter's Dream requires three quests and Jaeger's assort.");

        // Check all references before registering any quests or purchase restrictions.
        var unlocks = definitions.SelectMany(d => d.NewQuest.Rewards!["Success"]
            .Where(r => r.Type == RewardType.AssortmentUnlock)
            .Select(r => (QuestId: d.NewQuest.Id, Offer: r.Items!.Single(), Level: r.LoyaltyLevel))).ToList();
        if (unlocks.Count != 3 || definitions.Any(d => templateTable.Quests.ContainsKey(d.NewQuest.Id)))
            throw new InvalidOperationException("Hunter's Dream has missing unlocks or conflicting quest IDs.");

        foreach (var unlock in unlocks)
        {
            if (!templateTable.Items.ContainsKey(unlock.Offer.Template)
                || !jaeger.Assort.Items.Any(i => i.Id == unlock.Offer.Id && i.Template == unlock.Offer.Template)
                || !jaeger.Assort.LoyalLevelItems.TryGetValue(unlock.Offer.Id, out var level)
                || level != unlock.Level)
                throw new InvalidOperationException($"Hunter's Dream cannot find its Jaeger offer: {unlock.Offer.Id}");
        }

        foreach (var definition in definitions)
        {
            // The native service only registers languages supplied by the mod.
            // Supply English for every other installed language, as vanilla fallback.
            foreach (var language in localeTable.Global.Keys)
                definition.Locales.TryAdd(language, definition.Locales["en"]);

            var result = customQuestService.CreateQuest(definition);
            if (!result.Success)
                throw new InvalidOperationException($"Could not register Hunter's Dream: {string.Join("; ", result.Errors)}");
        }

        if (!jaeger.QuestAssort.TryGetValue("success", out var successfulQuests))
        {
            successfulQuests = new Dictionary<MongoId, MongoId>();
            jaeger.QuestAssort["success"] = successfulQuests;
        }
        foreach (var unlock in unlocks)
            successfulQuests[unlock.Offer.Id] = unlock.QuestId;

        logger.Info("[MSGA] Hunter's Dream: registered 3 Jaeger quests and 3 purchase unlocks.");
        return Task.CompletedTask;
    }
}
