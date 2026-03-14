using System.ComponentModel;
using System.Runtime.CompilerServices;
using RayTutorial.Domain;
using RayTutorial.Rendering;

namespace RayTutorial.Lab;

public sealed class LabState : ILabState
{
    private readonly Dictionary<string, RenderSurfaceState> renderSurfacesById = new();
    private readonly Dictionary<string, RenderOutletState> renderOutletsById = new();
    private string selectedLessonId = string.Empty;
    private string selectedSceneId = string.Empty;
    private string selectedLayoutName = string.Empty;
    private AovKind selectedAov = AovKind.Beauty;
    private RenderResolution sharedRenderResolution = RenderResolution.Default;

    public event PropertyChangedEventHandler? PropertyChanged;

    public LabState()
    {
        ResetRenderTopology();
    }

    public string SelectedLessonId
    {
        get => selectedLessonId;
        set => SetProperty(ref selectedLessonId, value);
    }

    public string SelectedSceneId
    {
        get => selectedSceneId;
        set => SetProperty(ref selectedSceneId, value);
    }

    public string SelectedLayoutName
    {
        get => selectedLayoutName;
        set => SetProperty(ref selectedLayoutName, value);
    }

    public AovKind SelectedAov
    {
        get => selectedAov;
        set => SetProperty(ref selectedAov, value);
    }

    public RenderResolution SharedRenderResolution
    {
        get => sharedRenderResolution;
        set
        {
            if (!SetProperty(ref sharedRenderResolution, value))
            {
                return;
            }

            foreach (var surface in renderSurfacesById.Values.ToArray())
            {
                renderSurfacesById[surface.SurfaceId] = surface with { Resolution = value };
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RenderSurfaces)));
        }
    }

    public IReadOnlyCollection<RenderSurfaceState> RenderSurfaces => renderSurfacesById.Values;

    public IReadOnlyCollection<RenderOutletState> RenderOutlets => renderOutletsById.Values;

    public void ApplyPreset(LabPreset preset)
    {
        SelectedLessonId = preset.LessonId;
        SelectedSceneId = preset.SceneId;
        SelectedLayoutName = preset.LayoutName;
        SelectedAov = preset.DefaultAov;
        ResetRenderTopology();
    }

    public string GetRenderSurfaceId(string outletId) => renderOutletsById[outletId].SurfaceId;

    public RenderSurfaceDescriptor GetRenderSurfaceDescriptor(string surfaceId)
    {
        var surface = renderSurfacesById[surfaceId];
        return new RenderSurfaceDescriptor(surface.SurfaceId, surface.SceneId, surface.Resolution);
    }

    public AovKind GetSelectedAov(string outletId) => renderOutletsById[outletId].SelectedAov;

    public void SetSelectedAov(string outletId, AovKind aov)
    {
        var outlet = renderOutletsById[outletId];
        if (outlet.SelectedAov == aov)
        {
            return;
        }

        renderOutletsById[outletId] = outlet with { SelectedAov = aov };
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RenderOutlets)));
    }

    public void ForkSurfaceForOutlet(string outletId, string forkReason)
    {
        var outlet = renderOutletsById[outletId];
        var sourceSurface = renderSurfacesById[outlet.SurfaceId];
        var forkedSurfaceId = $"{outletId}-{forkReason}-{renderSurfacesById.Count}";
        var forkedSurface = sourceSurface with
        {
            SurfaceId = forkedSurfaceId,
            OutputSetId = forkReason
        };

        renderSurfacesById[forkedSurfaceId] = forkedSurface;
        renderOutletsById[outletId] = outlet with { SurfaceId = forkedSurfaceId };
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RenderSurfaces)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RenderOutlets)));
    }

    private void ResetRenderTopology()
    {
        renderSurfacesById.Clear();
        renderOutletsById.Clear();

        renderSurfacesById["lesson-main"] = new RenderSurfaceState(
            "lesson-main",
            selectedSceneId,
            sharedRenderResolution,
            "lesson-camera",
            "PathTracingPreview",
            "default",
            8,
            3);

        renderOutletsById["beauty"] = new RenderOutletState("beauty", "lesson-main", AovKind.Beauty);
        renderOutletsById["comparison"] = new RenderOutletState("comparison", "lesson-main", AovKind.Beauty);
        renderOutletsById["aov-inspector"] = new RenderOutletState("aov-inspector", "lesson-main", AovKind.Variance);
        renderOutletsById["performance-lens"] = new RenderOutletState("performance-lens", "lesson-main", AovKind.InstanceId);

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RenderSurfaces)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RenderOutlets)));
    }

    private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        if (propertyName == nameof(SelectedSceneId))
        {
            foreach (var surface in renderSurfacesById.Values.ToArray())
            {
                renderSurfacesById[surface.SurfaceId] = surface with { SceneId = selectedSceneId };
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RenderSurfaces)));
        }

        return true;
    }
}
