namespace RayTutorial.Lab;

public interface ILabPresetCatalog
{
    IReadOnlyList<LabPreset> GetPresets();
}
