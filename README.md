I was not able to fully complete the assignment.
I saw the EMail late and I'm restricted in time, since i still work full time at the moment.
I've spent less than 16h in total, but hope it can still be considered a valid entry.

Quick explanation about the code frame work:
GameInstance and GameMode principal is adapted from Unreal standard framework (A very lightweight version in this case)
I did use this idea of replicating Unreal's framework in Unity before (In it's fullest, we always have a gameinstance -> a gamemode -> a playercontroller -> a default pawn (that gets "Possesed" by the player controller))
I used one 3rd party "Feature" SerializableDictionary, since this is a feature lacking in Unity since the beginning and should have been integrated by Unity for a long time!

what was left on my TODO list:
- Make the database sprites addressables so we can load and unload per level as needed
- Make adjustments to run on Android
- Adding a Pause menu so we are able to: Save/Load, Go back to Main Menu or Quit during Gameplay
