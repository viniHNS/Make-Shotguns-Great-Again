import { DependencyContainer } from "tsyringe";
import { IPostDBLoadMod } from "@spt/models/external/IPostDBLoadMod";
import { DatabaseServer } from "@spt/servers/DatabaseServer";
import { IPreSptLoadMod } from "@spt/models/external/IpreSptLoadMod";
import { ILogger } from "@spt/models/spt/utils/ILogger";
import { CustomItemService } from "@spt/services/mod/CustomItemService";
import { LogTextColor } from "@spt/models/spt/logging/LogTextColor";
import { ItemHelper } from "@spt/helpers/ItemHelper";
import { BaseClasses } from "@spt/models/enums/BaseClasses";
import { JsonUtil } from "@spt/utils/JsonUtil";
import path from "path";
import fs from "fs";

class Mod implements IPostDBLoadMod, IPreSptLoadMod {
  private readonly MOD_NAME = "[ViniHNS] Make Shotguns Great Again";
  
  // Item IDs constants
  private readonly ITEM_IDS = {
    BEAR_LOWER: "55d7217a4bdc2d86028b456d",
    SAIGA_12K: "576165642459773c7a400233",
    BENELLI_M3: "6259b864ebedf17603599e88",
    KS23: "5e848cc2988a8701445df1e8",
    KS23_WIRE_STOCK: "5e848dc4e4dbc5266a4ec63d",
    MP153: "56dee2bdd2720bc8328b4567",
    MP133: "54491c4f4bdc2db1078b4568",
    MP12: "67537f2e72cb0015b8512669",
    MP43: "5580223e4bdc2d1c128b457f",
    MP43_SAWED_OFF: "64748cb8de82c85eaf0a273a",
    MTS_255_CYLINDER: "60dc519adf4c47305f6d410d",
    STANDARD_12G_BUCK: "560d5e524bdc2d25448b4571"
  };

  private readonly NEW_CARTRIDGE_IDS = [
    "6756378c3825f690a72789bf",
    "675766d7a045de209488e750", 
    "67579112e55c9b7479e5d9ea",
    "6758e4a30db075f8dc01bb17"
  ];

  preSptLoad(container: DependencyContainer): void {
    const logger = container.resolve<ILogger>("WinstonLogger");
    logger.logWithColor(`${this.MOD_NAME}: Loading...`, LogTextColor.GREEN);
  }

  public postDBLoad(container: DependencyContainer): void {
    const logger = container.resolve<ILogger>("WinstonLogger");
    
    try {
      const databaseServer = container.resolve<DatabaseServer>("DatabaseServer");
      const tables = databaseServer.getTables();
      const itemHelper = container.resolve<ItemHelper>("ItemHelper");
      const customItemService = container.resolve<CustomItemService>("CustomItemService");

      // Apply modifications
      this.applyShotgunModifications(tables);
      this.addNewCartridgesToShotguns(tables, itemHelper);
      this.addCustomModifications(tables);
      this.loadCustomDatabase(container);

      logger.logWithColor(`${this.MOD_NAME}: Loading complete.`, LogTextColor.GREEN);
    } catch (error) {
      logger.error(`${this.MOD_NAME}: Error during loading: ${error}`);
    }
  }

  private applyShotgunModifications(tables: any): void {
    const items = tables.templates.items;
    
    // Saiga-12K modifications
    this.modifySaiga12K(items[this.ITEM_IDS.SAIGA_12K]);
    
    // Benelli M3 modifications
    this.modifyBenelliM3(items[this.ITEM_IDS.BENELLI_M3]);
    
    // Add MP-12 to primary and secondary slots
    this.addItemToSlots(items[this.ITEM_IDS.BEAR_LOWER], [0, 1], this.ITEM_IDS.MP12);
  }

  private modifySaiga12K(saiga12K: any): void {
    const modifications = {
      handguards: [
        "5b800e9286f7747a8b04f3ff", "5b80242286f77429445e0b47",
        "647dd2b8a12ebf96c3031655", "5c17664f2e2216398b5a7e3c",
        "5d2c829448f0353a5c7d6674", "5efaf417aeb21837e749c7f2"
      ],
      dustCovers: [
        "5d2c772c48f0355d95672c25", "5d2c770c48f0354b4a07c100",
        "5d2c76ed48f03532f2136169", "5649af884bdc2d1b2b8b4589"
      ],
      rearSights: ["5649d9a14bdc2d79388b4580"]
    };

    // Add modifications to slots
    saiga12K._props.Slots[1]._props.filters[0].Filter.push(...modifications.handguards);
    saiga12K._props.Slots[4]._props.filters[0].Filter.push(...modifications.dustCovers);
    saiga12K._props.Slots[5]._props.filters[0].Filter.push(...modifications.rearSights);

    // Heat modifications
    saiga12K._props.HeatFactorByShot = 4;
    saiga12K._props.HeatFactor = 0.9;
  }

