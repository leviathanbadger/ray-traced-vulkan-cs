# Roadmap

## Implementation Order

The project should be built in layers that produce a usable teaching tool early rather than delaying all value until the renderer is deep.

## Phase 1: Repository Foundation

- create the .NET solution
- create all planned projects with a clean reference graph
- establish formatting, testing, and build conventions
- add a docs-first architecture baseline

Exit criteria:

- solution builds with placeholder projects
- project boundaries are documented and enforced

## Phase 2: Application Shell

- implement main window and docked workstation layout
- add pane management for single, split, and quad view
- add lesson navigation shell
- add placeholder control panels and metrics strip

Exit criteria:

- application launches and pane layout changes work
- lesson metadata can be loaded and displayed

## Phase 3: Rendering Foundation

- create Vulkan bootstrap layer
- create swapchain and viewport integration
- implement camera controls
- add frame timing and basic debug reporting

Exit criteria:

- a scene can render in a stable viewport
- camera motion and resize handling work

## Phase 4: Lesson and Preset Infrastructure

- define lesson schema
- define preset and app-state mutation model
- bind lesson actions to live application state
- support recommended AOV and layout changes from lesson content

Exit criteria:

- a lesson can apply a reproducible scene and renderer state

## Phase 5: Core Ray Tracing Lessons

- implement ray visibility and hit debug modes
- implement acceleration structure demonstrations
- implement the first hardware ray tracing pipeline lesson views

Exit criteria:

- the user can step through the first three modules with live feedback

## Phase 6: Path Tracing and AOVs

- add progressive accumulation
- add bounce depth controls
- add key AOV outputs
- add direct and indirect lighting decomposition where feasible

Exit criteria:

- path tracing lessons are visually useful and stable

## Phase 7: Production Concepts and Comparison

- add multi-pane synchronized comparison workflows
- add VFX renderer architecture lessons
- add performance-focused lesson content and overlays

Exit criteria:

- the application supports guided comparison between real-time and offline-leaning modes

## Phase 8: Polish and Validation

- refine lessons and presets
- improve visual presentation of debug outputs
- validate behavior on the target RTX 3090 hardware
- document setup and known limitations

Exit criteria:

- the MVP is credible as a durable teaching tool

## MVP Backlog

Priority 0:

- solution and project scaffolding
- basic app shell
- Vulkan viewport
- lesson schema

Priority 1:

- `PrimitiveDiagnostics` scene
- `CornellVariant` scene
- ray visibility lesson
- acceleration structure lesson

Priority 2:

- path tracing accumulation
- key AOVs
- split-view comparison
- preset system

Priority 3:

- `GlossyInterior` scene
- real-time hybrid RT lesson path
- VFX architecture lesson
- performance overlays

## Deferred Topics

- advanced denoising techniques beyond core conceptual treatment
- advanced sampling strategies such as more complete MIS lessons
- broader hardware support and fallback renderer parity
- deeper commercial renderer product comparisons
- expanded asset pipeline and larger scene library
