# Progress

This repository contains a Unity-based simulation and policy strategy game. The project uses a scene-driven structure, with gameplay logic in the `Assets/Scripts` folder and a shipped desktop build already available under the `Builds` directory.

## Project overview

- Engine: Unity 6 (`6000.4.6f1`)
- Main project entry points:
  - `Assets/Scenes/MainTitle.unity` – title/menu scene
  - `Assets/Scenes/MainScene.unity` – main gameplay scene
- Primary source code folder:
  - `Assets/Scripts`

## Source code navigation

### Core gameplay and world simulation

- `Assets/Scripts/CampaignTimeManager.cs` – year/timeline progression
- `Assets/Scripts/EarthStateController.cs` – overall world state and simulation controller
- `Assets/Scripts/EarthMetrics.cs` – metric container/aggregate values
- `Assets/Scripts/TimeControls.cs` – time-speed controls
- `Assets/Scripts/CameraControl.cs` – camera interaction and movement
- `Assets/Scripts/RotationScript.cs` – planetary/world rotation behavior

### UI and menu flow

- `Assets/Scripts/UI/MainMenuUI.cs` – main menu play/quit flow
- `Assets/Scripts/UI/GameMenuUI.cs` – in-game pause/restart/resume behavior
- `Assets/Scripts/UI/MenuControls.cs` – generic menu input handling
- `Assets/Scripts/UI/MetricUpdater.cs` – updates displayed metrics into the UI
- `Assets/Scripts/UI/CyberpunkUIAnimation.cs` – animated menu or HUD effects

### Policy tree and interactions

- `Assets/Scripts/PolicyTreeScripts/PolicyTreeManager.cs` – central policy logic and point generation
- `Assets/Scripts/PolicyTreeScripts/NodeData.cs` – policy node metadata
- `Assets/Scripts/PolicyTreeScripts/NodeRuntime.cs` – runtime UI node behavior
- `Assets/Scripts/PolicyTreeScripts/EdgesManager.cs` – policy-edge / relationship graph generation
- `Assets/Scripts/PolicyTreeScripts/PolicyEnacting.cs` – policy application logic
- `Assets/Scripts/PolicyTreeScripts/PolicyMapInput.cs` – canvas interaction and zoom/pan behavior
- `Assets/Scripts/PolicyTreeScripts/ToolTip.cs` – hover tooltip logic

### Responsive Earth visuals

- `Assets/Scripts/ResponsiveEarth/AtmosphereController.cs`
- `Assets/Scripts/ResponsiveEarth/AuroraController.cs`
- `Assets/Scripts/ResponsiveEarth/CloudController.cs`
- `Assets/Scripts/ResponsiveEarth/EarthShaderController.cs`

## Build and run notes

A working desktop build is already present in the repository under `Builds/`.

Current build contents:

- `Builds/Progress.exe`
- `Builds/Progress_Data/`
- `Builds/MonoBleedingEdge/`
- `Builds/UnityPlayer.dll`

If you want to run the packaged game directly, open the `Builds` folder and launch `Progress.exe`.

## Suggested workflow

1. Open the Unity project in the root folder.
2. Start from `Assets/Scenes/MainTitle.unity` to understand the menu flow.
3. Move into `Assets/Scenes/MainScene.unity` for the main simulation scene.
4. Use the policy tree and world-state scripts together when tracing gameplay events.
5. If you need a quick test run, launch the existing build from `Builds/Progress.exe`.

## Notes for contributors

- The root folder contains Unity-generated project metadata and the actual editor solution files.
- `Assets/Editor` contains supporting game data and editable policy text entries.
- `Assets/Resources` and `Assets/Prefabs` are for runtime assets, reference prefabs, and data-driven content.
- If you are making code changes, keep the scene references consistent with the existing scene names and UI controller wiring.
