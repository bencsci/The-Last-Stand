# The Last Stand Game

**Note:** This project is too large to fully upload to GitHub, so only a few files are provided. However, the full project build is available via [Download the full game build here](https://drive.google.com/file/d/1xfw3hL9k2Mf9lrBYqGcIyz2X-6WW0VVu/view?usp=sharing).

Additionally, a playable build of the game is available for download. Simply download the folder, and to play the game, run the `.exe` file inside.

> **📌 Course Info:**  
> This project was created as part of **Saint Mary's University** Game Development course **CSCI 3827**.  
> Developed by group members:
> - Emmet Dixon  
> - Ben Le  
> - Eric Wall  
>
> ⚠️ A web version of the game also exists, but some features may not function properly as it was originally designed for PC download and play.

## How to Run

Game Download: [Link](https://drive.google.com/file/d/1Jz4s31RWZTETcwGlmlX5LOprHxDTlOAP/view?usp=sharing)

1. Download the folder containing the full build of the game.
2. Inside the folder, run the `.exe` file to launch the game.
3. Enjoy!
   
## How to Play

For a detailed explanation of the gameplay, refer to the **How To Play** menu in the game. Below is a short rundown:

- **Wave Number**: Displayed in the top left.
- **Health (HP)**, **Current Ammo**, and **Reserve Ammo**: Displayed in the bottom right.
- **Points**: Displayed in the bottom left.

### Basic Gameplay Instructions:

1. **Kill all enemies** to advance to the next wave.
2. A **10-second waiting period** occurs between waves.
3. **Shoot enemies** to gain points, which can be spent to **buy better weapons**.
4. **Visit the Ice Cream Truck** to buy weapons and ammo.
5. **Bosses spawn every 5 rounds**.
6. **Ammo** can drop on the ground after the 5th round by killing enemies.
7. **Health auto-regenerates** for 10 HP every 15 seconds during the first 9 rounds.
8. **Notes** are hidden around the map. Press **F** within range to open them, granting 100 points upon the first discovery.

## Screenshots
![Menu](https://github.com/user-attachments/assets/4fd49b0e-f10b-450b-9cbc-dec35ec56324)
![Shop](https://github.com/user-attachments/assets/53d06874-659f-40a5-800c-5d69b592c92f)

## Gameplay Features

- **Wave-Based System**: The game uses a wave-based system. At the start of each wave, enemies are spawned around the player, with the number of enemies increasing as the wave number rises. The wave ends once all enemies are defeated.

- **Point System**: Players earn points for every hit on an enemy. These points can be spent at the **Ice Cream Truck** to purchase weapons, ammo, and health packs.

- **Critical Hits**: When the player lands three consecutive shots, they deal a critical hit, resulting in bonus damage and points.

- **Fatal Shots**: The player earns more points for landing the fatal shot on an enemy. Critical fatal shots provide even more points.

- **Damage Indicators**: When the player lands a shot, damage text will appear above the enemy, showing how much damage was dealt.

- **Crosshair**: The crosshair follows the player's mouse movements. When aiming at an enemy, the crosshair turns **green**. When not aiming at an enemy, it remains **white**.

- **Hidden Notes**: Scattered around the map, these notes reveal bits of the game’s lore. Discovering these notes grants 100 points.

- **Ammo Drops**: Starting from the 5th round, enemies may drop ammo packs. Walking over them will replenish the player's ammo reserve.

- **Health Regeneration**: Health regenerates by 10 HP every 15 seconds for the first 9 rounds. After round 9, health can only be replenished by buying health packs at the **Food Truck**.

## Gameplay Clips
![GameplayClip](https://github.com/user-attachments/assets/24da733f-c1cb-47a8-a187-6d0d20abb35f)
![DeathClip](https://github.com/user-attachments/assets/5121df76-5447-40b4-9d56-d21db1d19757)

## Built With 🛠️  

This game was developed using the **Unity Game Engine**.  

### **Packages Used**:  
- **Input System** – For handling player inputs like movement, aiming, and shooting.  
- **AI Navigation (NavMesh)** – For enemy pathfinding and AI behavior.

