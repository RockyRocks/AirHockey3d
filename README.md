# AirHockey3d

AirHockey3d is a fully-featured, 3D Air Hockey game built with Unity. It features realistic physics, AI opponents, and various game modes to provide an engaging arcade experience. 

## Features
- **Realistic 3D Physics:** Precise puck behavior and paddle movement.
- **Multiple Game Modes:** Play for tickets, play for money, and other challenge modes.
- **AI Opponents:** Challenging computer-controlled AI.
- **Polished UI:** Immersive menus, splash screens, and in-game UI.

## Requirements
- **Unity Version:** Unity 6000.4.1f1 or compatible later versions.
- **Platform:** Windows, Mac, or Linux (Standard Unity Supported Platforms).
- **.NET SDK:** Compatible with modern .NET for Unity's C# scripting.

## Getting Started
1. Open the Unity Hub.
2. Click **Add project from disk** and select the `AirHockey3d` folder.
3. Open the project.
4. Navigate to `Assets/Scenes` and open the Main Menu or Game scene to start.
5. Press the **Play** button in the Unity Editor to test the game.

## Directory Structure
Below is a high-level overview of the project's directory structure:

```text
AirHockey3d/
├── Assets/                 # Primary game assets
│   ├── 3dModels/           # 3D models (paddles, tables, pucks)
│   ├── Audio/              # Sound effects and background music
│   ├── Fonts/              # Custom fonts used in the UI
│   ├── Materials/          # Textures and materials for 3D objects
│   ├── Prefabs/            # Reusable Unity prefabs
│   ├── Scenes/             # Unity scene files (Menu, Game, etc.)
│   └── Scripts/            # Core C# logic and game scripts
│       ├── AirHockey/      # Puck and Player controllers
│       ├── MotionBlur/     # Camera and post-processing effects
│       └── Utils/          # Debugging and utility scripts
├── Packages/               # Unity Package Manager manifests
├── ProjectSettings/        # Unity project-wide settings
└── README.md               # Project documentation
```

## Compilation Note
This project has been recently upgraded to support modern Unity component systems, migrating away from legacy properties (like `gameObject.audio` or `gameObject.rigidbody`) to `GetComponent<T>()`, and legacy UI (`GUIText`) has been updated. If using an older version of Unity, you may encounter compilation issues.

## Current State & Known Issues
Please be aware that a few assets currently have missing references. The project is in need of a major revamp and overhaul to bring it fully up to current mobile standards.

## License
This project is licensed under the GNU General Public License v3.0 - see the [LICENSE](LICENSE) file for details.
Copyright (C) 2026 Rakesh Kumar Raparla.
