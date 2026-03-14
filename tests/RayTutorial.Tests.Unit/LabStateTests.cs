using RayTutorial.Domain;
using RayTutorial.Lab;
using RayTutorial.Rendering;

namespace RayTutorial.Tests.Unit;

public sealed class LabStateTests
{
    [Fact]
    public void ApplyPresetUpdatesLiveSelectionState()
    {
        var state = new LabState();
        var preset = new LabPreset(
            "preset",
            "lesson",
            "Display",
            "Description",
            "CornellVariant",
            "Quad View",
            AovKind.Variance);

        state.ApplyPreset(preset);

        Assert.Equal("lesson", state.SelectedLessonId);
        Assert.Equal("CornellVariant", state.SelectedSceneId);
        Assert.Equal("Quad View", state.SelectedLayoutName);
        Assert.Equal(AovKind.Variance, state.SelectedAov);
    }

    [Fact]
    public void EnsureOutputAvailableForOutletForksSurfaceWhenOutputIsMissing()
    {
        var state = new LabState();

        state.EnsureOutputAvailableForOutlet("beauty", AovKind.Depth);

        var surfaceId = state.GetRenderSurfaceId("beauty");
        var comparisonSurfaceId = state.GetRenderSurfaceId("comparison");
        var surface = state.GetRenderSurfaceDescriptor(surfaceId);

        Assert.NotEqual("lesson-main", surfaceId);
        Assert.Equal("lesson-main", comparisonSurfaceId);
        Assert.Contains(AovKind.Depth, surface.EnabledOutputs);
    }

    [Fact]
    public void EnsureOutputAvailableForOutletDoesNotForkWhenOutputAlreadyExists()
    {
        var state = new LabState();
        var originalSurfaceId = state.GetRenderSurfaceId("beauty");

        state.EnsureOutputAvailableForOutlet("beauty", AovKind.Beauty);

        Assert.Equal(originalSurfaceId, state.GetRenderSurfaceId("beauty"));
    }

    [Fact]
    public void PresentationModeStaysWithSharedSurfaceForRawVsDenoised()
    {
        var state = new LabState();
        var sourceSurfaceId = state.GetRenderSurfaceId("beauty");

        state.BindOutletToSurface("comparison", sourceSurfaceId);
        state.SetSelectedSourceOutput("comparison", AovKind.Beauty);
        state.SetPresentationMode("comparison", PresentationMode.Denoised);

        Assert.Equal(sourceSurfaceId, state.GetRenderSurfaceId("comparison"));
        Assert.Equal(PresentationMode.Denoised, state.GetPresentationMode("comparison"));
    }

    [Fact]
    public void ApplySurfaceSettingsToOutletForksWhenSurfaceIsShared()
    {
        var state = new LabState();

        state.ApplySurfaceSettingsToOutlet("beauty", "PathTracingReference", 64, 6);

        var beautySurfaceId = state.GetRenderSurfaceId("beauty");
        var comparisonSurfaceId = state.GetRenderSurfaceId("comparison");
        var beautySurface = state.GetRenderSurfaceState(beautySurfaceId);

        Assert.NotEqual("lesson-main", beautySurfaceId);
        Assert.Equal("lesson-main", comparisonSurfaceId);
        Assert.Equal("PathTracingReference", beautySurface.RenderMode);
        Assert.Equal(64, beautySurface.SamplesPerPixel);
        Assert.Equal(6, beautySurface.MaxBounces);
    }

    [Fact]
    public void ApplySurfaceSettingsToOutletUpdatesExistingDedicatedSurface()
    {
        var state = new LabState();
        state.ForkSurfaceForOutlet("beauty", "clone");
        var dedicatedSurfaceId = state.GetRenderSurfaceId("beauty");

        state.ApplySurfaceSettingsToOutlet("beauty", "HybridRayQuery", 32, 1);

        Assert.Equal(dedicatedSurfaceId, state.GetRenderSurfaceId("beauty"));
        var surface = state.GetRenderSurfaceState(dedicatedSurfaceId);
        Assert.Equal("HybridRayQuery", surface.RenderMode);
        Assert.Equal(32, surface.SamplesPerPixel);
        Assert.Equal(1, surface.MaxBounces);
    }
}
