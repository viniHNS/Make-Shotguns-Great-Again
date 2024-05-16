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

        // Find the Benelli M3 item by its Id
        const express = tables.templates.items["5d6e67fba4b9361bc73bc779"];

        // Adds Full-Auto fire mode to the Saiga12K
        saiga12K._props.weapFireType.push("fullauto");

        // buff the rate of fire of the full auto Saiga12K
        saiga12K._props.bFirerate = 500;

        // buff the rate of fire of the semi-auto Benelli M3
        benelliM3._props.SingleFireRate = 600;
        benelliM3._props.bFirerate = 100;

    }
}

module.exports = { mod: new Mod() }