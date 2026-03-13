using RayTutorial.Domain;

namespace RayTutorial.Tests.Unit;

public sealed class CoordinateSystemTests
{
    [Fact]
    public void HoudiniStyleConventionUsesAgreedProjectDefaults()
    {
        var convention = CoordinateSystem.HoudiniStyle;

        Assert.Equal(Handedness.RightHanded, convention.Handedness);
        Assert.Equal(UpAxis.Y, convention.UpAxis);
        Assert.Equal(WorldUnit.Meter, convention.Unit);
    }
}
