import { DependencyContainer } from "tsyringe";
import { IPostDBLoadMod } from "@spt/models/external/IPostDBLoadMod";
import { DatabaseServer } from "@spt/servers/DatabaseServer";
import { IPreSptLoadMod } from "@spt/models/external/IpreSptLoadMod";
import { ILogger } from "@spt/models/spt/utils/ILogger";
import { CustomItemService } from "@spt/services/mod/CustomItemService";
import { NewItemFromCloneDetails } from "@spt/models/spt/mod/NewItemDetails";
import { LogTextColor } from "@spt/models/spt/logging/LogTextColor";
import { ItemHelper } from "@spt/helpers/ItemHelper";
import { BaseClasses } from "@spt/models/enums/BaseClasses";
import { JsonUtil } from "@spt/utils/JsonUtil";
import { VFS } from "@spt/utils/VFS";
import { ImporterUtil } from "@spt/utils/ImporterUtil";
import path from "path";

class Mod implements IPostDBLoadMod, IPreSptLoadMod {
  preSptLoad(container: DependencyContainer): void {
    // get the logger from the server container
    const logger = container.resolve<ILogger>("WinstonLogger");
    logger.logWithColor(
      "[ViniHNS] Making the shotguns great again!",
      LogTextColor.GREEN
    );
  }

