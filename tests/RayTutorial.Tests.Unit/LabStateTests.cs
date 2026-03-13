using RayTutorial.Domain;
using RayTutorial.Lab;

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
}
