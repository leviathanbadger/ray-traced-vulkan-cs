namespace RayTutorial.Domain;

public enum UpAxis
{
    Y
}

public enum Handedness
{
    RightHanded
}

public enum WorldUnit
{
    Meter
}

public sealed record CoordinateSystem(
    Handedness Handedness,
    UpAxis UpAxis,
    WorldUnit Unit)
{
    public static CoordinateSystem HoudiniStyle { get; } =
        new(Handedness.RightHanded, UpAxis.Y, WorldUnit.Meter);
}
