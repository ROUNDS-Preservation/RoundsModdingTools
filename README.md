
# Rounds Modding Tools

## How to Build the Project

1. **Setup Symlink:**
    - Run the file named `Setup Symlink.bat`.
    - Upon opening the file, you'll be prompted with `Please enter the location of your Rounds folder:`.
    - Locate your Rounds folder by:
        - Going to Steam.
        - Right-clicking on the game "Rounds".
        - Hovering over "Manage".
        - Clicking on "Browse Local Files".
    - Copy the path from the "Path Bar" at the top of the File Explorer window.
    - Paste the copied path into the command prompt window.
    - After pasting, you should see `Symlink created successfully.` This indicates that the symlink has been created.
    - Close the command prompt window.

2. **Create Assemblies Folder:**
    - In the same level where the `RoundsFolder` symlink was created, create a new folder called `Assemblies`.
    - Paste the following files into the newly created `Assemblies` folder:
        - `CardChoiceSpawnUniqueCardPatch.dll` can be built from this [GitHub link](https://github.com/ROUNDS-Preservation/CardChoiceSpawnUniqueCardPatch).
        - `UnboundCards.dll` and `UnboundLib.dll` can be built from this [GitHub link](https://github.com/ROUNDS-Preservation/UnboundLib).
        - `BepInEx.dll` and `0Harmony.dll` can be found in the `BepInEx` folder named `core`.

3. **Build the Project:**
    - Open the project file.
    - Press `Ctrl + B` to build.
