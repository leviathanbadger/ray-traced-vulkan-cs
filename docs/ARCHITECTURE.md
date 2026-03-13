# Architecture

## High-Level Shape

The application should be split into focused projects so rendering, lessons, UI, and domain concepts can evolve independently. The solution should favor clear boundaries over short-term convenience.

## Proposed Solution Layout

- `RayTutorial.App`
  - executable entry point
  - dependency injection and composition root
  - startup configuration

- `RayTutorial.UI`
  - Avalonia views and styles
  - docking and pane layout
  - parameter editors
  - lesson panel presentation

- `RayTutorial.Lessons`
  - lesson graph
  - topic metadata
  - lesson step definitions
  - simplification notes
  - action and preset descriptors

- `RayTutorial.Lab`
  - parameter model for exposed controls
  - preset application logic
  - pane linking and comparison orchestration
  - command routing from lesson content into the live app state

- `RayTutorial.Domain`
  - shared enums and immutable data contracts
  - coordinate system conventions
  - AOV definitions
  - material parameter contracts
  - scene and camera descriptors

- `RayTutorial.Scene`
  - runtime scene graph
  - transforms and instances
  - cameras and lights
  - scene loading into runtime structures

- `RayTutorial.Assets`
  - asset manifest parsing
  - texture and mesh import integration
  - scene package descriptions

- `RayTutorial.Rendering`
  - backend-neutral renderer contracts
  - frame and pass interfaces
  - renderer settings contracts
  - common render data abstractions

- `RayTutorial.Rendering.Vulkan`
  - Vulkan instance and device setup
  - swapchain management
  - descriptor and resource lifetime management
  - acceleration structure builds
  - ray tracing pipeline setup
  - synchronization and frame pacing

- `RayTutorial.Rendering.PathTracing`
  - tutorial-focused integrator logic
  - accumulation control
  - AOV generation orchestration
  - quality mode definitions

- `RayTutorial.Rendering.Debug`
  - overlays
  - visualization modes
  - GPU timing and counters
  - traversal and instance debug displays

- `RayTutorial.Shaders`
  - shader source tree
  - compilation metadata
  - reflection metadata
  - shader packaging support

- `RayTutorial.Infrastructure`
  - configuration
  - logging
  - persistence
  - serialization helpers

- `RayTutorial.Tests.Unit`
  - domain and lesson tests

- `RayTutorial.Tests.Integration`
  - app and renderer integration tests where practical

- `RayTutorial.Tools.ShaderBuild`
  - shader compilation utility

- `RayTutorial.Tools.AssetPrep`
  - optional preprocessing of demo scenes and supporting assets

## Reference Graph Principles

- `RayTutorial.Domain` should be low-level and broadly reusable.
- `RayTutorial.Rendering` should depend on `RayTutorial.Domain`, not UI or lesson projects.
- `RayTutorial.Rendering.Vulkan` should implement rendering interfaces without owning lesson logic.
- `RayTutorial.Lessons` should describe what to teach, not how the renderer works internally.
- `RayTutorial.Lab` is the bridge between lesson actions and live app state.
- `RayTutorial.UI` should talk to presentation/view-model layers rather than raw Vulkan objects.

## UI Architecture

The UI should feel like a rendering workstation rather than a slide deck.

Main regions:

- lesson/topic navigator
- viewport grid
- control inspector
- lesson/reference tabs
- optional bottom metrics strip

Recommended patterns:

- MVVM-style state flow for Avalonia
- renderer hosted behind a viewport control boundary
- declarative control descriptors so lessons can refer to parameters by stable identifiers
- layout presets for switching between single, split, and quad comparison views

## Lesson System Architecture

Lessons should be data-driven.

Each lesson module should define:

- stable lesson ID
- title and summary
- prerequisite lessons
- recommended scene
- recommended pane layout
- recommended AOVs
- control bindings to highlight
- preset actions
- explanation text
- simplification notes

Each lesson step should be able to drive the app into a known state without ad hoc UI scripting.

## Scene Architecture

The runtime scene model should be small and explicit.

Key requirements:

- Y-up right-handed transforms
- meter-based scale assumptions
- separation between imported asset data and instantiated runtime scene data
- support for many instances to demonstrate BLAS and TLAS behavior
- stable IDs for meshes, materials, instances, and lights so lessons can reference them

## Rendering Architecture

The renderer should support multiple teaching modes without becoming a full renderer framework.

Targeted rendering modes:

- debug ray visibility mode
- single-bounce ray-traced effects mode
- hybrid-style real-time mode
- progressive path tracing mode

Required output capabilities:

- beauty
- albedo
- normal
- depth
- world position
- object or instance ID
- direct and indirect lighting components where available
- variance or convergence aid where practical

## Coordinate and Unit Conventions

These conventions must be fixed and documented early:

- handedness: right-handed
- up axis: `+Y`
- unit: meters
- camera and import code must document forward and projection assumptions explicitly

## Performance and Debug Instrumentation

This is a teaching tool, so instrumentation is part of the product, not just developer support.

Expose where practical:

- frame time
- samples per pixel
- accumulation state
- resolution
- bounce depth
- ray count estimates
- BLAS and TLAS build or update timing
- warning badges for unsupported settings or hardware constraints

## Testing Strategy

Initial testing focus:

- lesson metadata validation
- preset application correctness
- coordinate system and transform consistency
- scene manifest loading
- parameter binding stability

Renderer correctness testing should focus first on:

- initialization
- shader and pipeline setup integrity
- deterministic CPU-side helpers

Visual regression testing can be introduced later if the build pipeline supports it.
