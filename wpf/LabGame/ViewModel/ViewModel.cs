using DelegateCommandNM;
using EnumsNM;
using EventArgsNM;
using GameModelNM;
using LabFieldNM;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using ViewModelBaseNM;

namespace ViewModelNM;

public class ViewModel : ViewModelBase
{
    #region App and ThisViewModel EventHandlers, connectors
    public DelegateCommand NewGameCommand { get; private set; }
    public DelegateCommand LoadGameCommand { get; private set; }
    public DelegateCommand SaveGameCommand { get; private set; }
    public DelegateCommand ExitCommand { get; private set; }

    public event EventHandler? NewGame;
    public event EventHandler? LoadGame;
    public event EventHandler? SaveGame;
    public event EventHandler? PauseGame;
    public event EventHandler? ExitGame;
    #endregion

    #region Private fields
    private readonly GameModel _model = null!;
    private Point _previousPlayerPos;
    private TimeSpan _gameTime;
    private MapSize _mapSize;
    private Boolean _gameIsPaused;
    #endregion

    #region Properties
    public ObservableCollection<LabField> Fields { get; private set; } = null!;
    public String GameTime { get { return _gameTime.ToString("g"); } }
    public Boolean IsGameEasy
    {
        get { return _mapSize == MapSize.Small; }
        set
        {
            if (_mapSize == MapSize.Small)
                return;

            _mapSize = MapSize.Small;
            OnPropertyChanged(nameof(IsGameEasy));
            OnPropertyChanged(nameof(IsGameMedium));
            OnPropertyChanged(nameof(IsGameHard));
        }
    }
    public Boolean IsGameMedium
    {
        get { return _mapSize == MapSize.Medium; }
        set
        {
            if (_mapSize == MapSize.Medium)
                return;

            _mapSize = MapSize.Medium;
            OnPropertyChanged(nameof(IsGameEasy));
            OnPropertyChanged(nameof(IsGameMedium));
            OnPropertyChanged(nameof(IsGameHard));
        }
    }
    public Boolean IsGameHard
    {
        get { return _mapSize == MapSize.Large; }
        set
        {
            if (_mapSize == MapSize.Large)
                return;

            _mapSize = MapSize.Large;
            OnPropertyChanged(nameof(IsGameEasy));
            OnPropertyChanged(nameof(IsGameMedium));
            OnPropertyChanged(nameof(IsGameHard));
        }
    }
    public Boolean IsGamePaused
    {
        get { return _gameIsPaused; }
        set
        {
            if (_gameIsPaused == value)
                return;

            _gameIsPaused = value;
            OnPropertyChanged(nameof(IsGamePaused));
            OnPauseGame();
        }
    }
    #endregion

    public ViewModel(GameModel model)
    {
        Fields = new();
        _previousPlayerPos = new();

        _model = model;
        _model.newGame += new EventHandler<NewGameEventArgs>(GameModel_NewGame);
        _model.playerMoved += new EventHandler<PlayerMovedEventArgs>(GameModel_PlayerMoved);

        NewGameCommand = new DelegateCommand(param => OnNewGame());
        LoadGameCommand = new DelegateCommand(param => OnLoadGame());
        SaveGameCommand = new DelegateCommand(param => OnSaveGame());
        ExitCommand = new DelegateCommand(param => OnExitGame());
    }

    private void GameModel_NewGame(object? sender, NewGameEventArgs info)
    {
        _previousPlayerPos = _model.PlayerPosition;

        _gameTime = TimeSpan.Zero;
        OnPropertyChanged(nameof(GameTime));

        _mapSize = _model.MapSizeType;
        InitMap();
        Fields[GetListCoordinate(info.PlayerPosition)].IsPlayer = true;

        SetGameDiff();
    }

    private void SetGameDiff()
    {
        switch (_mapSize)
        {
            case MapSize.Small:
                IsGameEasy = true;
                break;
            case MapSize.Medium:
                IsGameMedium = true;
                break;
            case MapSize.Large:
                IsGameHard = true;
                break;
        }
    }

    private void GameModel_PlayerMoved(object? sender, PlayerMovedEventArgs info)
    {
        foreach (Point dark in info.CellsToFree)
        {
            Fields[GetListCoordinate(dark)].IsLit = false;
        }

        foreach (Point light in info.CellsToLight)
        {
            Fields[GetListCoordinate(light)].IsLit = true;
        }

        Fields[GetListCoordinate(_previousPlayerPos)].IsPlayer = false;
        Fields[GetListCoordinate(info.PlayerPosition)].IsPlayer = true;
    }

    public void Timer_Tick(object? sender, EventArgs e)
    {
        _gameTime = _gameTime.Add(TimeSpan.FromSeconds(1));
        OnPropertyChanged(nameof(GameTime));
    }

    public void UserKeyInput(Arrow direction)
    {
        _previousPlayerPos = _model.PlayerPosition;
        _model.MovePlayer(direction);
    }

    private void InitMap()
    {
        Fields.Clear();

        for (int i = 0; i < _model.MapSize; i++)
        {
            for (int j = 0; j < _model.MapSize; j++)
            {
                Fields.Add(new LabField());
            }
        }
    }

    private int GetListCoordinate(Point p)
    {
        return p.Y * _model.MapSize + p.X;
    }

    #region Event caller functions
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

    private void OnPauseGame()
    {
        PauseGame?.Invoke(this, EventArgs.Empty);
    }

    private void OnExitGame()
    {
        ExitGame?.Invoke(this, EventArgs.Empty);
    }
    #endregion

}