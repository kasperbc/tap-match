# Tap Match
- Unity version: 6000.3.12f1

### Project architecture overview

The project is mainly split up into two segments:

- The "logic" part, which handles data and implementation of the game logic (i.e. where each matchable is, what matchables are connected to n, gravity and spawning new tiles)
- The "visual" part, which displays that to the player, mainly keeping up with the logic with events.

This was done to make implementing the game logic easier with a centralized class handling the game state (`GameBoard.cs`). While this was succesful, it did make displaying the game state trickier, opposed to using fewer classes and combining the "UI" and "data" classes. Here, I used events to communicate between the two (without creating a hard link between them) and as I understand it, this also makes white box testing easier.

---
Other things to note:
- Game settings are found in the `Assets/Data` folder
- `Assets/Scenes/SampleScene` is the game scene