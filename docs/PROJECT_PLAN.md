# Project Plan

## Summary

This project will be a desktop teaching application and rendering lab that explains modern ray tracing through live experiments, comparisons, and targeted lesson content. It is not intended to be a full production renderer or general-purpose engine. The goal is to make the mechanics, tradeoffs, and architecture of ray tracing concrete for graphics programmers.

## Objectives

- Teach how ray tracing works in real-time and offline rendering contexts
- Explain the major architectural concepts behind modern GPU ray tracing
- Show where game engines and offline renderers make different tradeoffs
- Provide direct visual feedback through AOVs, debug overlays, and pane comparison
- Keep the codebase modular enough for long-term expansion

## Non-Goals

- Full parity with commercial renderers such as Arnold, Redshift, Karma, or Mantra
- A complete survey of foundational rasterization topics
- Vendor-specific reverse engineering of commercial renderer internals
- Shipping a general-purpose DCC renderer or full-featured engine
- Supporting hardware that lacks Vulkan ray tracing through identical code paths

## Audience

Primary:

- graphics programmers

Secondary:

- technical artists
- advanced students with rendering background

The content should assume familiarity with basic 3D math and rasterization concepts. It should not assume deep experience with Vulkan ray tracing, path tracing integrators, or production AOV workflows.

## Product Model

The application should function as a guided lab.

Each lesson is built around:

- one or more curated scenes
- one or more presets
- a bounded set of parameters to manipulate
- recommended AOVs or debug views
- a compact technical explanation
- a clear note where the model is intentionally simplified

This supports both guided learning and open-ended experimentation.

## Platform and Technology Direction

- Language: C#
- Runtime: modern .NET
- Application type: desktop
- Platforms: Windows and Linux
- Rendering backend: Vulkan
- UI direction: workstation-style technical tool

Rationale:

- Vulkan is the right backend for an honest explanation of hardware ray tracing pipelines.
- A desktop app is a better fit than browser deployment for GPU feature access, tooling, and performance.
- A workstation layout supports multi-pane comparisons, AOV inspection, and large control surfaces.

## User Experience Principles

- Prioritize live visual feedback over long passive text sections.
- Keep explanatory text close to the controls and images it discusses.
- Make important comparisons one click away.
- Prefer a few well-curated scenes over a large undirected scene library.
- Surface performance and quality tradeoffs directly in the UI.
- Distinguish between tutorial abstractions and real implementation complexity.

## Core Features

- one, two, and four pane synchronized view modes
- curated scenes optimized for specific lessons
- renderer presets that change scene, camera, and parameter state together
- AOV selection and per-pane comparison
- progressive accumulation for path-traced modes
- debug overlays for geometry, traversal, and renderer state
- lesson panel with concept, inspect, and do sections
- persistent project-wide coordinate and unit conventions

## Teaching Strategy

The project should minimize time spent re-teaching rasterization basics. Rasterization is used as contrast where it helps explain why ray tracing is valuable or why hybrid renderers exist.

The material should emphasize:

- ray traversal and hit evaluation
- acceleration structures
- hardware ray tracing pipeline stages
- hybrid game rendering
- path tracing and sampling
- AOVs and diagnostics
- architectural differences between real-time and offline systems

## Scope Boundary for the First Useful Release

The first useful version should be a coherent teaching tool, not a partial engine skeleton.

It should include:

- a stable application shell
- a Vulkan-backed viewport
- lesson metadata and preset orchestration
- 3 curated scenes
- a useful AOV/debug set
- lesson modules covering core ray tracing, path tracing, and production concepts

## Success Criteria

The project is successful when a graphics programmer can use it to:

- understand how a ray tracing pipeline is structured
- observe how BVH-related concepts change behavior and cost
- compare real-time hybrid ray tracing to path tracing
- inspect AOVs and understand why offline pipelines care about them
- reason about quality versus cost tradeoffs with direct visual evidence
