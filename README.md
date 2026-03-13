# Ray Tracing Tutorial

Interactive desktop tutorial and lab for modern ray tracing in C#.

The application is intended to teach how ray tracing fits into:

- modern real-time game rendering pipelines
- offline and VFX-oriented rendering pipelines
- production renderer architecture at a conceptual level

This repository currently contains the project plan and an initial .NET 10 solution scaffold. Rendering and UI dependencies have not been integrated yet.

## Goals

- Build a cross-platform desktop application for Windows and Linux
- Use Vulkan as the rendering backend
- Target graphics programmers first, while remaining useful to technical artists
- Favor teaching clarity over total completeness
- Be explicit when lesson material uses simplifications
- Use a Houdini-style world convention: right-handed, Y-up, meters

## Product Shape

The intended application is a guided lab rather than a passive slideshow tutorial.

Core UI regions:

- viewport area with one, two, or four synchronized render panes
- control lab with renderer, sampling, AOV, and scene controls
- lesson/reference panel with explanations, presets, and guided experiments

## Documentation Map

- [docs/PROJECT_PLAN.md](/home/bslade/repos/leviathanbadger/ray-traced-vulkan-cs/docs/PROJECT_PLAN.md)
- [docs/ARCHITECTURE.md](/home/bslade/repos/leviathanbadger/ray-traced-vulkan-cs/docs/ARCHITECTURE.md)
- [docs/LESSON_PLAN.md](/home/bslade/repos/leviathanbadger/ray-traced-vulkan-cs/docs/LESSON_PLAN.md)
- [docs/ROADMAP.md](/home/bslade/repos/leviathanbadger/ray-traced-vulkan-cs/docs/ROADMAP.md)
- [docs/DECISIONS.md](/home/bslade/repos/leviathanbadger/ray-traced-vulkan-cs/docs/DECISIONS.md)

Repository-level build files:

- [Directory.Build.props](/home/bslade/repos/leviathanbadger/ray-traced-vulkan-cs/Directory.Build.props)
- [global.json](/home/bslade/repos/leviathanbadger/ray-traced-vulkan-cs/global.json)

## Working Assumptions

- Primary test hardware: NVIDIA RTX 3090
- Do not make newer-than-3090-specific features central to the experience
- Vulkan ray tracing is required for the full experience
- Windows and Linux are in scope
- macOS is intentionally out of scope

## Near-Term Implementation Priorities

1. Create the multi-project .NET solution and project reference graph.
2. Build the application shell and docked workstation-style UI.
3. Stand up the Vulkan renderer foundation and viewport.
4. Implement lesson metadata, presets, and comparison panes.
5. Add the first diagnostic scene and core ray visibility lesson.
