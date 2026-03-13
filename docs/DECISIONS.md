# Decisions

## Current Project Decisions

### Platform

- Windows and Linux are in scope.
- macOS is out of scope.

Reason:

- Vulkan ray tracing support and validation are a better fit on Windows and Linux for this project.

### Rendering Backend

- Vulkan is the primary and only planned rendering backend.

Reason:

- The project is specifically intended to explain modern hardware ray tracing in a technically honest way.

### Application Type

- The product is a desktop application, not a browser application.

Reason:

- Desktop deployment better supports the required GPU features, tooling, and performance model.

### Language

- C# is the implementation language.

Reason:

- It satisfies the project requirement and supports clean multi-project solution organization.

### UI Toolkit

- Avalonia is the selected desktop UI toolkit.

Reason:

- It supports Windows and Linux cleanly, works well with the planned workstation-style layout, and keeps the C# desktop stack cohesive.

### Audience

- The primary audience is graphics programmers.
- Technical-artist-relevant context is welcome when it improves understanding.

### Teaching Priority

- Favor clarity over completeness.
- Explicitly note when a lesson uses a simplification or abstraction.

### Coordinate System

- Right-handed
- Y-up
- Unit is meter

Reason:

- Match Houdini-style conventions throughout the project.

### Hardware Baseline

- The primary validation target is an NVIDIA RTX 3090.
- Do not make newer hardware features central to the core lessons.

### Lesson Style

- The application should be a guided lab with a workstation-style UI.
- The tutorial layer should drive experiments and presets rather than replace the application with long text sequences.

### Content Scope

- Rasterization basics are not a major focus.
- Real-time hybrid ray tracing and offline path tracing are both core parts of the curriculum.
- VFX/commercial renderer coverage should stay conceptual and architectural rather than vendor-forensic.

## Open Questions for Future Work

- long-term docking approach beyond the initial shell layout
- shader authoring toolchain details
- scene import pipeline details
- lesson content serialization format
- whether CPU-only educational tracing helpers should exist as a separate runtime path