  public postDBLoad(container: DependencyContainer): void {
    // get database from server
    const databaseServer = container.resolve<DatabaseServer>("DatabaseServer");

    // Resolve the CustomItemService container
    const CustomItem =
      container.resolve<CustomItemService>("CustomItemService");

    const itemHelper = container.resolve<ItemHelper>("ItemHelper");

    // Get all the in-memory json found in /assets/database
    const tables = databaseServer.getTables();

    const achievements = tables.templates.achievements;

    // Find the Saiga12K item by its Id
    const saiga12K = tables.templates.items["576165642459773c7a400233"];

    // Find the Benelli M3 item by its Id
    const benelliM3 = tables.templates.items["6259b864ebedf17603599e88"];

    // Find the KS-23M item by its Id
    const ks23 = tables.templates.items["5e848cc2988a8701445df1e8"];

    const ks23_wire_stock = tables.templates.items["5e848dc4e4dbc5266a4ec63d"];

    // Find the MP-153 item by its Id
    const mp153 = tables.templates.items["56dee2bdd2720bc8328b4567"];

    // Find the MP-133 item by its Id
    const mp133 = tables.templates.items["54491c4f4bdc2db1078b4568"];

    const mp12 = tables.templates.items["67537f2e72cb0015b8512669"];

    const mp43 = tables.templates.items["5580223e4bdc2d1c128b457f"];

    const mp43_sawed_off = tables.templates.items["64748cb8de82c85eaf0a273a"];

    function createClonedItem(details: NewItemFromCloneDetails) {
      CustomItem.createItemFromClone(details);
    }

    // QOL --------------------------------------------------------------------------------

    // Adds Full-Auto fire mode to the Saiga12K
    saiga12K._props.weapFireType = ["single", "fullauto"];

    // buff the rate of fire of the full auto Saiga12K
    saiga12K._props.bFirerate = 400;

    const saiga12K_handguards_to_add = [
      "5b800e9286f7747a8b04f3ff",
      "5b80242286f77429445e0b47",
      "647dd2b8a12ebf96c3031655",
      "5c17664f2e2216398b5a7e3c",
      "5d2c829448f0353a5c7d6674",
      "5efaf417aeb21837e749c7f2",
    ];

    const saiga12k_dustcover_to_add = [
      "5d2c772c48f0355d95672c25",
      "5d2c770c48f0354b4a07c100",
      "5d2c76ed48f03532f2136169",
      "5649af884bdc2d1b2b8b4589",
    ];

    const saiga12k_rear_sight_to_add = ["5649d9a14bdc2d79388b4580"];

    saiga12K._props.Slots[1]._props.filters[0].Filter.push(
      ...saiga12K_handguards_to_add
    );
    saiga12K._props.Slots[4]._props.filters[0].Filter.push(
      ...saiga12k_dustcover_to_add
    );
    saiga12K._props.Slots[5]._props.filters[0].Filter.push(
      ...saiga12k_rear_sight_to_add
    );

    saiga12K._props.HeatFactorByShot = 4;
    saiga12K._props.HeatFactor = 0.9;

    // buff the rate of fire of the semi-auto Benelli M3
    benelliM3._props.SingleFireRate = 850;
    benelliM3._props.bFirerate = 200;
    benelliM3._props.CanQueueSecondShot = true;

    // Adding MP-12 to the primary and secondary slots
    tables.templates.items[
      "55d7217a4bdc2d86028b456d"
    ]._props.Slots[0]._props.filters[0].Filter.push("67537f2e72cb0015b8512669");

    // Adding new cartridges to the guns
    let magazine_shotguns_to_add_new_cartidges = [];

    let shotguns_to_add_new_cartidges = [];

    const newCartridgeIds = [
      "6756378c3825f690a72789bf",
      "675766d7a045de209488e750",
      "67579112e55c9b7479e5d9ea",
      "6758e4a30db075f8dc01bb17",
    ];

    const item = Object.values(tables.templates.items);

    const magazines = item.filter(x => itemHelper.isOfBaseclass(x._id, BaseClasses.MAGAZINE));
    const shotguns = item.filter(x => itemHelper.isOfBaseclass(x._id, BaseClasses.SHOTGUN));

    for (const magazine of magazines) {
      if (magazine._props.Cartridges[0]._props.filters[0].Filter.includes("560d5e524bdc2d25448b4571")) {
        magazine_shotguns_to_add_new_cartidges.push(magazine._id);
      }
    }
    
    for (const shotgun of shotguns) {
      if (shotgun?._props?.Chambers[0]?._props?.filters[0]?.Filter?.includes("560d5e524bdc2d25448b4571")) {
        shotguns_to_add_new_cartidges.push(shotgun._id);  
      }
    }
    

    magazine_shotguns_to_add_new_cartidges.forEach((element) => {
      const item = tables.templates.items[element];
      newCartridgeIds.forEach((cartridgeId) => {
        item._props.Cartridges[0]._props.filters[0].Filter.push(cartridgeId);
      });
    });

    shotguns_to_add_new_cartidges.forEach((element) => {
      const item = tables.templates.items[element];
      newCartridgeIds.forEach((cartridgeId) => {
        item._props.Chambers[0]._props.filters[0].Filter.push(cartridgeId);
      });
    });

    [mp43, mp43_sawed_off].forEach((weapon) => {
      [0, 1].forEach((chamberIndex) => {
        newCartridgeIds.forEach((cartridgeId) => {
          weapon._props.Chambers[chamberIndex]._props.filters[0].Filter.push(
            cartridgeId
          );
        });
      });
    });

    // -----------------------------------------------------------------------------------

    // Custom bundles --------------------------------------------------------------------

    //Add the 6 shell magazine to the KS-23M
    ks23._props.Slots[2]._props.filters[0].Filter.push(
      "665a17431775fbd821da3298"
    );
    ks23._props.Slots.push({
      _name: "mod_mount",
      _id: "665c88a09a8a1cfbe59cd8d2",
      _parent: "5e848cc2988a8701445df1e8",
      _props: {
        filters: [
          {
            Shift: 0,
            Filter: ["55d48a634bdc2d8b2f8b456a"],
          },
        ],
      },
      _required: false,
      _mergeSlotWithChildren: false,
      _proto: "55d30c4c4bdc2db4468b457e",
    });

    ks23_wire_stock._props.Prefab.path = "ks23stock.bundle";
    ks23_wire_stock._props.Slots.push({
      _name: "mod_stock",
      _id: "665b5c811722cdfd0a6e6dd5",
      _parent: "5e848dc4e4dbc5266a4ec63d",
      _props: {
        filters: [
          {
            Shift: 0,
            Filter: ["5a0c59791526d8dba737bba7"],
          },
        ],
      },
      _required: false,
      _mergeSlotWithChildren: false,
      _proto: "55d30c4c4bdc2db4468b457e",
    });

    //Add the 13 shell magazine to the MP-153
    mp153._props.Slots[2]._props.filters[0].Filter.push(
      "665b2ce3a592acfa0e1749b6"
    );

    // Add M-LOK handguard to the benelli M3
    benelliM3._props.Slots[1]._props.filters[0].Filter.push(
      "665cd7bf309e1f1a84d7a39b"
    );

    // Add the 30 shell magazine to the SAIGA-12K
    saiga12K._props.Slots[7]._props.filters[0].Filter.push(
      "67547b3da7233eec99aff92c"
    );

    // Add achivements
    achievements["6759d53e10acb02163a6c905"] = {
      id: "6759d53e10acb02163a6c905",
      imageUrl: "/files/achievement/Standard_22.png",
      assetPath: "",
      rewards: [],
      conditions: {
        availableForFinish: [
          {
            id: "6759d55b7a650f8973ed15a2",
            index: 0,
            dynamicLocale: false,
            visibilityConditions: [],
            globalQuestCounterId: "",
            parentId: "",
            value: 1,
            type: "Completion",
            oneSessionOnly: false,
            completeInSeconds: 0,
            doNotResetIfCounterCompleted: false,
            isResetOnConditionFailed: false,
            isNecessary: false,
            counter: {
              id: "6759d56909423033706706c2",
              conditions: [
                {
                  id: "6759d56e45f57a715ce0b215",
                  dynamicLocale: false,
                  target: "Savage",
                  compareMethod: ">=",
                  value: 1,
                  weapon: [
                    "6758e4a30db075f8dc01bb17",
                    
                  ],
                  distance: {
                    value: 0,
                    compareMethod: ">=",
                  },
                  weaponModsInclusive: [],
                  weaponModsExclusive: [],
                  enemyEquipmentInclusive: [],
                  enemyEquipmentExclusive: [],
                  weaponCaliber: [],
                  savageRole: [],
                  bodyPart: [],
                  daytime: {
                    from: 0,
                    to: 0,
                  },
                  enemyHealthEffects: [],
                  resetOnSessionEnd: false,
                  conditionType: "Kills",
                },
              ],
            },
            conditionType: "CounterCreator",
          },
        ],
        fail: [],
      },
      instantComplete: false,
      showNotificationsInGame: false,
      showProgress: false,
      prefab: "",
      rarity: "Common",
      hidden: false,
      showConditions: false,
      progressBarEnabled: true,
      side: "Pmc",
      index: 9994,
    };


    // -----------------------------------------------------------------------------------

    // Thanks TRON <3
    const logger = container.resolve<ILogger>("WinstonLogger");
    const db = container.resolve<DatabaseServer>("DatabaseServer").getTables();
    const ImporterUtil = container.resolve<ImporterUtil>("ImporterUtil");
    const JsonUtil = container.resolve<JsonUtil>("JsonUtil");
    const VFS = container.resolve<VFS>("VFS");
    const locales = db.locales.global;
    const items = db.templates.items;
    const handbook = db.templates.handbook.Items;
    const modPath = path.resolve(__dirname.toString()).split(path.sep).join("/") + "/";
    const mydb = ImporterUtil.loadRecursive(`${modPath}../db/`);

    const itemPath = `${modPath}../db/templates/items/`;
    const handbookPath = `${modPath}../db/templates/handbook/`;

    for (const itemFile in mydb.templates.items) {
      const item = JsonUtil.deserialize(
        VFS.readFile(`${itemPath}${itemFile}.json`)
      );
      const hb = JsonUtil.deserialize(
        VFS.readFile(`${handbookPath}${itemFile}.json`)
      );

      const itemId = item._id;
      //logger.info(itemId);

      items[itemId] = item;
      //logger.info(hb.ParentId);
      //logger.info(hb.Price);
      handbook.push({
        Id: itemId,
        ParentId: hb.ParentId,
        Price: hb.Price,
      });
    }
    for (const trader in mydb.traders.assort) {
      const traderAssort = db.traders[trader].assort;

      for (const item of mydb.traders.assort[trader].items) {
        traderAssort.items.push(item);
      }

      for (const bc in mydb.traders.assort[trader].barter_scheme) {
        traderAssort.barter_scheme[bc] =
          mydb.traders.assort[trader].barter_scheme[bc];
      }

      for (const level in mydb.traders.assort[trader].loyal_level_items) {
        traderAssort.loyal_level_items[level] =
          mydb.traders.assort[trader].loyal_level_items[level];
      }
    }
    //logger.info("Test");
    // default localization
    for (const localeID in locales) {
      for (const id in mydb.locales.en.templates) {
        const item = mydb.locales.en.templates[id];
        //logger.info(item);
        for (const locale in item) {
          //logger.info(locale);
          //logger.info(item[locale]);
          //logger.info(`${id} ${locale}`);
          locales[localeID][`${id} ${locale}`] = item[locale];
        }
      }

      for (const id in mydb.locales.en.preset) {
        const item = mydb.locales.en.preset[id];
        for (const locale in item) {
          //logger.info(`${id} ${locale}`);
          locales[localeID][`${id}`] = item[locale];
        }
      }
    }

    for (const localeID in mydb.locales) {
      for (const id in mydb.locales[localeID].templates) {
        const item = mydb.locales[localeID].templates[id];
        //logger.info(item);
        for (const locale in item) {
          locales[localeID][`${id}`] = item[locale];
        }
      }

      for (const id in mydb.locales[localeID].preset) {
        const item = mydb.locales[localeID].preset[id];
        for (const locale in item) {
          //logger.info(`${id} ${locale}`);
          locales[localeID][`${id} ${locale}`] = item[locale];
        }
      }
    }
    // add the winchester 00 buckshot to the MP-12

    logger.logWithColor(
      "[Making the shotguns great again!] Database: Loading complete.",
      LogTextColor.GREEN
    );
  }
}

module.exports = { mod: new Mod() };
