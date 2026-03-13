namespace RayTutorial.Lessons;

public interface ILessonCatalog
{
    IReadOnlyList<LessonDescriptor> GetLessons();
}
