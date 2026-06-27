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

1. Download the latest `VGForgeStreamliner-v*.zip` from the [Releases](../../releases) page.
2. Extract it directly into your game root: `<Steam>\steamapps\common\Vanguard Galaxy\`
   - The zip already contains the correct `BepInEx\plugins\` folder structure.

## Usage

The overlay opens automatically whenever you open the Forge at a space station and closes when you leave.

Press **F10** (default) to toggle it manually at any time.

### Overlay sections

**Craft Queue** — lists everything you have queued. The `>` marker shows the active item; sub-components currently being crafted are also marked with `>`. Click **X** on an item to remove it (removing the active item also cancels all running forge jobs).

**Selected Recipe** — mirrors the recipe and quantity currently selected in the Forge UI. Shows missing sub-components, how many runs each needs, and whether you have sufficient raw materials. Use **Craft Missing Sub-Parts Now** to fill all available slots immediately, or **Add to Queue** to let the mod handle the full crafting chain automatically.

### Configuration

BepInEx generates a config file at:
```
BepInEx\config\com.virtue3.vgforgestreamliner.cfg
```

| Key | Default | Description |
|-----|---------|-------------|
| `ToggleKey` | `F10` | Key to show/hide the overlay |

## Queue behaviour notes

- The queue pauses when you leave a station and clears automatically on departure.
- Cancelling any forge job (via the game's UI) cancels the mod queue and stops all running jobs.
- Sub-component needs are evaluated fresh when each queue item becomes active, so earlier crafting runs are taken into account.

---

## Development

### Deploy (local testing)

Close the game first — it locks the DLL. Then from the repo root:

```powershell
cd VanguardMod
dotnet build
```

This builds in Debug mode and copies `VGForgeStreamliner.dll` directly to the BepInEx plugins folder.
BepInEx log: `<game>\BepInEx\LogOutput.log`

### Release

Requires the [GitHub CLI](https://cli.github.com/) (`gh auth login` once to authenticate).

```powershell
# From repo root:
.\release.ps1                    # patch bump: 1.0.0 → 1.0.1 (default)
.\release.ps1 -BumpType minor    # minor bump: 1.0.0 → 1.1.0
.\release.ps1 -BumpType major    # major bump: 1.0.0 → 2.0.0
```

The script will:
1. Bump the version in `VanguardMod/VanguardMod.csproj` and `VanguardMod/Plugin.cs`
2. Build in Release mode
3. Package `VGForgeStreamliner-v{version}.zip` with the correct folder structure
4. Create a GitHub release with auto-generated notes

### Regenerating decompiled game source

The `Assembly-CSharp/` folder contains decompiled game source used as reference when writing patches. It is excluded from the repo (owned by the game developer). To regenerate it locally:

1. Install **[dnSpy](https://github.com/dnSpy/dnSpy/releases)**.
2. Open `<Steam>\steamapps\common\Vanguard Galaxy\VanguardGalaxy_Data\Managed\Assembly-CSharp.dll`.
3. File → Export to Project → output to `Assembly-CSharp/` in the repo root.

The decompiled source is reference only — it does not need to compile and is never included in the mod.
