# Lesson Plan

## Teaching Philosophy

The tutorial should be organized around experiments, not chapters of text. Each lesson should let the user inspect a controlled rendering situation, change a few meaningful variables, and observe the result through both the beauty image and supporting AOVs.

The lesson system should avoid pretending that a teaching abstraction is the full implementation story. When the lesson simplifies something, it should say so explicitly.

## Lesson Structure

Each lesson should include:

- objective
- target scene
- default pane layout
- highlighted controls
- recommended AOVs
- guided actions
- explanation
- simplification notes
- follow-up experiments

## Module 1: Ray Queries and Visibility

Goals:

- establish what a ray represents in the renderer
- explain primary visibility in practical terms
- show what information a hit returns

Suggested interactions:

- inspect hit distance, normal, instance ID, and barycentrics
- switch between beauty and debug outputs
- compare direct ray hits against a simple conceptual diagram

Suggested scene:

- simple primitives and a few instances

## Module 2: Acceleration Structures

Goals:

- explain why brute force intersection testing does not scale
- introduce BVH intuition
- explain BLAS and TLAS roles

Suggested interactions:

- toggle instance counts
- compare rebuild and refit-style conceptual cases
- inspect instance IDs and bounds overlays

Suggested simplification note:

- real traversal behavior and builder heuristics are more complex than the lesson model

## Module 3: Vulkan Hardware Ray Tracing Pipeline

Goals:

- explain raygen, miss, closest-hit, and any-hit responsibilities
- show how trace calls map to visible results
- introduce the shader binding table at a high level

Suggested interactions:

- enable or disable shadow rays
- swap miss shaders or closest-hit responses in simplified demos
- inspect how different stages affect final output

Suggested simplification note:

- early lessons should describe the pipeline behavior before diving into full Vulkan setup complexity

## Module 4: Real-Time Game Ray Tracing

Goals:

- explain hybrid rendering
- show why games use limited ray budgets
- demonstrate common features such as hard shadows and reflections

Suggested interactions:

- switch between raster baseline, hybrid mode, and a more path-traced reference
- adjust ray counts or feature toggles
- compare raw and temporally accumulated outputs

Suggested note:

- temporal techniques and denoising are deep subjects; early treatment should stay conceptual

## Module 5: From Single Bounce to Path Tracing

Goals:

- introduce bounce depth, throughput, and indirect light
- explain why path tracing converges progressively
- make noise and convergence intuitive

Suggested interactions:

- change max bounce depth
- change samples per pixel
- inspect direct vs indirect AOVs
- compare low-sample and high-sample panes

Suggested scenes:

- Cornell-style scene
- glossy interior or product shot

## Module 6: Sampling and Noise

Goals:

- explain why some sampling strategies converge better than others
- connect material behavior to variance
- make noisy images diagnostically useful instead of confusing

Suggested interactions:

- compare low and higher sample counts
- show rough vs smooth material response
- inspect variance-related views if available

Suggested simplification note:

- detailed MIS and advanced sampling strategies can be extension topics, not early core content

## Module 7: Materials and BSDF Intuition

Goals:

- explain practical material categories used in the tutorial
- connect roughness and Fresnel intuition to visible outcomes
- keep shading educational without pretending to match every production shader system

Suggested interactions:

- switch between diffuse, conductor, dielectric, and emissive cases
- vary roughness and IOR-related controls where exposed
- compare beauty and specular-related AOVs

## Module 8: AOVs and Render Diagnostics

Goals:

- explain why render decomposition matters
- bridge tutorial rendering concepts to offline and VFX workflows
- make AOV inspection a normal part of analysis

Suggested AOVs:

- beauty
- albedo
- normal
- depth
- world position
- object or instance ID
- direct diffuse
- indirect diffuse
- direct specular
- indirect specular
- emission
- variance or convergence aid

Suggested interactions:

- compare beauty against component AOVs
- isolate why a noisy or expensive result occurs

## Module 9: Offline and VFX Renderer Architecture

Goals:

- explain how production renderers differ architecturally from game renderers
- keep vendor comparisons conceptual and high level
- explain why AOVs, shader graphs, and final-frame priorities matter

Topics:

- interactive preview versus final frame
- CPU-oriented versus GPU-oriented assumptions
- shading graph flexibility
- out-of-core or memory management concerns
- compositing-oriented output design
- practical biased versus less-biased tradeoffs

Suggested note:

- commercial renderers differ in many implementation details not visible from the outside; comparisons here are architectural, not exhaustive

## Module 10: Performance and Cost

Goals:

- connect visual quality to computational cost
- explain traversal, shading, divergence, and structure build costs
- show why different pipelines optimize different variables

Suggested interactions:

- compare different bounce depths
- compare more and fewer instances
- compare material complexity presets
- inspect frame time and structure update costs

## Module 11: Capstone Lab

Goals:

- give the user a place to synthesize the prior lessons
- compare hybrid and offline-leaning approaches in one scene
- encourage independent exploration with bounded guidance

Suggested capabilities:

- multiple pane comparison
- free parameter editing
- preset restoration
- lesson-linked analysis prompts

## Initial Scene Set

- `PrimitiveDiagnostics`
  - simple primitives, clear hit behavior, great for debugging and first principles

- `CornellVariant`
  - ideal for bounce lighting, convergence, and AOV inspection

- `GlossyInterior`
  - useful for reflections, roughness, denoising, and path tracing comparisons

- `InstancingLayout`
  - useful for BLAS and TLAS discussion and performance tradeoffs

The MVP can ship with the first three scenes if scope needs to stay tight.
