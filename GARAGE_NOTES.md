# The Outpost Garage - Project Notes (27 Oct 2026)

Author of record: Divya Patel (W1995126), Group [No. - fill in]
AI assistance: Claude (Anthropic), 27 Oct 2026. Permitted by J. Ferguson (email, 27 Oct 2026). Keep the email and the full chat transcript in your individual GitHub folder.

## Where things are
- Unity project: `C:\Users\nxyso\My project` (git remote: github.com/soseeee/Garage)
- Scene: `Assets/Garage/Scenes/OutpostGarage.unity` (build index 0)
- Scripts: `Assets/Garage/Scripts/` - InspectionCamera, ExplodedView, PaintController, EngineController, GarageUI, DemoStation
- Vehicle model: `Assets/Garage/Models/HoverVehicle.fbx` (exported from Blender)
- Blender source: `BlenderSource/HoverVehicle.blend` (outside Assets on purpose, so Unity does not try to import the .blend)
- Materials (URP Lit): `Assets/Garage/Materials/`
- Post-processing profile: `Assets/Garage/Settings/GaragePostFX.asset` (Bloom, ACES, Vignette, Colour Adjustments)
- Test screenshots: `Recordings/` (project root, not in Assets)

## What changed
- New scene with hangar: walls, pillars, ceiling beams, floor seams, door, gantry, crates, tool cabinets, fuel cells, a round service platform with a cyan rim and amber hazard marks, and cyan/amber lighting.
- Hover vehicle modelled in Blender. Five separate groups: `Chassis`, `Cockpit`, `PowerCore`, `Thruster_L`, `Thruster_R`. Each group's pivot is at its bounds centre. Separate material slots: Body_Paint, Trim_Dark, Trim_Metal, Glass, Engine_Glow, Thruster_Glow, Hazard_Amber.
- Interaction scripts (see Controls).
- Six labelled CW1 demo stations (`CW1_DemoStations`). All are PLACEHOLDERS with a red cube. Each student's real tutorial work goes under that station's `Content (student work goes here)` object. Then set `isPlaceholder = false` and fill in the contributor name on the DemoStation component.
- Project settings: Run In Background = true. OutpostGarage added to Build Settings at index 0.
- `SampleScene` is still in the project. It is unchanged, apart from a test cube that was never saved.

## Controls
| Input | Action |
|---|---|
| Left or right mouse drag (starting on the 3D view) | Orbit the vehicle (vertical angle clamped 5-70 deg) |
| Mouse wheel | Zoom (4.5-18 m) |
| R / RESET VIEW button | Reset camera |
| Slider | Exploded view 0-100% (shows % readout) |
| ASSEMBLE button | Return to 0% |
| 1-4 / colour swatches | Paint: Outpost Amber, Ion Cyan, Graphite, Crimson Flare |
| E / START ENGINE button | Engine start/stop (light, glow, hover bob, generated hum) |

Engine rules:
- The engine won't start unless the vehicle is assembled. A message is shown instead.
- Exploding the vehicle while it is running shuts it down.
- Pressing E part-way through a start or stop reverses it smoothly.

## Verification actually performed (27 Oct, Unity 6000.6.7f1, Play Mode)
- Compiles with 0 errors and 0 warnings. No missing scripts. No missing or pink materials.
- Real mouse input in the Game view:
  - Dragging the slider moves it to 80%, and the camera does NOT orbit.
  - Dragging in the 3D view orbits.
  - The mouse wheel zooms.
- Start Engine while exploded shows the refusal message.
- Assemble returns the slider to 0%.
- Start Engine gives the RUNNING state with glow, lights and hover.
- The swatch click changes the paint and the label.
- The R, E and 2 keys work.
- Scripted tests:
  - The slider sequence 1 → 0 → 0.5 → 0 leaves no drift: start positions are cached, never added to.
  - Three rapid engine toggles end in the correct state.
  - Exploding while running auto-stops the engine.
  - A UI raycast hits the engine button.
- Not verified:
  - Audio was not listened to. The hum is generated in code; check it is audible.
  - Frame rate was not measured.
  - The 1920x1080 layout was checked only at a 1101x506 Game view.

