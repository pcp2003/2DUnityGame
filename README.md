# 2D Unity Game

<<<<<<< HEAD
## Main Menu
![Main Menu](https://raw.githubusercontent.com/pcp2003/2DUnityGame/main/docs/screenshots/gameplay.png)
=======
## 🎮 Main Menu
![Main Menu](https://raw.githubusercontent.com/pcp2003/2DUnityGame/main/docs/screenshots/mainMenu.png)
>>>>>>> 35a1fe07a4b128fb7d965aea224bc13b72e166f9

## Gameplay
![Game Gameplay](https://raw.githubusercontent.com/pcp2003/2DUnityGame/main/docs/gifs/gameplay.gif)

---

## Overview

**2D Unity Game** is an action-adventure 2D game developed in **Unity 6 (6000.0.23f1)**. The player must navigate through an island, defeat enemies, collect keys of different colors, open chests to gain power-ups, and survive waves of increasingly challenging encounters.

This project demonstrates core game development concepts including:
- Top-down character movement and combat
- Enemy AI with pathfinding and obstacle avoidance
- Inventory management system
- Power-up mechanics
- Dynamic difficulty scaling
- Audio management

---

## ✨ Features

✅ **Dynamic Player Movement** - Smooth character movement with 4-directional animations  
✅ **Combat System** - Melee attack mechanics with range detection and cooldown management  
✅ **Enemy AI** - Goblins and Soldiers with intelligent pathfinding and obstacle avoidance  
✅ **Inventory System** - Collect colored keys and manage inventory with slot limitations  
✅ **Color-Based Chest System** - Open color-matched chests with corresponding keys  
✅ **Power-Up System** - Randomized rewards from chests (health restoration, damage boost, etc.)  
✅ **Health & Damage System** - Player and enemy health management with hit animations  
✅ **Kill Counter** - Track enemy defeats with persistent counter display  
✅ **Audio System** - Walking, attack, and ambient background music with volume control  
✅ **Difficulty Scaling** - Enemy stats scale with enemy count for progressive challenge  
✅ **Multiple Scenes** - Menu scene and main gameplay scene with proper transitions  

---

## 🎯 Gameplay Mechanics

### Player
- **Health:** 3 HP (configurable)
- **Attack Range:** 1.0 units
- **Attack Damage:** 1 HP per hit (configurable)
- **Attack Cooldown:** 1.0 second
- **Movement Speed:** 3.0 units/second
- **Inventory Capacity:** 3 keys maximum

### Enemies

#### Goblin
- **Health:** 20 HP (scales with difficulty)
- **Attack Damage:** 5 HP (scales with difficulty)
- **Movement Speed:** 2.0 units/second (scales with difficulty)
- **Attack Range:** 1.0 unit
- **Behavior:** Pursues player, avoids obstacles, drops keys on defeat
- **Key Drop Chance:** 100% (weighted random distribution)

#### Soldier
- **Similar mechanics to Goblin with potential variations**

### Chests
- **5 Color Variants:** Red, Green, Blue, Purple, Pink
- **1 Special Variant:** Golden (accepts any key)
- **Mechanism:** Requires matching colored key to open
- **Reward:** Random power-up from loot pool
- **State:** Animates opening and becomes permanently locked after use

### Keys
- **Color Variants:** Red, Green, Blue, Purple, Pink, Golden
- **Capacity:** Limited inventory space (3 keys max)
- **Source:** Dropped by defeated enemies
- **Function:** Required to open matching color chests

---

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── PlayerController.cs          # Player movement, combat, and inventory
│   ├── PlayerHealth.cs              # Player health management
│   ├── Goblin.cs                    # Goblin enemy AI and behavior
│   ├── Soldier.cs                   # Soldier enemy mechanics
│   ├── EnemiesHealth.cs             # Enemy health management
│   ├── chestscript.cs               # Chest mechanics and opening
│   ├── KeyScript.cs                 # Key collection and management
│   ├── Cell.cs                      # Grid cell system
│   ├── TileMap.cs                   # Tilemap management
│   ├── Spawner.cs                   # Enemy spawning system
│   ├── PowerUpManager.cs            # Power-up generation and distribution
│   ├── UIHandler.cs                 # UI management
│   ├── CanvasUpdate.cs              # Canvas rendering and updates
│   ├── CameraFollow.cs              # Camera follow player logic
│   ├── BackGroundMusicManager.cs    # Audio and music management
│   ├── MainMenu.cs                  # Main menu functionality
│   ├── ExitButton.cs                # Exit button behavior
│   └── OptionsScripts/              # Options menu functionality
│
├── Scenes/
│   ├── Menu.unity                   # Main menu scene
│   └── PlayingScene.unity           # Main gameplay scene
│
├── Prefabs/
│   ├── Player.prefab                # Player prefab
│   ├── Goblin.prefab                # Goblin enemy prefab
│   ├── Soldier.prefab               # Soldier enemy prefab
│   ├── Red/Green/Blue/Purple/Pink Chest.prefab
│   ├── Red/Green/Blue/Purple/Pink/Golden Key.prefab
│   ├── Health PowerUp.prefab
│   ├── GridElement [0-5].prefab    # Environment grid elements
│   ├── SlotBar[1-3].prefab         # Inventory slot UI
│   ├── KillCounter.prefab          # Kill counter display
│   └── Environment Prefabs (Trees, Rocks, Bushes)
│
├── Animations/
│   ├── Player_Character/            # Player animations
│   ├── Enemy_Goblin/                # Goblin animations
│   ├── Soldier/                     # Soldier animations
│   └── Chests/                      # Chest open/close animations
│
└── GameUi.uxml                      # UI layout definition
```

---

## 🛠️ Requirements

- **Unity Engine:** 6000.0.23f1 or compatible version
- **C# Version:** 7.3+
- **Platform:** Windows, macOS, Linux (desktop platforms)
- **.NET Framework:** Unity's Mono/.NET runtime

### Dependencies
- Unity 2D Sprite Animation
- Universal Render Pipeline (URP)
- Input System Package
- UI Toolkit

---

## 📦 Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/yourusername/2DUnityGame.git
   cd 2DUnityGame
   ```

2. **Open in Unity:**
   - Launch Unity Hub
   - Click "Open Project"
   - Select the `2DUnityGame` folder
   - Wait for Unity to import assets and compile scripts

3. **Load the Game Scene:**
   - Navigate to `Assets/Scenes/`
   - Double-click `PlayingScene.unity` to load the main game
   - Press the Play button in the Unity Editor

---

## Controls

### Keyboard & Mouse
| Action | Input | Notes |
|--------|-------|-------|
| **Move** | `WASD` or `Arrow Keys` | 8-directional movement |
| **Attack** | `Left Mouse Button` | Melee attack in facing direction |
| **Inventory** | See UI on screen | Automatic updates on key collection |
| **Menu** | Main Menu Scene | Start/Quit game |

### Gamepad / Controller Support
| Action | Input | Notes |
|--------|-------|-------|
| **Move** | `Left Analog Stick` | 8-directional movement |
| **Attack** | `Right Trigger (RT)` or `Face Button (B/Circle)` | Melee attack in facing direction |
| **Menu** | `Start Button` | Navigate menus |

**Note:** The game supports both keyboard/mouse and gamepad controls simultaneously. Use whichever control scheme you prefer!

---

## Game Elements

### Power-Ups
Power-ups are randomly generated when opening chests. Potential effects include:
- **Health Restoration** - Recover lost HP
- **Damage Boost** - Increase attack damage
- **Speed Enhancement** - Temporary movement speed increase
- *Additional power-ups can be added via `PowerUpManager.cs`*

### Enemy Spawning
- Enemies spawn dynamically based on game progression
- Spawner script manages wave system and difficulty scaling
- Enemy stats (health, damage, speed) scale based on current enemy count
- Maximum enemy count determines difficulty level

### UI System
- **Health Display** - Current player health indicator
- **Kill Counter** - Total defeated enemies count
- **Inventory Slots** - Visual representation of collected keys
- **Power-Up Messages** - Notifications when collecting rewards
- **Main Menu** - Navigate to gameplay or exit

---

## 🔧 Customization

### Adjusting Player Stats
Edit in `PlayerController.cs`:
```csharp
public float speed = 3.0f;           // Movement speed
public float health = 3;             // Maximum health
public float attackRange = 1.0f;     // Attack detection radius
public float attackDamage = 1;       // Damage per hit
private float attackCooldown = 1.0f; // Time between attacks
```

### Adjusting Enemy Difficulty
Edit in `Goblin.cs`:
```csharp
private float speed = 2.0f;
private float attackInterval = 1.0f;
private float health = 20;
private float attackDamage = 5;
```

### Key Drop Rates
Edit in `Goblin.cs` `DropKey()` method:
```csharp
float[] pesos = { 0.1f, 0.18f, 0.18f, 0.18f, 0.18f, 0.18f }; // Weighted chances
```

---

## 🎨 Art & Animation

The game features:
- **Sprite-based 2D graphics** with animation sets for:
  - Player character (idle, walking, attacking, hit, death)
  - Goblin enemy (walking, attacking, hit, death)
  - Soldier enemy (custom animations)
  - Chests (opening animation)
  
- **Environmental elements:**
  - Trees, rocks, bushes (static props)
  - Tilemp-based level layout

---

## 🔊 Audio

Audio management handled by `BackGroundMusicManager.cs`:
- **Footstep sounds** - Play when moving
- **Attack sounds** - Play on successful hits
- **Background music** - Continuous ambience
- **Volume control** - Adjustable master volume

---

## 🚀 Future Enhancements

- [ ] Multiple level progression with increasing difficulty
- [ ] Boss encounters with unique attack patterns
- [ ] Additional enemy types and variations
- [ ] Power-up system expansion
- [ ] Combo attack system
- [ ] Multiplayer support (indicated by array usage in attack code)
- [ ] Leaderboard system
- [ ] Save/Load game state
- [ ] Mobile platform support

---

## 🤝 Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

---

## 🙏 Acknowledgments

- Unity Technologies for the fantastic game engine
- The open-source game development community
- All contributors and testers

---

**Last Updated:** November 14, 2025  
**Version:** 1.0.0
