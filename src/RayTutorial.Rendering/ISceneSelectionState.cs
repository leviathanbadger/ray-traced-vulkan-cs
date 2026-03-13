using System.ComponentModel;

namespace RayTutorial.Rendering;

public interface ISceneSelectionState : INotifyPropertyChanged
{
    string SelectedSceneId { get; }
}
