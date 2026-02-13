# Make Shotguns Great Again! — Client Plugin

## What is this?

This is the client-side BepInEx plugin for [Make Shotguns Great Again](https://github.com/viniHNS/Make-Shotguns-Great-Again), a mod for [SPT](https://sp-tarkov.com/ "The main goal of the project is to provide a separate offline single-player experience with ready-to-use progression for the official BSG client.") that improves shotguns in the game.

## What does this plugin do?

This plugin handles the client-side patches that require runtime code modification:

### Dragon Breath Visual Effect
- Spawns a procedural incendiary spark effect when firing **12/70 Dragon Breath** rounds.
  ###### Special thanks to **jankytheclown** [HollywoodFX](https://github.com/SleepingPills/HollywoodFX) mod, whose work served as the base for the particle effects code.

### Malfunction Improvements
- **Skip Inspection**: Allows clearing weapon malfunctions without inspecting the weapon first. *(Configurable)*
- **Remove Boss Forced Malfunctions**: Prevents bosses from forcing a weapon malfunction on the player. *(Configurable)*

## Configuration

Both malfunction options can be toggled in the BepInEx configuration:

| Setting | Default | Description |
|---------|---------|-------------|
| Skip Inspection Before Clearing | `true` | Clear malfunctions without inspecting first |
| Remove Boss Forced Malfunctions | `true` | Prevent bosses from forcing malfunctions |

## License

This mod is licensed under the [MIT License](LICENSE).

## Credits
