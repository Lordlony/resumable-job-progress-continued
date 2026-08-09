# Resumable Job Progress (Continued)

RimWorld 1.6 continuation of [Resumable Job Progress by denev](https://steamcommunity.com/sharedfiles/filedetails/?id=2801102127).

The original mod and its author remain credited. Lordlony maintains the 1.6 update in this repository. This continuation will be removed or transferred at the original author's request, or if the original mod is updated.

## Included source

`Source/` contains the current C# source and Visual Studio solution for the 1.6 build. It deliberately excludes RimWorld, Harmony, compiler binaries, generated output, editor caches, and historic backup files.

No license has been added by this continuation. This repository does not claim ownership of denev's original work or grant rights beyond those held by the original author.

## Build for RimWorld 1.6

1. Install Visual Studio 2022 with the .NET desktop development workload.
2. Obtain RimWorld and Harmony through their normal distributions; do not commit their DLLs to this repository.
3. Build `Source/ResumableJobProgress.sln` in Release configuration, passing the two MSBuild properties:

```powershell
msbuild Source\ResumableJobProgress.sln /p:Configuration=Release /p:RimWorldDir="C:\path\to\RimWorld" /p:HarmonyDir="C:\path\to\Harmony\Assemblies"
```

The build writes `ResumableJobProgress.dll` to `v1.6/Assemblies` when used from the full mod package. In a source-only checkout, adjust the output path or copy the resulting DLL into the package's `v1.6/Assemblies` folder.

## Compatibility notes

- Harmony is required.
- Smart Deconstruction and Mining variants are detected at runtime; overlapping deconstruction/uninstall progress handling is disabled while they are active.
- Medieval Overhaul mending support is discovered at runtime and is not a hard dependency.
