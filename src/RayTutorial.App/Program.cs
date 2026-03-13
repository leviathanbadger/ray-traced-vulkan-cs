namespace RayTutorial.App;

public static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("RayTutorial scaffold");
        Console.WriteLine("Target runtime: .NET 10");
        Console.WriteLine("Rendering backend: Vulkan");
        Console.WriteLine("Coordinate system: right-handed, Y-up, meters");
        Console.WriteLine($"Arguments: {string.Join(' ', args)}");
    }
}
