using Game;
using DelegateCommandNM;
using ViewModelBaseNM;
using Args;
using System.Collections.ObjectModel;
using LabFieldNM;
using GameDifficultyNM;
using Enums;
using Microsoft.Maui.Controls.Shapes;

namespace GameViewModelNM;

public class GameViewModel : ViewModelBase
{
    #region Commands and Events
    public DelegateCommand NewGameCommand { get; private set; }
    public DelegateCommand LoadGameCommand { get; private set; }
    public DelegateCommand SaveGameCommand { get; private set; }
    public DelegateCommand ExitCommand { get; private set; }

    public DelegateCommand MoveLeft { get; private set; }
    public DelegateCommand MoveRight { get; private set; }
    public DelegateCommand MoveUp { get; private set; }
    public DelegateCommand MoveDown { get; private set; }

    public event EventHandler NewGame;
    public event EventHandler LoadGame;
    public event EventHandler SaveGame;
    public event EventHandler ExitGame;
    public event EventHandler TimePaused;
    public event EventHandler PlayerWon;
    #endregion

    #region Properties
    public ObservableCollection<LabField> Fields { get; set; }
    public ObservableCollection<GameDifficultyViewModel> DifficultyLevels { get; set; }
    public GameDifficultyViewModel Difficulty
    {
        get => _difficulty;
        set
        {
            _difficulty = value;
            OnPropertyChanged();
        }
    }

    private Boolean _isGamePaused;
    public Boolean IsGamePaused
    {
        get => _isGamePaused;
        set
        {
            if (_isGamePaused != value)
            {
                _isGamePaused = value;
                OnTimePaused();
                OnPropertyChanged();
            }
        }
    }

    private Int32 _tableSize;
    public int TableSize
    {
        get => _tableSize;
        set
        {
            _tableSize = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(GameTableRows));
            OnPropertyChanged(nameof(GameTableColumns));
        }
    }

    public RowDefinitionCollection GameTableRows
    {
        get => new (Enumerable.Repeat(new RowDefinition(GridLength.Star), TableSize).ToArray());
    }

    public ColumnDefinitionCollection GameTableColumns
    {
        get => new(Enumerable.Repeat(new ColumnDefinition(GridLength.Star), TableSize).ToArray());
    }

    public String GameTime { get { return _gameTime.ToString("g"); } }
    private TimeSpan _gameTime;

    #endregion

    private GameDifficultyViewModel _difficulty = null!;
    private readonly GameModel _model = null!;

    public GameViewModel(GameModel model)
    {
        _model = model;
        _model.NewGame += GameModel_NewGame;
        _model.PlayerMoved += GameModel_PlayerMoved;
        _model.PlayerWon += GameModel_PlayerWon;

        NewGameCommand = new DelegateCommand(param => OnNewGame());
        LoadGameCommand = new DelegateCommand(param => OnLoadGame());
        SaveGameCommand = new DelegateCommand(param => OnSaveGame());
        ExitCommand = new DelegateCommand(param => OnExitGame());
        MoveLeft = new DelegateCommand(param => MovedLeft());
        MoveRight = new DelegateCommand(param => MovedRight());
        MoveUp = new DelegateCommand(param => MovedUp());
        MoveDown = new DelegateCommand(param => MovedDown());

        DifficultyLevels = new ObservableCollection<GameDifficultyViewModel>
            {
                new GameDifficultyViewModel { Difficulty = MapSize.Small },
                new GameDifficultyViewModel { Difficulty = MapSize.Medium },
                new GameDifficultyViewModel { Difficulty = MapSize.Large }
            };
        Difficulty = DifficultyLevels[1];

        Fields = new();

        _isGamePaused = false;
    }

    #region Game events receivers

    private void GameModel_NewGame(object? sender, NewGameEventArgs info)
    {
        System.Diagnostics.Debug.WriteLine("KIIRAS fuggvenyben: ");

        Fields.Clear();
        InitMap();
        _gameTime = TimeSpan.Zero;
        RefreshTable();
        TableSize = _model.MapSize;

        System.Diagnostics.Debug.WriteLine(Fields.Count);
        
    }

    private void InitMap()
    {
        for (Int32 i = 0; i < _model.MapSize; i++)
        {
            for (Int32 j = 0; j < _model.MapSize; j++)
            {
                Fields.Add(new LabField(i, j));
            }
        }
    }

    private void RefreshTable()
    {
        IsGamePaused = false;
        foreach (LabField field in Fields)
        {
            if (field.IsLit)
            {
                field.IsLit = false;
            }
            if (field.IsPlayer)
            {
                field.IsPlayer = false;
            }
        }
    }

    private void GameModel_PlayerMoved(object? sender, PlayerMovedEventArgs info)
    {
        foreach (System.Drawing.Point p in info.CellsToFree)
        {
            Fields[GetListCoordinate(p)].IsLit = false;
            Fields[GetListCoordinate(p)].IsPlayer = false;
        }

        foreach (System.Drawing.Point p in info.CellsToLight)
        {
            Fields[GetListCoordinate(p)].IsLit = true;
        }

        Fields[GetListCoordinate(info.PlayerPosition)].IsPlayer = true;
    }

    private void GameModel_PlayerWon(object? sender, EventArgs info)
    {
        IsGamePaused = true;
        OnPlayerWon();
    }

    #endregion

    public void Timer_Tick(object? sender, EventArgs e)
    {
        _gameTime = _gameTime.Add(TimeSpan.FromSeconds(1));
        OnPropertyChanged(nameof(GameTime));
    }

    #region Movement private methods
    private void MovedUp()
    {
        Move(Arrow.Up);
    }

    private void MovedDown()
    {
        Move(Arrow.Down);
    }

    private void MovedLeft()
    {
        Move(Arrow.Left);
    }

    private void MovedRight()
    {
        Move(Arrow.Right);
    }

    private void Move(Arrow arrow)
    {
        if (!IsGamePaused)
        {
            _model.MovePlayer(arrow);
        }
    }

    private int GetListCoordinate(System.Drawing.Point p)
    {
        return p.Y * _model.MapSize + p.X;
    }

    #endregion

    #region Event callers
    private void OnNewGame()
    {
        NewGame?.Invoke(this, EventArgs.Empty);
    }

    private void OnLoadGame()
    {
        LoadGame?.Invoke(this, EventArgs.Empty);
    }

    private void OnSaveGame()
    {
        SaveGame?.Invoke(this, EventArgs.Empty);
    }

    private void OnExitGame()
    {
        ExitGame?.Invoke(this, EventArgs.Empty);
    }

    private void OnTimePaused()
    {
        TimePaused?.Invoke(this, EventArgs.Empty);
    }

    private void OnPlayerWon()
    {
        PlayerWon?.Invoke(this, EventArgs.Empty);
    }
    #endregion
}