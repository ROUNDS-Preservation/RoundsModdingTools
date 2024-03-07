:: This script creates a symlink to the rounds folder.

set /p folderPath="Please enter the location of your Rounds folder: "

if not exist "%folderPath%" (
    echo Error: Folder location does not exist. Please enter a valid folder location.
    pause
    exit /b 1
)

rmdir /s /q RoundsFolder > nul 2>&1
mklink /J RoundsFolder "%folderPath%"

if errorlevel 1 (
    echo Error: Failed to create symlink.
) else (
    echo Symlink created successfully.
)

pause