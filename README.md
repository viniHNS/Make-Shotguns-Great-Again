# Make Shotguns Great Again!

#### [EN](README.md) | [PT_BR](README_BR.md)

![fire](https://media1.giphy.com/media/v1.Y2lkPTc5MGI3NjExcTZiZzdxeXJ0eTBxNWNtbHVxdzBweWkxbjhiNnJoc2ZiOTh4a3VzZyZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/u1aUNE2xRmk3xUaSAJ/giphy.gif)

## What is this?

This is a mod for [SPT](https://www.sp-tarkov.com "The main goal of the project is to provide a separate offline single-player experience with ready-to-use progression for the official BSG client. Now you can play Escape From Tarkov while waiting for their servers to come back online, while you're disconnected from the Internet, or if you need to take a break from cheaters.") that improves the shotguns in the game and adds new attachments, guns, and ammunition.

## What does this mod do?

Adds some features to the shotguns in the game and new attachments for them:

- ~~The Saiga12K now has a Full Auto mode. (be careful with the recoil ಠ_ಠ)~~ Full auto version exists in the game now.
- The semi-automatic mode of the Benelli M3 has been slightly adjusted, similar to other semi-automatic shotguns in the game, such as the MP-155 and MP-153.
- The ETMI-019 and Kiba Arms SPRM rail mounts are now compatible with more optics and mounts.
- Now you can put the Kiba Arms SPRM rail mount and the AK GP-25 accessory kit recoil pad
in the KS-23M.
- The ETMI-019 and Kiba Arms SPRM rail mounts can now be equipped on the MTs-255
- The Kiba Arms SPRM rail mount can now be equipped on the Mossberg 590A1.
- Raised the Barrikada slug's accuracy stat to match the mod's other 12ga slugs, tightening its group.
- Now the saiga-12k can use most of the AKs handguards.
- Increased AA-12 Rate of Fire..

Malfunction Overhaul:
* **Skip Inspection Before Clearing**
    * Allows players to clear a weapon malfunction immediately using the clear keybind, bypassing the requirement to visually inspect the weapon first.

* **Remove Boss Forced Malfunctions**
    * Prevents bosses (such as Kollontay) from triggering scripted, forced weapon malfunctions on the player's active firearm. This ensures that jams are only caused by weapon condition or ammunition stats.

These can be adjusted in the BepInEx config (F12)

## Configuration

A `config/config.json` file lets you decide whether bots are allowed to use the mod's FRAG-12 and Dragon's Breath ammunition. Both are enabled by default.

```json
{
  "enableBotsUseFrag12": true,
  "enableBotsUseDragonBreath": true,
  "enableDebugLogs": false
}
```

## Fixes

* **KS-23 Optics Zeroing:** Fixed optics mounted on the KS-23's rail shooting off-reticle regardless of the sight used. Shots now land where the reticle points.


## New Attachments

- KS-23M 23x75 6-shell magazine
- MP-153 competition 13-shell magazine
- Benelli M3 'M-LOK' handguard
- Saiga-12k Alliance Armament 30-round magazine
- **Mossberg 590A1 12ga 508mm threaded barrel** — the vanilla 590A1 has no way to mount a muzzle device at all. This barrel is threaded at the front, so the shotgun can finally run a choke or a suppressor. Takes the same muzzle devices as the MP-153.

## "New" Guns
- **MP-12 Single Shot Shotgun**:  
A compact shotgun designed for precision. Perfect for those who like to keep things simple and deadly.  

- **MP-700 .700 Nitro Express Double Rifle**:  
A beast of a weapon capable of taking down even the toughest adversaries. This double-barreled powerhouse chambers the **new .700 Nitro Express FMJ**, ensuring you make every shot count.

- **MP-700 Shorty .700 Nitro Express Double Rifle**:  
A sawed-off variant of the MP-700. Losing the stock and most of the barrel length trades accuracy for a devastatingly concealable hand cannon. Recoil is, unsurprisingly, brutal.

- **ZiD SP-81 23x75 pistol**:  
A modified SP-81 signal pistol, re-chambered and reinforced to fire 23x75mm rounds. Incompatible with flares and other signal ammunition.

## New Ammunition and Hideout Production Recipes

- 12/70 Magnum Express Kinghunter
- 12/70 Flechette Kinghunter and box with 25 rounds
- 12/70 AP Slug SVAROG and box with 5 rounds
- 12/70 Winchester Super-X 00 buckshot
- .700 Nitro Express FMJ
- 12/70 'Hellfire' hybrid buckshot
- 12/70 FRAG-12 HE
- 12/70 Brass Case
- 5.45x39mm Svalka (buckshot anti-drone round)

## Installation

1.  Download the `makeshotgunsgreatagain.zip`.
2.  Drag and drop the `.zip` file directly into the root folder of your SPT installation.
3.  Right-click the `.zip` file and select **"Extract Here"**.
4.  The folders should merge automatically. If you get a prompt to overwrite files, say yes.

## License

This mod is licensed under the [MIT License](LICENSE).

## Credits

- 12/70 Winchester Super-X 00 buckshot -> [Guy in a Poncho](https://sketchfab.com/ponchoguy)
- 12/70 AP Slug SVAROG -> [Deadcomrade](https://sketchfab.com/deadcomrade)
- 12/70 Flechette Kinghunter -> [Deadcomrade](https://sketchfab.com/deadcomrade)
- 12/70 Magnum Express Kinghunter -> [Deadcomrade](https://sketchfab.com/deadcomrade)
