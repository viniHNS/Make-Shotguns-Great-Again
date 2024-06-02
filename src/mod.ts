import { DependencyContainer } from "tsyringe";
import { IPostDBLoadMod } from "@spt-aki/models/external/IPostDBLoadMod";
import { DatabaseServer } from "@spt-aki/servers/DatabaseServer";
import { IPreAkiLoadMod } from "@spt-aki/models/external/IPreAkiLoadMod";
import { ILogger } from "@spt-aki/models/spt/utils/ILogger";
import { LogTextColor } from "@spt-aki/models/spt/logging/LogTextColor";
import { JsonUtil } from "@spt-aki/utils/JsonUtil";
import { VFS } from "@spt-aki/utils/VFS";
import { ImporterUtil } from "@spt-aki/utils/ImporterUtil";
import path from "path";

class Mod implements IPostDBLoadMod, IPreAkiLoadMod
{
    preAkiLoad(container: DependencyContainer): void 
    {
        // get the logger from the server container
        const logger = container.resolve<ILogger>("WinstonLogger");
        logger.logWithColor("[ViniHNS] Making the shotguns great again!", LogTextColor.GREEN);
    }

    public postDBLoad(container: DependencyContainer): void 
    {
        // get database from server
        const databaseServer = container.resolve<DatabaseServer>("DatabaseServer");

        // Get all the in-memory json found in /assets/database
        const tables = databaseServer.getTables();

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


        // QOL --------------------------------------------------------------------------------
        
        // Adds Full-Auto fire mode to the Saiga12K
        saiga12K._props.weapFireType.push("fullauto");

        // buff the rate of fire of the full auto Saiga12K
        saiga12K._props.bFirerate = 450;

        // buff the rate of fire of the semi-auto Benelli M3
        benelliM3._props.SingleFireRate = 850;
        benelliM3._props.bFirerate = 200;

        // -----------------------------------------------------------------------------------


        
        // Custom bundles --------------------------------------------------------------------

        //Add the 6 shell magazine to the KS-23M
        ks23._props.Slots[2]._props.filters[0].Filter.push("665a17431775fbd821da3298");
        ks23._props.Slots.push(
            {
                "_name": "mod_mount",
                "_id": "665c88a09a8a1cfbe59cd8d2",
                "_parent": "5e848cc2988a8701445df1e8",
                "_props": {
                  "filters": [
                    {
                      "Shift": 0,
                      "Filter": [
                        "55d48a634bdc2d8b2f8b456a"
                      ]
                    }
                  ]
                },
                "_required": false,
                "_mergeSlotWithChildren": false,
                "_proto": "55d30c4c4bdc2db4468b457e"
              }
        )

        ks23_wire_stock._props.Prefab.path = "ks23stock.bundle";
        ks23_wire_stock._props.Slots.push(
            {
                "_name": "mod_stock",
                "_id": "665b5c811722cdfd0a6e6dd5",
                "_parent": "5e848dc4e4dbc5266a4ec63d",
                "_props": {
                  "filters": [
                    {
                      "Shift": 0,
                      "Filter": [
                        "5a0c59791526d8dba737bba7"
                      ]
                    }
                  ]
                },
                "_required": false,
                "_mergeSlotWithChildren": false,
                "_proto": "55d30c4c4bdc2db4468b457e"
            }
        );

        //Add the 13 shell magazine to the MP-153
        mp153._props.Slots[2]._props.filters[0].Filter.push("665b2ce3a592acfa0e1749b6");

        // Add M-LOK handguard to the benelli M3
        benelliM3._props.Slots[1]._props.filters[0].Filter.push("665cd7bf309e1f1a84d7a39b");

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
        const modPath = path.resolve(__dirname.toString()).split(path.sep).join("/")+"/";

        const mydb = ImporterUtil.loadRecursive(`${modPath}../db/`);

        const itemPath = `${modPath}../db/templates/items/`;
        const handbookPath = `${modPath}../db/templates/handbook/`;

        for(const itemFile in mydb.templates.items) {
            const item = JsonUtil.deserialize(VFS.readFile(`${itemPath}${itemFile}.json`));
            const hb = JsonUtil.deserialize(VFS.readFile(`${handbookPath}${itemFile}.json`));

            const itemId = item._id;
            //logger.info(itemId);

            items[itemId] = item;
            //logger.info(hb.ParentId);
            //logger.info(hb.Price);
            handbook.push({
                "Id": itemId,
                "ParentId": hb.ParentId,
                "Price": hb.Price
            });
        }
        for (const trader in mydb.traders.assort) {
            const traderAssort = db.traders[trader].assort
            
            for (const item of mydb.traders.assort[trader].items) {
                traderAssort.items.push(item);
            }
    
            for (const bc in mydb.traders.assort[trader].barter_scheme) {
                traderAssort.barter_scheme[bc] = mydb.traders.assort[trader].barter_scheme[bc];
            }
    
            for (const level in mydb.traders.assort[trader].loyal_level_items) {
                traderAssort.loyal_level_items[level] = mydb.traders.assort[trader].loyal_level_items[level];
            }
        }
        //logger.info("Test");
        // default localization
        for (const localeID in locales)
        {
            for (const id in mydb.locales.en.templates) {
                const item = mydb.locales.en.templates[id];
                //logger.info(item);
                for(const locale in item) {
                    //logger.info(locale);
                    //logger.info(item[locale]);
                    //logger.info(`${id} ${locale}`);
                    locales[localeID][`${id} ${locale}`] = item[locale];
                }
            }

            for (const id in mydb.locales.en.preset) {
                const item = mydb.locales.en.preset[id];
                for(const locale in item) {
                    //logger.info(`${id} ${locale}`);
                    locales[localeID][`${id}`] = item[locale];
                }
            }
        }

        for (const localeID in mydb.locales)
        {
            for (const id in mydb.locales[localeID].templates) {
                const item = mydb.locales[localeID].templates[id];
                //logger.info(item);
                for(const locale in item) {
                    locales[localeID][`${id}`] = item[locale];
                }
            }

            for (const id in mydb.locales[localeID].preset) {
                const item = mydb.locales[localeID].preset[id];
                for(const locale in item) {
                    //logger.info(`${id} ${locale}`);
                    locales[localeID][`${id} ${locale}`] = item[locale];
                }
                
            }

        }

    }
}

module.exports = { mod: new Mod() }