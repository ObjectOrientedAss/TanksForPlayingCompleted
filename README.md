# Tanks For Playing
This is a simple multiplayer game made with Unity, using only Steamworks Facepunch SDK. I made this project to teach the P2P Networking principles to AIV second year students.
I would also like to thank Filippo Maria Perlini (https://github.com/peepo-mary-pearls) for the models/shaders/textures.

## The Game
A max of four players face each other in a small arena with their own tanks. Each match is divided in a given number of rounds with a specific duration, who wins the most rounds, wins the match.

## The Rules
The host can decide the game rules before starting the match.
- Win condition: Decide if when the round time ends, the winner is the one with the most kills or the one with the most damage dealt.
- Rounds to win: Decide how many rounds must be won in order to win the match (remember that if there is a Tie, this value is automatically increased while playing).
- Respawn Cooldown: Decide how much time must pass in order to respawn after being killed during the match.
- Round duration: Decide the duration of a single round.

## FAQs

- How much time did it take to complete it?
A couple of weeks.

- Is it a COMPLETE game? Can i play it with other people just cloning this repo?
Yay.

- Why Peer to Peer?
Because it's faster to setup, and it is a teaching project.

- Why Steamworks Facepunch?
Because i find Steamworks.NET lacking and more complex. Nothing personal, sorry.

- I have downloaded the project and opened it through Unity, but nothing is working and i'm getting errors. Why?
You have to open Steam (and login, of course) before playing in Unity.

- When i accept an invite from Steam, it asks me to open Spacewar. Why?
It's because this is not a released game, it's just a sandbox project. To test, Steam allows developers to use 480 as AppId, which happens to be Spacewar on the Steam Store.
If you want to accept an invite or join a lobby, use the in-game UI only.

## What can i find of interesting in this project?
- If you want a small hint on how to implement a vary basic lag compensation, check:
LagCompensation.cs
- If you want a small hint on how to use a "loading screen + wait players to do something before moving on and sync them" combo, check:
LoadingScreenBehaviour.cs | WaitHandler.cs
- If you want a small hint on how to build and read a packet using Facepunch SDK, check:
P2PPacketWriter.cs | P2PPacketReader.cs
- If you want a small hint on how to handle the main Networking events, and other events in general, check:
NetworkManager.cs | NW_EventSystem.cs | GP_EventSystem.cs | UI_EventSystem.cs
- If you want a small hint on how to perform host validation on something, check:
Bullet.cs

## Known bugs
- Pickup text is not hidden when an interactable gets auto-disabled and the player is standing near to it.
- Lag compensation simulation makes the tanks move through objects if their moving in that direction. It is not a real bug, it's just something that is still not handled.
- The shooting VFX is not right aligned with the turret.
- This is a 4 player max game, but if in the create lobby panel you select a higher number, it is valid. It's a super easy fix but i had no time.
- The cannon and the mortar turrets material has always the same color. It's because they were added later and the material changing snippet was not performing a loop at that time, but changing the only turret i had.

## Final considerations
It's not perfect, it's not definitive, it's not bugless (as i have reported in the known bugs session), but it does its dirty job very well.
I will probably improve it if/when i'll be more free.
