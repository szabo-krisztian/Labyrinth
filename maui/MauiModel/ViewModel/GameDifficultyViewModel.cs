using Enums;
using ViewModelBaseNM;

namespace GameDifficultyNM;

public class GameDifficultyViewModel : ViewModelBase
{
    private MapSize _difficulty;

    public MapSize Difficulty
    {
        get => _difficulty;
        set
        {
            _difficulty = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(DifficultyText));
        }
    }

    public string DifficultyText => _difficulty.ToString();
}