  private modifyBenelliM3(benelliM3: any): void {
    benelliM3._props.SingleFireRate = 850;
    benelliM3._props.bFirerate = 200;
    benelliM3._props.CanQueueSecondShot = true;
  }

  private addItemToSlots(item: any, slotIndices: number[], itemId: string): void {
    slotIndices.forEach(index => {
      item._props.Slots[index]._props.filters[0].Filter.push(itemId);
    });
  }

  private addNewCartridgesToShotguns(tables: any, itemHelper: ItemHelper): void {
    const items = Object.values(tables.templates.items);
    
    const shotgunMagazines = this.findCompatibleMagazines(items, itemHelper);
    const shotguns = this.findCompatibleShotguns(items, itemHelper);
    
    // Add cartridges to magazines
    this.addCartridgesToItems(tables.templates.items, shotgunMagazines, this.NEW_CARTRIDGE_IDS, 'Cartridges');
    
    // Add cartridges to shotguns
    this.addCartridgesToItems(tables.templates.items, shotguns, this.NEW_CARTRIDGE_IDS, 'Chambers');
    
    // Special cases
    this.addCartridgesToSpecialWeapons(tables.templates.items);
  }

  private findCompatibleMagazines(items: any[], itemHelper: ItemHelper): string[] {
    return items
      .filter(item => itemHelper.isOfBaseclass(item._id, BaseClasses.MAGAZINE))
      .filter(magazine => 
        magazine._props.Cartridges?.[0]?._props?.filters?.[0]?.Filter?.includes(this.ITEM_IDS.STANDARD_12G_BUCK)
      )
      .map(magazine => magazine._id);
  }

  private findCompatibleShotguns(items: any[], itemHelper: ItemHelper): string[] {
    return items
      .filter(item => itemHelper.isOfBaseclass(item._id, BaseClasses.SHOTGUN))
      .filter(shotgun => 
        shotgun?._props?.Chambers?.[0]?._props?.filters?.[0]?.Filter?.includes(this.ITEM_IDS.STANDARD_12G_BUCK)
      )
      .map(shotgun => shotgun._id);
  }

  private addCartridgesToItems(itemsDb: any, itemIds: string[], cartridgeIds: string[], containerType: string): void {
    itemIds.forEach(itemId => {
      const item = itemsDb[itemId];
      if (item?._props?.[containerType]?.[0]?._props?.filters?.[0]?.Filter) {
        item._props[containerType][0]._props.filters[0].Filter.push(...cartridgeIds);
      }
    });
  }

  private addCartridgesToSpecialWeapons(itemsDb: any): void {
    // MP43 variants
    [this.ITEM_IDS.MP43, this.ITEM_IDS.MP43_SAWED_OFF].forEach(weaponId => {
      const weapon = itemsDb[weaponId];
      [0, 1].forEach(chamberIndex => {
        this.NEW_CARTRIDGE_IDS.forEach(cartridgeId => {
          weapon._props.Chambers[chamberIndex]._props.filters[0].Filter.push(cartridgeId);
        });
      });
    });

    // MTS-255 cylinder
    const cylinder = itemsDb[this.ITEM_IDS.MTS_255_CYLINDER];
    for (let i = 0; i < 5; i++) {
      cylinder._props.Slots[i]._props.filters[0].Filter.push(...this.NEW_CARTRIDGE_IDS);
    }
  }

  private addCustomModifications(tables: any): void {
    const items = tables.templates.items;
    
    // KS-23M modifications
    this.modifyKS23(items[this.ITEM_IDS.KS23], items[this.ITEM_IDS.KS23_WIRE_STOCK]);
    
    // MP-153 modifications
    items[this.ITEM_IDS.MP153]._props.Slots[2]._props.filters[0].Filter.push("665b2ce3a592acfa0e1749b6");
    
    // Benelli M3 M-LOK handguard
    items[this.ITEM_IDS.BENELLI_M3]._props.Slots[1]._props.filters[0].Filter.push("665cd7bf309e1f1a84d7a39b");
    
    // Saiga-12K 30 shell magazine
    items[this.ITEM_IDS.SAIGA_12K]._props.Slots[7]._props.filters[0].Filter.push("67547b3da7233eec99aff92c");
  }

  private modifyKS23(ks23: any, ks23WireStock: any): void {
    // Add 6 shell magazine
    ks23._props.Slots[2]._props.filters[0].Filter.push("665a17431775fbd821da3298");
    
    // Add mount slot
    ks23._props.Slots.push(this.createSlot("mod_mount", "665c88a09a8a1cfbe59cd8d2", ["55d48a634bdc2d8b2f8b456a"]));
    
    // Wire stock modifications
    ks23WireStock._props.Prefab.path = "ks23stock.bundle";
    ks23WireStock._props.Slots.push(this.createSlot("mod_stock", "665b5c811722cdfd0a6e6dd5", ["5a0c59791526d8dba737bba7"]));
  }

