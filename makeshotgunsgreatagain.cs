using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Utils;
using Range = SemanticVersioning.Range;

namespace makeshotgunsgreatagain;

public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "com.vinihns.makeshotgunsgreatagain";
    public override string Name { get; init; } = "Make Shotguns Great Again";
    public override string Author { get; init; } = "ViniHNS";
    public override SemanticVersioning.Version Version { get; init; } = new("1.13.0");
    public override Range SptVersion { get; init; } = new("~4.0.0");
    public override string? License { get; init; } = "MIT";
    public override bool? IsBundleMod { get; init; } = true;

    public override Dictionary<string, Range>? ModDependencies { get; init; } = new()
    {
        { "com.wtt.commonlib", new Range("2.0.15") }

    };

    public override string? Url { get; init; }
    public override List<string>? Contributors { get; init; }
    public override List<string>? Incompatibilities { get; init; }
}

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 2)]
public class Mod(
    WTTServerCommonLib.WTTServerCommonLib wttCommon,
    DatabaseService databaseService,
    ItemHelper itemHelper,
    ISptLogger<Mod> logger
) : IOnLoad
{
    private const string SAIGA_12K_TPL = "576165642459773c7a400233";
    private const string SAIGA_12K_FULLATO_TPL = "674fe9a75e51f1c47c04ec23";
    private const string BENELLI_M3_TPL = "6259b864ebedf17603599e88";
    private const string KS23_TPL = "5e848cc2988a8701445df1e8";
    private const string KS23_WIRE_STOCK_TPL = "5e848dc4e4dbc5266a4ec63d";
    private const string MP153_TPL = "56dee2bdd2720bc8328b4567";

    private const string AA12_GEN1_TPL = "66ffa9b66e19cc902401c5e8";
    private const string AA12_GEN2_TPL = "67124dcfa3541f2a1f0e788b";


    private const string MP155_ULTIMA_HANDGUARD_TPL = "606ee5c81246154cad35d65e";
    private const string SPRM_RAIL_MOUNT_TPL = "55d48a634bdc2d8b2f8b456a";
    private const string ETMI_019_RAIL_TPL = "5dfe14f30b92095fd441edaf";

    // IDs for special case weapons
    private const string MP43_TPL = "5d5d85c286f77427997c0883";
    private const string MP43_SAWED_OFF_TPL = "5d5d870186f7742798498584";
    private const string MTS_255_CYLINDER_TPL = "6107328513316926220e3345";
    private const string MTS_255_TPL = "60db29ce99594040e04c4a27";

    private static readonly List<string> NEW_CARTRIDGE_IDS =
    [
        "6911031c1e9fa1008ce6e1aa", "69111bf76c4be2b06bd0745c", "69111d386d6226b577e619b1", "69111e8fab39296e6f0f310a", "6911202ab8757755a4d62f3c", "698924bf6dcd41ac313f5921"
    ];

    private static readonly List<string> SIGHTS_TO_ADD_IDS = [
      "57ac965c24597706be5f975c",
      "57aca93d2459771f2c7e26db",
      "544a3f024bdc2d1d388b4568",
      "544a3a774bdc2d3a388b4567",
      "5d2dc3e548f035404a1a4798",
      "57adff4f24597737f373b6e6",
      "5c0517910db83400232ffee5",
      "591c4efa86f7741030027726",
      "570fd79bd2720bc7458b4583",
      "570fd6c2d2720bc6458b457f",
      "558022b54bdc2dac148b458d",
      "5c07dd120db834001c39092d",
      "5c0a2cec0db834001b7ce47d",
      "58491f3324597764bc48fa02",
      "584924ec24597768f12ae244",
      "5b30b0dc5acfc400153b7124",
      "6165ac8c290d254f5e6b2f6c",
      "60a23797a37c940de7062d02",
      "5d2da1e948f035477b1ce2ba",
      "5c0505e00db834001b735073",
      "609a63b6e2ff132951242d09",
      "584984812459776a704a82a6",
      "59f9d81586f7744c7506ee62",
      "570fd721d2720bc5458b4596",
      "57ae0171245977343c27bfcf",
      "5dfe6104585a0c3e995c7b82",
      "544a3d0a4bdc2d1b388b4567",
      "5d1b5e94d7ad1a2b865a96b0",
      "609bab8b455afd752b2e6138",
      "58d39d3d86f77445bb794ae7",
      "616554fe50224f204c1da2aa",
      "5c7d55f52e221644f31bff6a",
      "616584766ef05c2ce828ef57",
      "5b3b6dc75acfc47a8773fb1e",
      "615d8d878004cc50514c3233",
      "5b2389515acfc4771e1be0c0",
      "577d128124597739d65d0e56",
      "618b9643526131765025ab35",
      "618bab21526131765025ab3f",
      "5c86592b2e2216000e69e77c",
      "5a37ca54c4a282000d72296a",
      "5d0a29fed7ad1a002769ad08",
      "5c064c400db834001d23f468",
      "58d2664f86f7747fec5834f6",
      "57c69dd424597774c03b7bbc",
      "5b3b99265acfc4704b4a1afb",
      "5aa66a9be5b5b0214e506e89",
      "5aa66c72e5b5b00016327c93",
      "5c1cdd302e221602b3137250",
      "61714b2467085e45ef140b2c",
      "6171407e50224f204c1da3c5",
      "61713cc4d8e3106d9806c109",
      "5b31163c5acfc400153b71cb",
      "5a33b652c4a28232996e407c",
      "5a33b2c9c4a282000c5a9511",
      "59db7eed86f77461f8380365",
      "5a1ead28fcdbcb001912fa9f",
      "5dff77c759400025ea5150cf",
      "626bb8532c923541184624b4",
      "62811f461d5df4475f46a332",
      "63fc449f5bd61c6cf3784a88",
      "6477772ea8a38bb2050ed4db",
      "6478641c19d732620e045e17",
      "64785e7c19d732620e045e15",
      "65392f611406374f82152ba5",
      "653931da5db71d30ab1d6296",
      "655f13e0a246670fb0373245",
      "6567e751a715f85433025998",
      "6761759e7ee06333f108bf86",
      "67641a851b2899700609901a"
    ];

    private const string STANDARD_12G_BUCK = "560d5e524bdc2d25448b4571";

    private const string SHOTGUN_BASE_CLASS = "5447b6094bdc2dc3278b4567";
    private const string MAGAZINE_BASE_CLASS = "5448bc234bdc2d3c308b4569";

    
    public async Task OnLoad()
    {
        var assembly = Assembly.GetExecutingAssembly();
        await wttCommon.CustomItemServiceExtended.CreateCustomItems(assembly);
        await wttCommon.CustomAssortSchemeService.CreateCustomAssortSchemes(assembly, "db/weaponPresets/Assorts");
        await wttCommon.CustomBotLoadoutService.CreateCustomBotLoadouts(assembly, "db/weaponPresets/BotLoadouts");
        await wttCommon.CustomHideoutRecipeService.CreateHideoutRecipes(assembly);

        ModifyExistingShotguns();
        AddNewCartridgesToShotguns();
        ModifyRails();
    }

    /// <summary>
    /// Finds and modifies the properties of existing shotguns.
    /// </summary>
    private void ModifyExistingShotguns()
    {

        var items = databaseService.GetItems();

        // --- Modify AA-12 ---
        if (items.TryGetValue(AA12_GEN1_TPL, out var aa12gen1))
        {
            ModifyAa12(aa12gen1);
        }
        else
        {
            logger.Warning($"Could not find AA-12 GEN1 ({AA12_GEN1_TPL}) to modify.");
        }

        if (items.TryGetValue(AA12_GEN2_TPL, out var aa12gen2))
        {
            ModifyAa12(aa12gen2);
        }
        else
        {
            logger.Warning($"Could not find AA-12 GEN2 ({AA12_GEN2_TPL}) to modify.");
        }

        // --- Modify MTs-255 ---
        if (items.TryGetValue(MTS_255_TPL, out var mts255))
        {
            ModifyMts255(mts255);
        }
        else
        {
            logger.Warning($"Could not find MTs-255 ({MTS_255_TPL}) to modify.");
        }

        // --- Modify Saiga-12K ---
        if (items.TryGetValue(SAIGA_12K_TPL, out var saiga12k))
        {
            ModifySaiga12K(saiga12k);
        }
        else
        {
            logger.Warning($"Could not find Saiga-12K ({SAIGA_12K_TPL}) to modify.");
        }

        if (items.TryGetValue(SAIGA_12K_FULLATO_TPL, out var saiga12kfullato))
        {
            ModifySaiga12K(saiga12kfullato);
        }
        else
        {
            logger.Warning($"Could not find Saiga-12K Full-Auto ({SAIGA_12K_FULLATO_TPL}) to modify.");
        }

        // --- Modify Benelli M3 ---
        if (items.TryGetValue(BENELLI_M3_TPL, out var benelliM3))
        {
            ModifyBenelliM3(benelliM3);
        }
        else
        {
            logger.Warning($"Could not find Benelli M3 ({BENELLI_M3_TPL}) to modify.");
        }

        // --- Modify MP-153 ---
        if (items.TryGetValue(MP153_TPL, out var mp153))
        {
            ModifyMp153(mp153);
        }
        else
        {
            logger.Warning($"Could not find MP-153 ({MP153_TPL}) to modify.");
        }

        // --- Modify KS-23 ---
        if (items.TryGetValue(KS23_TPL, out var ks23) && items.TryGetValue(KS23_WIRE_STOCK_TPL, out var ks23WireStock))
        {
            ModifyKS23(ks23, ks23WireStock);
        }
        else
        {
            logger.Warning($"Could not find KS-23 ({KS23_TPL}) or its wire stock ({KS23_WIRE_STOCK_TPL}) to modify.");
        }
    }

    private void ModifyRails()
    {
        var items = databaseService.GetItems();

        if (items.TryGetValue(ETMI_019_RAIL_TPL, out var etmiRail))
        {
            var filter = etmiRail.Properties?.Slots?.FirstOrDefault()?.Properties?.Filters?.FirstOrDefault()?.Filter;

            if (filter != null)
            {
                filter.Clear();
                foreach (var id in SIGHTS_TO_ADD_IDS)
                {
                    filter.Add(new MongoId(id));
                }
            }
        }
        else
        {
            logger.Warning($"Could not find ETMI rail ({ETMI_019_RAIL_TPL}) to modify.");
        }

        if (items.TryGetValue(SPRM_RAIL_MOUNT_TPL, out var sprmRail))
        {
            var filter = sprmRail.Properties?.Slots?.FirstOrDefault()?.Properties?.Filters?.FirstOrDefault()?.Filter;

            if (filter != null)
            {
                filter.Clear();
                foreach (var id in SIGHTS_TO_ADD_IDS)
                {
                    filter.Add(new MongoId(id));
                }
            }
        }
        else
        {
            logger.Warning($"Could not find SPRM rail mount ({SPRM_RAIL_MOUNT_TPL}) to modify.");
        }
    }

    private void AddNewCartridgesToShotguns()
    {
        if (NEW_CARTRIDGE_IDS.Count == 0)
        {
            logger.Warning("No new cartridge IDs were defined. Skipping cartridge addition.");
            return;
        }

        var allItems = databaseService.GetItems();
        var allItemsList = allItems.Values.ToList();

        var shotgunMagazines = FindCompatibleMagazines(allItemsList);
        var shotguns = FindCompatibleShotguns(allItemsList);

        // Add cartridges to magazines
        AddCartridgesToItems(allItems, shotgunMagazines, "Cartridges");

        // Add cartridges to shotguns
        AddCartridgesToItems(allItems, shotguns, "Chambers");

        // Special cases
        AddCartridgesToSpecialWeapons(allItems);
    }

    private List<string> FindCompatibleMagazines(List<TemplateItem> items)
    {
        var referenceCartridgeId = new MongoId(STANDARD_12G_BUCK);

        return items
            .Where(item => itemHelper.IsOfBaseclass(item.Id, MAGAZINE_BASE_CLASS))
            .Where(magazine =>
                magazine.Properties?.Cartridges?.FirstOrDefault()?.Properties?.Filters?.FirstOrDefault()?.Filter?.Contains(referenceCartridgeId) == true
            )
            .Select(magazine => magazine.Id.ToString())
            .ToList();
    }

    private List<string> FindCompatibleShotguns(List<TemplateItem> items)
    {
        var referenceCartridgeId = new MongoId(STANDARD_12G_BUCK);

        return items
            .Where(item => itemHelper.IsOfBaseclass(item.Id, SHOTGUN_BASE_CLASS))
            .Where(shotgun =>
                shotgun.Properties?.Chambers?.FirstOrDefault()?.Properties?.Filters?.FirstOrDefault()?.Filter?.Contains(referenceCartridgeId) == true
            )
            .Select(shotgun => shotgun.Id.ToString())
            .ToList();
    }

    private void AddCartridgesToItems(IReadOnlyDictionary<MongoId, TemplateItem> itemsDb, List<string> itemIds, string containerType)
    {
        var newCartridgeMongoIds = NEW_CARTRIDGE_IDS.Select(id => new MongoId(id));

        foreach (var itemId in itemIds)
        {
            if (!itemsDb.TryGetValue(new MongoId(itemId), out var item)) continue;

            var container = (containerType == "Chambers" ? item.Properties?.Chambers : item.Properties?.Cartridges)?.FirstOrDefault();
            var filter = container?.Properties?.Filters?.FirstOrDefault()?.Filter;

            if (filter != null)
            {
                foreach (var newId in newCartridgeMongoIds)
                {
                    filter.Add(newId);
                }
            }
        }
    }

    private void AddCartridgesToSpecialWeapons(IReadOnlyDictionary<MongoId, TemplateItem> itemsDb)
    {
        var newCartridgeMongoIds = NEW_CARTRIDGE_IDS.Select(id => new MongoId(id));

        // MP43 variants (double barrel)
        var mp43Ids = new[] { MP43_TPL, MP43_SAWED_OFF_TPL };
        foreach (var weaponId in mp43Ids)
        {
            if (!itemsDb.TryGetValue(new MongoId(weaponId), out var weapon)) continue;

            // Iterate through both chambers (0 and 1)
            foreach (var chamber in weapon.Properties?.Chambers ?? Enumerable.Empty<Slot>())
            {
                var filter = chamber.Properties?.Filters?.FirstOrDefault()?.Filter;
                if (filter != null)
                {
                    foreach (var newId in newCartridgeMongoIds)
                    {
                        filter.Add(newId);
                    }
                }
            }
        }

        // MTS-255 cylinder (revolver shotgun)
        if (itemsDb.TryGetValue(new MongoId(MTS_255_CYLINDER_TPL), out var cylinder))
        {
            foreach (var chamberSlot in cylinder.Properties?.Slots ?? Enumerable.Empty<Slot>())
            {
                var filter = chamberSlot.Properties?.Filters?.FirstOrDefault()?.Filter;
                if (filter != null)
                {
                    foreach (var newId in newCartridgeMongoIds)
                    {
                        filter.Add(newId);
                    }
                }
            }
        }
    }

    private void ModifyMp153(TemplateItem mp153)
    {
        var magazineFilter = mp153.Properties.Slots.ElementAtOrDefault(2)?.Properties?.Filters?.FirstOrDefault()?.Filter;
        if (magazineFilter != null)
        {
            magazineFilter.Add(new MongoId("6910ffd279b844c344ce9cdf"));
        }

        var handguardFilter = mp153.Properties.Slots.ElementAtOrDefault(1)?.Properties?.Filters?.FirstOrDefault()?.Filter;
        if (handguardFilter != null)
        {
            handguardFilter.Add(new MongoId(MP155_ULTIMA_HANDGUARD_TPL));
        }
    }

    private void ModifyAa12(TemplateItem aa12)
    {
        aa12.Properties.BFirerate = 450;
        aa12.Properties.RecoilForceUp -= 30;
        aa12.Properties.RecoilForceBack -= 30;
    }

    private void ModifySaiga12K(TemplateItem saiga12k)
    {
        void AddItemsToSlotFilter(int slotIndex, List<string> itemIds)
        {
            var filter = saiga12k.Properties?.Slots?.ElementAtOrDefault(slotIndex)?.Properties?.Filters?.FirstOrDefault()?.Filter;

            if (filter != null)
            {
                foreach (var id in itemIds)
                {
                    filter.Add(new MongoId(id));
                }
            }
            else
            {
                logger.Warning($"Could not find filter for slot index {slotIndex} on {saiga12k.Id}.");
            }
        }

        AddItemsToSlotFilter(1, ["5b800e9286f7747a8b04f3ff", "5b80242286f77429445e0b47", "647dd2b8a12ebf96c3031655", "5c17664f2e2216398b5a7e3c", "5d2c829448f0353a5c7d6674", "5efaf417aeb21837e749c7f2"]);
        AddItemsToSlotFilter(4, ["5d2c772c48f0355d95672c25", "5d2c770c48f0354b4a07c100", "5d2c76ed48f03532f2136169", "5649af884bdc2d1b2b8b4589"]);
        AddItemsToSlotFilter(5, ["5649d9a14bdc2d79388b4580"]);
        AddItemsToSlotFilter(7, ["6910dd763afa5d8fab09b27c"]);
    }

    private void ModifyBenelliM3(TemplateItem benelliM3)
    {
        if (benelliM3.Properties != null)
        {
            benelliM3.Properties.SingleFireRate = 850;
            benelliM3.Properties.BFirerate = 200;
            benelliM3.Properties.CanQueueSecondShot = true;
        }

        var handguardFilter = benelliM3.Properties.Slots.ElementAtOrDefault(1)?.Properties?.Filters?.FirstOrDefault()?.Filter;
                 if (handguardFilter != null)
                 {
                     handguardFilter.Add(new MongoId("6910f8984a20c41289074652"));
        }

    }

    private void ModifyMts255(TemplateItem mts255)
    {
        if (mts255.Properties != null)
        {

            mts255.Properties.DoubleActionAccuracyPenalty = 0;

            if (mts255.Properties.Slots == null)
            {
                logger.Error("MTs-255 has no slots. Skipping modification.");
                return;
            }

            var mtsSlots = mts255.Properties.Slots.ToList();
            mtsSlots.Add(CreateSlot("mod_mount", "67041a851b2899700609901b", mts255.Id, [SPRM_RAIL_MOUNT_TPL, ETMI_019_RAIL_TPL]));
            mts255.Properties.Slots = mtsSlots;
        }
    }

    private void ModifyKS23(TemplateItem ks23, TemplateItem ks23WireStock)
    {
        if (ks23.Properties?.Slots == null || ks23WireStock.Properties?.Slots == null)
        {
            logger.Error("Failed to modify KS-23: Properties or Slots list is null.");
            return;
        }

        // Add 6-shell magazine to the magazine slot filter
        var magazineFilter = ks23.Properties.Slots.ElementAtOrDefault(2)?.Properties?.Filters?.FirstOrDefault()?.Filter;
        if (magazineFilter != null)
        {
            magazineFilter.Add(new MongoId("6910ebc01b0a1cfdd5877581"));
        }

        // Create a new list from the existing slots
        var ks23Slots = ks23.Properties.Slots.ToList();
        // Add the new slot to the new list
        ks23Slots.Add(CreateSlot("mod_mount", "665c88a09a8a1cfbe59cd8d2", ks23.Id,
            ["55d48a634bdc2d8b2f8b456a"]));
        // Assign the updated list back to the properties
        ks23.Properties.Slots = ks23Slots;

        if (ks23WireStock.Properties.Prefab != null)
        {
            ks23WireStock.Properties.Prefab.Path = "ks23stock.bundle";
        }

        var wireStockSlots = ks23WireStock.Properties.Slots.ToList();
        wireStockSlots.Add(CreateSlot("mod_stock", "665b5c811722cdfd0a6e6dd5", ks23WireStock.Id,
            ["5a0c59791526d8dba737bba7"]));
        // Assign the updated list back to the properties
        ks23WireStock.Properties.Slots = wireStockSlots;
    }

    /// <summary>
    /// Creates a new Slot object to be added to an item.
    /// </summary>
    /// <param name="name">The internal name of the slot (e.g., "mod_stock").</param>
    /// <param name="id">The new unique ID for this slot.</param>
    /// <param name="parentId">The ID of the item this slot will belong to.</param>
    /// <param name="filterItems">A list of item TPLs that can be attached to this slot.</param>
    /// <returns>A new Slot object.</returns>
    private Slot CreateSlot(string name, string id, MongoId parentId, List<string> filterItems)
    {
        return new Slot
        {
            Name = name,
            Id = new MongoId(id),
            Parent = parentId,
            Properties = new SlotProperties
            {
                Filters =
                [
                    new SlotFilter
                    {
                        Shift = 0, 
                        // Convert the list of string IDs to a HashSet of MongoId objects
                        Filter = filterItems.Select(itemId => new MongoId(itemId)).ToHashSet()
                    }
                ]
            },
            Required = false,
            MergeSlotWithChildren = false,
            Prototype = new MongoId()
        };
    }
}