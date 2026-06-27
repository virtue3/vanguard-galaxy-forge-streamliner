# VG Forge Streamliner

A BepInEx mod for **Vanguard Galaxy** that removes the tedium of multi-step forge crafting.

## Features

- **Auto highest-rarity recipe** — when you select a recipe in the Forge, the mod automatically defaults to the highest rarity tier available instead of the lowest.
- **Sub-component analysis** — the overlay shows exactly which sub-components are missing, how many forge runs are needed, and how many parallel jobs will be used across your available slots.
- **Parallel slot distribution** — "Craft Missing Sub-Parts Now" splits runs across all free forge slots simultaneously to minimise total crafting time.
- **Craft queue** — "Add to Queue" queues final recipes and works through them automatically, crafting sub-components as needed and starting the next recipe when the previous one finishes.

## Requirements

- [Vanguard Galaxy](https://store.steampowered.com/app/1573390/Vanguard_Galaxy/) (Steam)
- [BepInEx 5.x](https://github.com/BepInEx/BepInEx/releases) (Mono build) installed in the game folder

## Installation

1. Download `VanguardMod.dll`.
2. Drop it into `<Steam>\steamapps\common\Vanguard Galaxy\BepInEx\plugins\`.
3. Launch the game through Steam.

## Usage

The overlay opens automatically whenever you open the Forge at a space station and closes when you leave.

Press **F10** (default) to toggle it manually at any time.

### Overlay sections

**Craft Queue** — lists everything you have queued. The `>` marker shows the active item; sub-components currently being crafted are also marked with `>`. Click **X** on an item to remove it (removing the active item also cancels all running forge jobs).

**Selected Recipe** — mirrors the recipe and quantity currently selected in the Forge UI. Shows missing sub-components, how many runs each needs, and whether you have sufficient raw materials. Use **Craft Missing Sub-Parts Now** to fill all available slots immediately, or **Add to Queue** to let the mod handle the full crafting chain automatically.

### Configuration

BepInEx generates a config file at:
```
BepInEx\config\com.vanguardmod.forgepatch.cfg
```

| Key | Default | Description |
|-----|---------|-------------|
| `ToggleKey` | `F10` | Key to show/hide the overlay |

## Queue behaviour notes

- The queue pauses when you leave a station and clears automatically on departure.
- Cancelling any forge job (via the game's UI) cancels the mod queue and stops all running jobs.
- Sub-component needs are evaluated fresh when each queue item becomes active, so earlier crafting runs are taken into account.