  private createSlot(name: string, id: string, filters: string[]): any {
    return {
      _name: name,
      _id: id,
      _parent: this.ITEM_IDS.KS23,
      _props: {
        filters: [{
          Shift: 0,
          Filter: filters
        }]
      },
      _required: false,
      _mergeSlotWithChildren: false,
      _proto: "55d30c4c4bdc2db4468b457e"
    };
  }

  private loadCustomDatabase(container: DependencyContainer): void {
    const logger = container.resolve<ILogger>("WinstonLogger");
    const db = container.resolve<DatabaseServer>("DatabaseServer").getTables();
    const jsonUtil = container.resolve<JsonUtil>("JsonUtil");
    
    const modPath = path.resolve(__dirname, "../");
    const dbPath = path.join(modPath, "db");
    
    if (!fs.existsSync(dbPath)) {
      logger.warning(`${this.MOD_NAME}: Database folder not found at ${dbPath}`);
      return;
    }

    try {
      const customDb = this.loadRecursive(dbPath, jsonUtil, logger);
      this.processCustomItems(customDb, db, jsonUtil);
      this.processTraderAssorts(customDb, db);
      this.processLocalizations(customDb, db);
    } catch (error) {
      logger.error(`${this.MOD_NAME}: Error loading custom database: ${error}`);
    }
  }

  private loadRecursive(dirPath: string, jsonUtil: JsonUtil, logger: ILogger): any {
    const result: any = {};
    
    const loadDirectory = (currentPath: string, obj: any): void => {
      if (!fs.existsSync(currentPath)) return;
      
      fs.readdirSync(currentPath).forEach(file => {
        const fullPath = path.join(currentPath, file);
        const stat = fs.statSync(fullPath);
        
        if (stat.isDirectory()) {
          obj[file] = {};
          loadDirectory(fullPath, obj[file]);
        } else if (file.endsWith('.json')) {
          const fileName = file.replace('.json', '');
          try {
            obj[fileName] = jsonUtil.deserialize(fs.readFileSync(fullPath, 'utf8'));
          } catch (error) {
            logger.error(`${this.MOD_NAME}: Error loading ${fullPath}: ${error}`);
          }
        }
      });
    };
    
    loadDirectory(dirPath, result);
    return result;
  }

  private processCustomItems(customDb: any, db: any, jsonUtil: JsonUtil): void {
    if (!customDb.templates?.items) return;

    Object.keys(customDb.templates.items).forEach(itemFile => {
      const item = customDb.templates.items[itemFile];
      const handbook = customDb.templates.handbook?.[itemFile];
      
      if (item && handbook) {
        db.templates.items[item._id] = item;
        db.templates.handbook.Items.push({
          Id: item._id,
          ParentId: handbook.ParentId,
          Price: handbook.Price
        });
      }
    });
  }

  private processTraderAssorts(customDb: any, db: any): void {
    if (!customDb.traders?.assort) return;

    Object.keys(customDb.traders.assort).forEach(traderId => {
      const traderAssort = db.traders[traderId]?.assort;
      const customAssort = customDb.traders.assort[traderId];
      
      if (traderAssort && customAssort) {
        if (customAssort.items) traderAssort.items.push(...customAssort.items);
        if (customAssort.barter_scheme) Object.assign(traderAssort.barter_scheme, customAssort.barter_scheme);
        if (customAssort.loyal_level_items) Object.assign(traderAssort.loyal_level_items, customAssort.loyal_level_items);
      }
    });
  }

  private processLocalizations(customDb: any, db: any): void {
    if (!customDb.locales) return;

    const locales = db.locales.global;
    
    // Process default English localization
    if (customDb.locales.en) {
      Object.keys(locales).forEach(localeId => {
        this.processLocaleData(customDb.locales.en, locales[localeId]);
      });
    }

    // Process specific localizations
    Object.keys(customDb.locales).forEach(localeKey => {
      if (localeKey !== 'en' && locales[localeKey]) {
        this.processLocaleData(customDb.locales[localeKey], locales[localeKey]);
      }
    });
  }

  private processLocaleData(sourceLocale: any, targetLocale: any): void {
    if (sourceLocale.templates) {
      Object.keys(sourceLocale.templates).forEach(id => {
        const item = sourceLocale.templates[id];
        Object.keys(item).forEach(key => {
          targetLocale[`${id} ${key}`] = item[key];
        });
      });
    }

    if (sourceLocale.preset) {
      Object.keys(sourceLocale.preset).forEach(id => {
        const item = sourceLocale.preset[id];
        Object.keys(item).forEach(key => {
          targetLocale[id] = item[key];
        });
      });
    }
  }
}

module.exports = { mod: new Mod() };