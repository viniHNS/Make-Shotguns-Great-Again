<div align="center">

# Make Shotguns Great Again!

Shotguns done right for SPT 4.1.6: better vanilla shotguns, new guns, attachments and ammo, Dragon's Breath sparks, and a malfunction overhaul.

![Version](https://img.shields.io/badge/version-1.15.0-orange?style=flat)
![SPT](https://img.shields.io/badge/SPT-4.1.6-blue?style=flat)
![WTT-CommonLib](https://img.shields.io/badge/WTT--CommonLib-3.0.6-purple?style=flat)
![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=flat&logo=dotnet)
![License](https://img.shields.io/badge/license-MIT-green?style=flat)

[Features](#features) · [Install](#install) · [New Guns](#new-guns) · [New Ammo](#new-ammo) · [Configuration](#configuration) · [Build](#build-from-source)

**English** · [Português](README_BR.md)

![fire](https://media1.giphy.com/media/v1.Y2lkPTc5MGI3NjExcTZiZzdxeXJ0eTBxNWNtbHVxdzBweWkxbjhiNnJoc2ZiOTh4a3VzZyZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/u1aUNE2xRmk3xUaSAJ/giphy.gif)

</div>

---

## Features

**Vanilla shotguns**

- **Benelli M3**: semi-auto mode tuned to match the other semi-auto shotguns (MP-153, MP-155).
- **Saiga-12K**: takes most AK handguards.
- **KS-23M**: takes the Kiba Arms SPRM rail mount and the AK GP-25 recoil pad.
- **MTs-255**: takes the ETMI-019 and Kiba Arms SPRM rail mounts.
- **Mossberg 590A1**: takes the Kiba Arms SPRM rail mount and a new [threaded barrel](#new-attachments) for chokes and suppressors.
- **ETMI-019 and Kiba Arms SPRM**: fit more optics and mounts.
- **AA-12**: higher rate of fire.
- **Barrikada slug**: accuracy raised to match the other 12ga slugs.

**Client**

- **Skip inspection before clearing**: clear a malfunction right away with the clear key, without inspecting the weapon first.
- **No forced boss malfunctions**: bosses like Kollontay can't force a jam on your weapon. Jams only come from weapon condition and ammo.
- **Dragon's Breath effect**: firing 12/70 'Hellfire' spawns incendiary sparks at the muzzle.
- **KS-23 optics fix**: optics on the KS-23 rail now hit where the reticle points.
- **Buckshot from any gun**: buckshot fired from non-shotguns (like the 5.45x39 Svalka from an AK) spreads its pellets instead of hitting a single point.

**New content**

- 4 [guns](#new-guns), 8 [attachments](#new-attachments) and 11 [ammo items](#new-ammo), sold by traders and craftable in the Workbench.
- Scavs can spawn with the MP-12 and MP-700 and load FRAG-12, Hellfire and .700 Nitro rounds. Tagilla can load Hellfire.

---

## Install

Install [WTT-CommonLib](https://github.com/GrooveypenguinX/WTT-CommonLib) **3.0.6** or newer (3.0.x) first. Then extract `makeshotgunsgreatagain.zip` into your SPT game folder:

```
<game folder>/
├── BepInEx/plugins/makeshotgunsgreatagain.dll
└── SPT_Runtime/user/mods/makeshotgunsgreatagain/
    ├── makeshotgunsgreatagain.dll
    ├── bundles.json
    ├── bundles/            models of the new items
    ├── config/config.json
    └── db/                 items, presets, trader offers, crafts and locales
```

Both parts are required: the server adds the items and the client handles the effects and fixes.

> The new items stay in your profile. Removing the mod after buying or looting them breaks the profile.

---

## New Guns

| Gun | Caliber | Sold by |
|---|---|---|
| MP-12 12g single-shot rifle | 12/70 | Prapor LL1 (6,412 ₽). Jaeger LL1 sells the **wood** (6,875 ₽) and **polymer** (7,103 ₽) builds |
| MP-700 .700 Nitro Express Double Rifle | .700 Nitro Express | Jaeger LL3 (129,564 ₽). Full build at Jaeger LL2 (195,632 ₽) |
| MP-700 Shorty .700 Nitro Express Double Rifle | .700 Nitro Express | Jaeger LL3 (145,000 ₽). Full build at Jaeger LL3 (215,746 ₽) |
| ZiD SP-81 23x75 pistol | 23x75 | Mechanic LL2 (65,321 ₽). A signal pistol rebuilt for 23x75 rounds; it can't fire flares |

The MP-700 Shorty is a sawed-off MP-700: no stock, short barrel, brutal recoil.

### New Attachments

| Attachment | Sold by |
|---|---|
| Mossberg 590A1 12ga 508mm threaded barrel | Mechanic LL2 (15,500 ₽) |
| KS-23M 23x75 6-shell magazine | Mechanic LL2 (4,600 ₽) |
| Benelli M3 Keymod Handguard | Mechanic LL2 (8,013 ₽) |
| MP-153 12ga competition 13-shell magazine | Mechanic LL3 (6,003 ₽) |
| SOK-12 12/76 Alliance Armament 30-round magazine | Mechanic LL3 (27,996 ₽) |
| MP-12 12ga 600mm barrel | Prapor LL1 (2,134 ₽) |
| MP-700 .700 Nitro Express Double Rifle 725mm Barrel | Jaeger LL3 (29,568 ₽) |
| MP-700 Shorty .700 Nitro Express 310mm Barrel | Jaeger LL3 (31,000 ₽) |

The 590A1 threaded barrel takes the same muzzle devices as the MP-153.

---

## New Ammo

| Ammo | Sold by | Workbench |
|---|---|---|
| 12/70 Magnum Express Kinghunter | Peacekeeper LL2 ($2) | Level 2 |
| 12/70 Flechette Kinghunter | Peacekeeper LL3 ($3). Pack of 25 at LL2 for a Portable Powerbank | Level 2 |
| 12/70 armor-piercing Slug "SVAROG" | Peacekeeper LL3 ($5). Pack of 5 at LL2 for a Diary | Level 3 |
| 12/70 'FRAG-12' | Peacekeeper LL4 ($106) | Level 3 |
| 12/70 'Hellfire' hybrid buckshot | Jaeger LL2 (1,236 ₽) | Level 3 |
| 12/70 Winchester Super-X 00 buckshot | Jaeger LL2 (76 ₽) | Level 2 |
| 12/70 7mm Buckshot Brass Case | Jaeger LL2 (44 ₽) | Level 1 |
| .700 Nitro Express FMJ | Jaeger LL3 (1,930 ₽) | Level 3 |
| 5.45x39mm 'Svalka' Anti-Drone Buckshot | — | Level 1 |

### Workbench Crafts

| Product | Qty | Level | Time | Ingredients | Tools |
|---|---|---|---|---|---|
| 5.45x39mm 'Svalka' | 30 | 1 | 1 h | 30x 5.45x39mm SP, 5x 12/70 8.5mm Magnum buckshot, 1x Disposable syringe | Leatherman Multitool |
| 12/70 7mm Buckshot Brass Case | 60 | 1 | 2 h | 1x Horse figurine, 1x Gunpowder "Kite" | Pliers Elite |
| 12/70 Magnum Express Kinghunter | 50 | 2 | 2 h 20 min | 2x Geiger-Muller counter, 1x Gunpowder "Hawk" | Pliers |
| 12/70 Flechette Kinghunter | 40 | 2 | 3 h | 4x Pack of nails, 1x Gunpowder "Eagle", 1x Gunpowder "Kite", 40x 12/70 7mm buckshot | Round pliers, Flat screwdriver |
| 12/70 Winchester Super-X 00 buckshot | 40 | 2 | 1 h | 1x Gunpowder "Eagle", 2x D Size battery | Pliers |
| 12/70 armor-piercing Slug "SVAROG" | 35 | 3 | 3 h | 1x Gunpowder "Eagle", 3x Spark plug | Screwdriver |
| 12/70 'Hellfire' hybrid buckshot | 40 | 3 | 4 h | 1x Gunpowder "Hawk", 1x Can of thermite, 1x Classic matches, 1x Hunting matches | Leatherman Multitool |
| 12/70 'FRAG-12' | 15 | 3 | 5 h 30 min | 1x Gunpowder "Eagle", 2x 40mm VOG-25 grenade, 1x Metal spare parts | Pliers, Screwdriver |
| .700 Nitro Express FMJ | 20 | 3 | 5 h | 3x Gunpowder "Eagle", 1x Weapon parts, 1x Military cable | Pliers |

---

## Configuration

### Client (`F12` menu)

| Section | Setting | Default | Description |
|---|---|---|---|
| Malfunctions | Skip Inspection Before Clearing | `true` | Clear malfunctions without inspecting the weapon first |
| Malfunctions | Remove Boss Forced Malfunctions | `true` | Stop bosses from forcing a malfunction on your weapon |
| Dragon Breath | Trails Enabled | `true` | Burning streaks behind the sparks |
| Dragon Breath | Collision Enabled | `true` | Sparks bounce off walls |
| Dragon Breath | Lights Enabled | `true` | Sparks light up their surroundings |
| Dragon Breath | Max Particles | `400` | Sparks alive at the same time (50–800) |
| Dragon Breath | Particles Per Shot | `100` | Sparks per shot (20–300) |
| Dragon Breath | Effect Duration | `4` | Seconds before the effect is removed (1–8) |
| Dragon Breath | Spread Angle | `4` | Width of the spark cone, in degrees (1–45) |
| Dragon Breath | Noise Strength | `6` | How much the sparks swirl (0–10) |
| Dragon Breath | Noise Frequency | `5` | How fast the swirl changes (0–10) |
| Dragon Breath | Air Resistance | `0.3` | How fast the sparks slow down (0–1) |
| Dragon Breath | Start Offset | `0.2` | Distance from the muzzle where the effect starts, in meters (0–1) |
| KS-23 Mount Calibration | Enable Mount Alignment Fix | `true` | Correct shots from optics on the KS-23 rail |
| KS-23 Mount Calibration | Shot Pitch Correction | `0.311` | Vertical correction, in degrees |
| KS-23 Mount Calibration | Shot Yaw Correction | `0.31` | Horizontal correction, in degrees |
| KS-23 Mount Calibration | Debug Logging | `false` | Log each correction to the BepInEx console |

Turning off trails, collision or lights, or lowering the particle counts, makes the Dragon's Breath effect lighter on performance.

### Server (`config/config.json`)

The file is in `SPT_Runtime/user/mods/makeshotgunsgreatagain/config/`. Restart the server after editing it.

| Field | Default | Description |
|---|---|---|
| `enableBotsUseFrag12` | `true` | Bots can load FRAG-12 rounds |
| `enableBotsUseDragonBreath` | `true` | Bots can load Hellfire (Dragon's Breath) rounds |
| `enableDebugLogs` | `false` | Log the mod's changes to the server console |

---

## Build from Source

**Requirements:** .NET 10 SDK and an SPT 4.1.6 install (the client project references the game's DLLs).

```sh
dotnet build makeshotgunsgreatagain.sln -c Release
```

The server project builds the client first and, in Release, creates `makeshotgunsgreatagain.zip` in the solution folder with both DLLs, `config/`, `db/`, `bundles/` and `bundles.json`.

> The client `.csproj` points at `D:\Jogos\SPT4.1` for its references (override with `-p:SptGameDir=...`), and both projects copy their build output into that install for testing. Change `SptModsDir` to your own SPT folder too. Close the SPT server before building, or the copy fails because the server DLL is in use.

### Project Structure

```
Make-Shotguns-Great-Again/
├── makeshotgunsgreatagain.sln
├── Server/                             .NET 10 server mod
│   ├── makeshotgunsgreatagain.cs       metadata, WTT loading and changes to vanilla items
│   ├── ModConfig.cs                    config.json model
│   ├── config/config.json
│   ├── bundles.json                    bundle list for the new items
│   ├── bundles/                        models of the new items
│   └── db/
│       ├── CustomItems/                new guns, attachments and ammo
│       ├── CustomAssortSchemes/        Peacekeeper ammo pack barters
│       ├── CustomHideoutRecipes/       Workbench crafts
│       ├── CustomLocales/              names of the MP-12 builds
│       └── weaponPresets/
│           ├── Assorts/                full builds sold by Jaeger
│           ├── BotLoadouts/            new guns and ammo for bots
│           └── GlobalPresets/          MP-12 default and polymer builds
└── Client/                             BepInEx plugin (netstandard2.1)
    ├── makeshotgunsgreatagain.cs       F12 settings
    └── Patches/
        ├── BuckshotDispersionPatch.cs                     pellet spread from non-shotguns
        ├── CanResolveMalfunctionsWithoutInspectionPatch.cs
        ├── DragonBreathMuzzlePatch.cs                     Dragon's Breath sparks
        ├── DragonBreathPatch.cs
        ├── KS23MountAlignmentPatch.cs                     KS-23 optics fix
        └── RemoveBossMalfunctionsPatch.cs
```

---

## Credits

- 12/70 Winchester Super-X 00 buckshot model: [Guy in a Poncho](https://sketchfab.com/ponchoguy)
- 12/70 AP Slug SVAROG, Flechette Kinghunter and Magnum Express Kinghunter models: [Deadcomrade](https://sketchfab.com/deadcomrade)
- Dragon's Breath particle effect: based on **jankytheclown**'s work in [HollywoodFX](https://github.com/SleepingPills/HollywoodFX)

---

## Resources

| Resource | URL |
|---|---|
| WTT-CommonLib | https://github.com/GrooveypenguinX/WTT-CommonLib |
| SPT Server C# | https://github.com/SP-Tushonka/server-csharp |
| Server Mod Examples | https://github.com/SP-Tushonka/server-mod-examples |
| SPT Wiki — Modding Resources | https://wiki.sp-tushonka.com/en/modding/Modding_Resources |
| SPT Scaffold | https://github.com/viniHNS/spt-scaffold |