## Known issues / remaining work
1. **Unity version:** this project uses 6000.6.7f1, but the module requires 6000.3.19f1, which is not installed. Ask Jeff whether 6000.6 is acceptable, or install 6000.3.19f1 and port the project. Scripts, FBX, materials and scenes should port, but test it.
2. **CW1 compulsory content is missing:**
   - All five tutorial stations and the minifigure bay are placeholders.
   - LegoWalk.fbx and the Lego brick .dae files must be downloaded from Blackboard.
   - Each student adds their own tutorial work and the UI-button minifig animation.
   - Don't show placeholders as finished work in the WIP video.
3. Group number and student names are not filled in (script headers say `Group [No.]`).
4. Station labels clip into pillars from some angles. The camera does not avoid pillars or gantry legs.
5. Wheel/hover-unit variants have not been built. That was optional.
6. Input conflicts with future tutorial scripts:
   - Garage keys are E, R, 1-4 and mouse drag.
   - Tutorial controllers should use arrows/WASD/Space.
   - The Tutorial 02-03 mouse-drag brick will compete with camera orbit. Possible fix: orbit with the right mouse button only, or disable `InspectionCamera` while at that station.

## For Codex (git)
New, untracked paths to review:
- `Assets/Garage/**` with .meta files
- `BlenderSource/HoverVehicle.blend`
- `Recordings/*.png` (test screenshots; probably add to .gitignore)
- `GARAGE_NOTES.md`

Modified:
- `ProjectSettings/ProjectSettings.asset` (runInBackground)
- `ProjectSettings/EditorBuildSettings.asset`

The repo has Git LFS configured. The .fbx (~245 KB) and .blend are small enough to commit normally.

## To record (28 Oct)
1. Open `OutpostGarage.unity`.
2. Set Game view to `1920x1080` (Game tab → aspect dropdown), then right-click the Game tab → Maximize (or Shift+Space).
3. Press Play. Click once in the 3D view before using keys.
4. Record with Xbox Game Bar (Win+Alt+R) or OBS.

### WIP shot list (~5 min)
| Time | Shot |
|---|---|
| 0:00-0:40 | Group intro and concept: an inspection/service bay for a hover vehicle, as an interactive diorama (CW2 direction). |
| 0:40-1:40 | Tutorial and minifig contributions, ONLY the ones that actually work by then. Each student narrates their own. |
| 1:40-2:40 | Orbit and zoom, then the explode slider to 100% and Assemble. Name the five groups. |
| 2:40-3:20 | Paint swatches and keys 1-4. Engine start, stop, and the refusal when exploded. |
| 3:20-4:20 | Planning: interaction flow, intended CW2 features, and roles. |
| 4:20-5:00 | Honest status: placeholders, Unity version question, next steps before 5 Nov and 12 Nov. |

Upload one MP4 named `3DIMD_2026_GroupNumber_WIP1.mp4` (per the Blackboard item; confirm with Jeff).

## Planning material (proposed design, for the group report to develop)
**Interaction flow:**
- Launch → framed view of the vehicle.
- Orbit/zoom ↔ explode slider ↔ assemble.
- Paint select at any time.
- Engine: assembled? → start → running → stop.
- Exploded → auto-stop.
- Reset view at any time.

**Use cases (actor: Visitor):**
- Inspect vehicle
- Separate components
- Reassemble
- Change paint
- Start/stop engine
- Reset view
- View tutorial station

**Domain/class sketch:**
- `InspectionCamera` → target `VehicleRig`
- `ExplodedView` holds `Part[]` (transform, offset, rotation)
- `PaintController` holds `PaintOption[]` and changes runtime paint material
- `EngineController` uses `ExplodedView`, glow materials, lights and an AudioSource
- `GarageUI` references all four, plus EventSystem/Canvas
- `DemoStation` holds title, contributor, status and content

**Roles to choose from (2 per student, from the brief):**
- Assets sourcing/creation
- Management versioning/presentations/meetings
- Design aesthetic/interaction/concept
- Coding interaction/movement-camera/animation-trigger
- Animation implementation
- Animation coding
