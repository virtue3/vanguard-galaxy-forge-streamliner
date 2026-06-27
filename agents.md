# Vanguard Galaxy Mod — Agent Notes

## Project layout

This is a BepInEx + Harmony mod for Vanguard Galaxy. See `CLAUDE.md` for full architecture notes.
The decompiled game source lives in `Assembly-CSharp/` (gitignored — owned by the game developer).

---

## Regenerating and fixing Assembly-CSharp

The `Assembly-CSharp/` folder is produced by exporting `Assembly-CSharp.dll` via dnSpy
(File → Export to Project). The raw export does **not** compile; the following fixes are required.

### 1. Rebuild the .csproj

The exported `Assembly-CSharp.csproj` is incomplete. Replace it with one that includes:

```xml
<PropertyGroup>
  <ProjectGuid>{D4448DEE-C09D-4846-9FF2-AA534A3AE8D5}</ProjectGuid>
  <TargetFrameworkVersion>v4.8</TargetFrameworkVersion>
  <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
  <LangVersion>latest</LangVersion>
</PropertyGroup>
```

And add `<Reference>` entries with `<HintPath>` pointing to each required DLL under:
`E:\STEAM\steamapps\common\Vanguard Galaxy\VanguardGalaxy_Data\Managed\`

Key DLLs needed: `mscorlib`, `UnityEngine.CoreModule`, `UnityEngine.IMGUIModule`,
`UnityEngine.InputLegacyModule`, `Unity.TextMeshPro`, `com.rlabrecque.steamworks.net`,
`System`, `System.Core`, and the Unity render pipeline / visual scripting DLLs.

### 2. Resolve `Forge` namespace conflicts

Several files use `Forge` unqualified, which conflicts with `Behaviour.Crafting.Forge`.
Fully qualify all references to the mining forge:

```csharp
// Before (ambiguous):
Forge current = Forge.current;

// After:
Source.Mining.Forge current = Source.Mining.Forge.current;
```

Affected files include `ForgeUI.cs`, `ForgeTabContents.cs`, and others in `Behaviour\UI\Forge\`.

### 3. Remove `TupleElementNames` attributes

dnSpy decompiles tuple return types with attributes that don't compile:

```csharp
// Remove blocks like this entirely:
[return: TupleElementNames(new string[]
    "parent",
    "children"
})]
```

Search for `TupleElementNames` across the project and delete the entire attribute block.

### 4. Add explicit Vector2 casts

Some implicit Vector2/Vector3 conversions are dropped by the decompiler. Add explicit casts where the compiler reports type mismatch:

```csharp
// Before:
Vector2 vector = this.target.transform.position;

// After:
Vector2 vector = (Vector2)this.target.transform.position;
```

### 5. Fix method resolution errors

A few calls land on the wrong overload after decompilation. Example in `LightJson\Serialization\JsonWriter.cs`:

```csharp
// Before:
this.Write(value.ToString(CultureInfo.InvariantCulture));

// After:
this.Write(value.AsNumber.ToString(CultureInfo.InvariantCulture));
```

Fix these case-by-case as the compiler flags them.

---

## Notes

- The `assembly-csharp-original/` folder holds the unmodified dnSpy export for reference.
- The `Assembly-CSharp/` folder holds the fixed version that compiles.
- Neither folder is tracked in git.
