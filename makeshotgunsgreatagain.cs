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
    public override SemanticVersioning.Version Version { get; init; } = new("1.8.0");
    public override Range SptVersion { get; init; } = new("~4.0.0");
    public override string? License { get; init; } = "MIT";
    public override bool? IsBundleMod { get; init; } = true;
    
    public override Dictionary<string, Range>? ModDependencies { get; init; } = new()
    {
        { "com.wtt.commonlib", new Range("~2.0.0") }
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
    // IDs of the weapons we are going to modify
    private const string SAIGA_12K_TPL = "576165642459773c7a400233";
    private const string SAIGA_12K_FULLATO_TPL = "674fe9a75e51f1c47c04ec23";
    private const string BENELLI_M3_TPL = "6259b864ebedf17603599e88";
    private const string KS23_TPL = "5e848cc2988a8701445df1e8";
    private const string KS23_WIRE_STOCK_TPL = "5e848dc4e4dbc5266a4ec63d";
    private const string MP153_TPL = "56dee2bdd2720bc8328b4567";
    
    // IDs for special case weapons
    private const string MP43_TPL = "5d5d85c286f77427997c0883";
    private const string MP43_SAWED_OFF_TPL = "5d5d870186f7742798498584";
    private const string MTS_255_CYLINDER_TPL = "6107328513316926220e3345";
    
    private static readonly List<string> NEW_CARTRIDGE_IDS =
    [
        "6911031c1e9fa1008ce6e1aa", "69111bf76c4be2b06bd0745c", "69111d386d6226b577e619b1", "69111e8fab39296e6f0f310a", "6911202ab8757755a4d62f3c"
    ];
    
    private const string STANDARD_12G_BUCK = "560d5e524bdc2d25448b4571";
    
    private const string SHOTGUN_BASE_CLASS = "5447b6094bdc2dc3278b4567";
    private const string MAGAZINE_BASE_CLASS = "5448bc234bdc2d3c308b4569";
    
    public async Task OnLoad()
    {
        var assembly = Assembly.GetExecutingAssembly();
        await wttCommon.CustomItemServiceExtended.CreateCustomItems(assembly);
        await wttCommon.CustomAssortSchemeService.CreateCustomAssortSchemes(assembly);
        
        ModifyExistingShotguns();
        AddNewCartridgesToShotguns();
    }

    /// <summary>
    /// Finds and modifies the properties of existing shotguns.
    /// </summary>
    private void ModifyExistingShotguns()
    {
        
        var items = databaseService.GetItems();

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
        } else 
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
                        Filter = filterItems.Select(itemId => new MongoId(itemId)).ToHashSet() // <-- CORRECTED
                    }
                ]
            },
            Required = false,
            MergeSlotWithChildren = false,
            Prototype = new MongoId() 
        };
    }
}