# Rounds Modding Tools

This mod contains tools for modding the game [Rounds](https://store.steampowered.com/app/1557740/ROUNDS/).

## How to Build the Project

### 1. Update Steam Library Path in Project File:

- Navigate to the `RoundsModdingTools` folder.
- Open the file named `RoundsModdingTools.csproj`.
- Locate the line resembling: `<SteamFolder>F:\SteamLibrary</SteamFolder>`.
- Replace `F:\SteamLibrary` with the path to your Steam Library folder.

### 2. Create Assemblies Folder:

- In the same directory where this `README.md` file is located, create a new folder called `Assemblies`.
- Paste the following files into the newly created `Assemblies` folder:
  - `CardChoiceSpawnUniqueCardPatch.dll` can be built from this [GitHub link](https://github.com/ROUNDS-Preservation/CardChoiceSpawnUniqueCardPatch).
  - `UnboundCards.dll` and `UnboundLib.dll` can be built from this [GitHub link](https://github.com/ROUNDS-Preservation/UnboundLib).
  - `BepInEx.dll` and `0Harmony.dll` can be found in the `BepInEx` folder named `core`.

### 3. Build the Project:

- Open the project file.
- Press `Ctrl + B` to build.
