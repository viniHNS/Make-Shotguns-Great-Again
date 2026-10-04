<div align="center">

# Make Shotguns Great Again!

Shotguns done right for SPT 4.1.6: better vanilla shotguns, new guns, attachments and ammo, Dragon's Breath sparks, and a malfunction overhaul.

![Version](https://img.shields.io/badge/version-1.17.0-orange?style=flat)
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

**Vanilla guns**

- **Benelli M3**: semi-auto mode tuned to match the other semi-auto shotguns (MP-153, MP-155).
- **Saiga-12K**: takes most AK handguards.
- **KS-23M**: takes the Kiba Arms SPRM rail mount and the AK GP-25 recoil pad.
- **MTs-255**: takes the ETMI-019 and Kiba Arms SPRM rail mounts.
- **Mossberg 590A1**: takes the Kiba Arms SPRM rail mount and a new [threaded barrel](#new-attachments) for chokes and suppressors.
- **MP-18 (7.62x54R)**: takes a new [threaded barrel](#new-attachments) for muzzle brakes and suppressors.
- **ETMI-019 and Kiba Arms SPRM**: fit more optics and mounts.
- **AA-12**: higher rate of fire.
- **Barrikada slug**: accuracy raised to match the other 12ga slugs.

**Client**

- **Skip inspection before clearing**: clear a malfunction right away with the clear key, without inspecting the weapon first.
- **No forced boss malfunctions**: bosses like Kollontay can't force a jam on your weapon. Jams only come from weapon condition and ammo.
- **Dragon's Breath effect**: firing 12/70 'Hellfire' spawns incendiary sparks at the muzzle.
- **KS-23 optics fix**: optics on the KS-23 rail now hit where the reticle points.
- **MP-18 suppressed sound**: a suppressed MP-18 now sounds suppressed. The game had the sound in the wrong slot of the weapon.
- **Buckshot from any gun**: buckshot fired from non-shotguns (like the 5.45x39 Svalka from an AK) spreads its pellets instead of hitting a single point.

**New content**

- 5 [guns](#new-guns), 15 [attachments](#new-attachments) and 14 [ammo items](#new-ammo), available through traders and Workbench crafts.
- Six quests: three Skier jobs unlock the VR80 builds, and Jaeger's Hunter's Dream unlocks the new .700 Nitro loads.
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
| MP-12 12g single-shot rifle | 12/70 | Jaeger LL1 sells the **wood** (6,875 ₽) and **polymer** (7,103 ₽) builds |
| MP-700 .700 Nitro Express Double Rifle | .700 Nitro Express | Jaeger LL3 (129,564 ₽). Full build at Jaeger LL2 (195,632 ₽) |
| MP-700 Shorty .700 Nitro Express Double Rifle | .700 Nitro Express | Jaeger LL3 (145,000 ₽). Full build at Jaeger LL3 (215,746 ₽) |
| ZiD SP-81 23x75 pistol | 23x75 | Mechanic LL2 (65,321 ₽). A signal pistol rebuilt for 23x75 rounds; it can't fire flares |
| Rock Island Armory VR80 12ga semi-automatic shotgun | 12/70 | Skier LL1 barter after Foreign Stock; LL2 purchase (110,000 ₽) after Repeat Business. Factory build with a 5-round magazine |

The MP-700 Shorty is a sawed-off MP-700: no stock, short barrel, brutal recoil.

The VR80 is a gas-operated semi-automatic shotgun with AR-style controls, a 508mm barrel and detachable 5- and 10-round box magazines. Its factory M-LOK handguard accepts compatible grips and accessory mounts; the top rail accepts optics. The factory stock includes the pistol grip. The factory build weighs 3.35 kg with an empty 5-round magazine.

After **Foreign Stock**, Skier LL1 trades the factory VR80 for one Dan Jackiel whiskey, one Tarkovskaya vodka and two Wilston cigarettes (one weapon per restock). Complete **Repeat Business** at Skier LL2 to unlock the 110,000 ₽ cash purchase.

Skier LL2 also trades the **VR80 Silent** build after the independent quest **Quiet Arrangement** (one per restock): the threaded barrel with a SilencerCo Salvo 12 suppressor, an EOTech XPS3-2 sight, a Magpul AFG grip and the 10-round magazine, for a Roler Submariner, a Wooden clock, a Golden neck chain and one Horse figurine.

| Skier quest | Requirements and objectives | Unlock |
| --- | --- | --- |
| Foreign Stock | LL1; eliminate 5 Scavs on Customs with any 12ga shotgun and hand over 2 Wilston cigarettes found in raid | Factory VR80 barter at LL1 |
| Repeat Business | LL2 and Foreign Stock completed; eliminate 10 Scavs on Customs with the VR80 | Factory VR80 cash purchase at LL2 |
| Quiet Arrangement | LL2, independent of the other two; eliminate 8 Scavs on Customs and 3 PMCs anywhere with any suppressed 12ga shotgun | VR80 Silent barter at LL2 |

Mechanic LL2 also sells the **MP-18 Tactical** build: an MP-18 with the threaded barrel, a SIG Sauer SRD762Ti suppressor and a Burris FullField TAC30 1-4x24 scope, for 4 Weapon parts and 2 Gunpowder "Eagle".

### New Attachments

| Attachment | Sold by |
|---|---|
| Mossberg 590A1 12ga 508mm threaded barrel | Mechanic LL2 (15,500 ₽) |
| KS-23M 23x75 6-shell magazine | Mechanic LL2 (4,600 ₽) |
| Benelli M3 Keymod Handguard | Mechanic LL2 (8,013 ₽) |
| MP-153 12ga competition 13-shell magazine | Mechanic LL3 (6,003 ₽) |
| SOK-12 12/76 Alliance Armament 30-round magazine | Mechanic LL3 (27,996 ₽) |
| MP-12 12ga 600mm barrel | Jaeger LL1 (2,134 ₽) |
| MP-18 7.62x54R 600mm threaded barrel | Mechanic LL2 (12,500 ₽) |
| MP-700 .700 Nitro Express Double Rifle 725mm Barrel | Jaeger LL3 (29,568 ₽) |
| MP-700 Shorty .700 Nitro Express 310mm Barrel | Jaeger LL3 (31,000 ₽) |
| VR80 12ga 5-round magazine | Skier LL2 (6,000 ₽) |
| VR80 12ga 10-round magazine | Skier LL2 (9,000 ₽) |
| VR80 stock with pistol grip | Skier LL2 (8,000 ₽) |
| VR80 12ga 508mm barrel | Skier LL2 (14,000 ₽) |
| VR80 12ga 508mm threaded barrel | Mechanic LL2 (15,500 ₽) |
| VR80 M-LOK handguard | Skier LL2 (11,000 ₽) |

The 590A1 threaded barrel takes the same muzzle devices as the MP-153.

The Saiga's Alliance Armament 30-round magazine has new UVs and worn anodized-aluminum textures.

The VR80 threaded barrel weighs 0.9 kg (ergonomics -10) and takes the same muzzle devices as the MP-153.

The MP-18 threaded barrel weighs 1.45 kg (ergonomics -15) and takes the SV-98 muzzle devices (the thread adapter takes the SV-98 suppressor) and the 7.62x51 direct-thread muzzle devices and suppressors of the AR-10, SR-25 and SCAR-H.

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
| .700 Nitro Express SP "Mammoth" | Jaeger LL3 (2,800 ₽), after Hunter's Dream - Part 1 | — |
| .700 Nitro Express AP "Goliath" | Jaeger LL4 (8,000 ₽), after Hunter's Dream - Part 3 | — |
| .700 Nitro Express buckshot "Cerberus" | Jaeger LL3 (3,600 ₽), after Hunter's Dream - Part 2 | — |
| 5.45x39mm 'Svalka' Anti-Drone Buckshot | — | Level 1 |

The MP-700 and MP-700 Shorty also take three custom .700 Nitro Express loads: **Mammoth** soft-point (430 damage, 15 penetration), **Goliath** armor-piercing (220 damage, 60 penetration), and **Cerberus** buckshot (8 pellets with 65 damage and 8 penetration each). These are custom additions for the mod, with fictional experimental loads. The FMJ does not penetrate plates, but it wrecks their durability. Each has a distinct projectile shape and identification markings.

### Hunter's Dream

Jaeger's three-part questline starts at level 30 after **Acquaintance**. Each part requires completion of the previous one. Use either the **MP-700** or **MP-700 Shorty**, with any compatible ammunition; progress carries over between raids.

| Quest | Objective | Purchase unlock |
|---|---|---|
| Hunter's Dream - Part 1 | Eliminate 15 Scavs on Woods from at least 40 meters away | Mammoth SP, Jaeger LL3 |
| Hunter's Dream - Part 2 | Eliminate 8 PMCs from no more than 30 meters away, on any map | Cerberus, Jaeger LL3 |
| Hunter's Dream - Part 3 | Eliminate Tagilla on Factory, day or night | Goliath AP, Jaeger LL4 |

Completing a quest unlocks its purchase offer; the trader loyalty requirement still applies. Goliath purchases require LL4 (minimum player level 33), even if Part 3 is completed earlier. Each quest also awards experience, roubles, Jaeger reputation and a small supply of the unlocked ammunition. The published FMJ purchase and craft remain available as before.

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
│       ├── CustomLocales/              names of the weapon builds
│       ├── Quests/                     Skier VR80 quests and Hunter's Dream
│       └── weaponPresets/
│           ├── Assorts/                full builds sold by Jaeger, Mechanic and Skier
│           ├── BotLoadouts/            new guns and ammo for bots
│           └── GlobalPresets/          default weapon builds
└── Client/                             BepInEx plugin (netstandard2.1)
    ├── makeshotgunsgreatagain.cs       F12 settings
    └── Patches/
        ├── BuckshotDispersionPatch.cs                     pellet spread from non-shotguns
        ├── CanResolveMalfunctionsWithoutInspectionPatch.cs
        ├── DragonBreathMuzzlePatch.cs                     Dragon's Breath sparks
        ├── DragonBreathPatch.cs
        ├── KS23MountAlignmentPatch.cs                     KS-23 optics fix
        ├── MP18SilencedSoundPatch.cs                      MP-18 suppressed sound
        └── RemoveBossMalfunctionsPatch.cs
```

---

## Credits

- Rock Island Armory VR80 shotgun and parts: adapted from [White-Horse's model](https://skfb.ly/6WP7G), licensed under [CC BY 4.0](https://creativecommons.org/licenses/by/4.0/). The model was split into modular parts and adapted to the game's materials and weapon rig. These assets retain their CC BY 4.0 attribution; the mod's MIT license covers the code.
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
