# The Outpost Garage - 3DIMD CW1 (Group [No.])

University of Westminster, 5MMCS007W 3D Interactive Media Development, 2026.
An interactive 3D diorama: a sci-fi hover-vehicle inspection and service bay, built in Unity (URP).

## Open the project
1. Clone this repo with GitHub Desktop.
2. In Unity Hub, choose **Add > Add project from disk** and select the repo folder.
3. Editor version: the project is currently on **Unity 6000.6.7f1**. The module requires **6000.3.19f1**, and this is being confirmed with Jeff. Do not upgrade or downgrade the project on your own.
4. Open `Assets/Garage/Scenes/OutpostGarage.unity` and press Play.

## Controls
| Input | Action |
|---|---|
| Mouse drag | Orbit the camera |
| Mouse wheel | Zoom |
| R | Reset view |
| Slider / ASSEMBLE | Exploded view of the 5 vehicle parts |
| 1-4 / swatches | Paint |
| E / button | Engine start/stop |

## What is done
- Hangar scene with lighting and post-processing.
- Hover vehicle modelled in Blender. Source file: `BlenderSource/HoverVehicle.blend`.
- Five separable parts: Chassis, Cockpit, PowerCore, Thruster_L, Thruster_R.
- Scripts in `Assets/Garage/Scripts/`:
  - `InspectionCamera`
  - `ExplodedView`
  - `PaintController`
  - `EngineController`
  - `GarageUI`
  - `DemoStation`

## What each student must add (CW1 compulsory)
The scene has 6 labelled stations in `CW1_DemoStations`. **All are placeholders.**

| Station | Tutorial | Student |
|---|---|---|
| Station_1 | Rigidbody brick controller (Tut 02-03) | [name] |
| Station_2 | Mouse-drag brick controller (Tut 02-03) | [name] |
| Station_3 | Character controller brick (Tut 02-03) | [name] |
| Station_4 | Brick triggers + exploding array (Tut 04) | [name] |
| Station_5 | Slider + exploding brick array (Tut 05) | [name] |
| Station_MechanicBay | One animated LegoWalk.fbx minifig per student, with a UI button (Tut 05.2) | everyone |

How to add your work:
1. Download LegoWalk.fbx and the Lego brick files from Blackboard.
2. Build your tutorial in your own scene. Export it as a Unity Package and import it into this project.
3. Put your objects under your station's `Content (student work goes here)` object and delete the red placeholder brick.
4. On the station's DemoStation component, set your name and untick `isPlaceholder`.
5. Name your assets and scripts with your name. Comment your code with your name and references.
6. Avoid E, R, 1-4 and left-mouse drag for your controls. Use arrows, WASD or Space.

## Repo layout (from the CW1 brief)
- `Assets/`, `Packages/`, `ProjectSettings/` - the Unity project. Never commit `Library/`; it is ignored by `.gitignore`.
- `Group[No]_CW1_Report/` - group planning report (to add)
- `Group[No]_CW1_GroupWork/` - meeting notes, Padlets, planning evidence (to add)
- `Group[No]_CW1_FirstName_LastName_StudentNo/` - one folder per student: individual report, AI chat transcripts plus AI summary, any AI permission evidence

## Deadlines
| Date | Deliverable |
|---|---|
| 29 Oct | WIP video, ~5 min, `3DIMD_2026_GroupNumber_WIP1.mp4`, uploaded to the Blackboard SharePoint link |
| 5 Nov | Peer review video |
| 12 Nov, 13:00 | Repo link and final video submitted on Blackboard |

The repo must be public or shared with GitHub user `u00w`.

## AI use
Garage scripts and the vehicle model were made with assistance from Claude (Anthropic). Divya Patel has written permission from J. Ferguson for this (27 Oct 2026). Every AI-assisted file carries a header comment saying so. Anyone else using AI must reference it in their code comments and report, and include their transcripts.

Full notes, verification results and the WIP shot list: `GARAGE_NOTES.md`.
