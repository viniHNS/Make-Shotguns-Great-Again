import { DependencyContainer } from "tsyringe";
import { IPostDBLoadMod } from "@spt-aki/models/external/IPostDBLoadMod";
import { DatabaseServer } from "@spt-aki/servers/DatabaseServer";
import { IPreAkiLoadMod } from "@spt-aki/models/external/IPreAkiLoadMod";
import { ILogger } from "@spt-aki/models/spt/utils/ILogger";
import { LogTextColor } from "@spt-aki/models/spt/logging/LogTextColor";

class Mod implements IPostDBLoadMod, IPreAkiLoadMod
{
    preAkiLoad(container: DependencyContainer): void 
    {
        // get the logger from the server container
        const logger = container.resolve<ILogger>("WinstonLogger");
        logger.logWithColor("Making the VPO-101 great again!", LogTextColor.GREEN);
    }

    public postDBLoad(container: DependencyContainer): void 
    {
        // get database from server
        const databaseServer = container.resolve<DatabaseServer>("DatabaseServer");

        // Get all the in-memory json found in /assets/database
        const tables = databaseServer.getTables();

        // Find the VPO-101 item by its Id
        const vpo101 = tables.templates.items["5c501a4d2e221602b412b540"];

        // All muzzle devices 7.62x51 (I hope)
        const items = ["5a34fd2bc4a282329a73b4c5", "5dfa3cd1b33c0951220c079b", "6130c43c67085e45ef1405a1", "5c7954d52e221600106f4cc7", "59bffc1f86f77435b128b872", "628a66b41d5e41750e314f34", "612e0e3c290d254f5e6b291d", "612e0d3767085e45ef14057f", "615d8eb350224f204c1da1cf", "607ffb988900dc2d9a55b6e4", "6065c6e7132d4d12c81fd8e1", "5c878e9d2e2216000f201903", "5bbdb8bdd4351e4502011460", "5b7d693d5acfc43bca706a3d", "5d026791d7ad1a04a067ea63", "5cdd7685d7f00c000f260ed2", "5cdd7693d7f00c0010373aa5", "5cf78720d7f00c06595bc93e", "5d02677ad7ad1a04a15c0f95", "5d1f819086f7744b355c219b", "5d443f8fa4b93678dd4a01aa", "5dfa3cd1b33c0951220c079b", "5fbc22ccf24b94483f726483", "5fbe7618d6fa9c00c571bb6c"]

        for (let i = 0; i < items.length; i++)
        {
            vpo101._props.Slots[6]._props.filters[0].Filter.push(items[i])
        }
        
        vpo101._props.Ergonomics = 30;

    }
}

module.exports = { mod: new Mod() }