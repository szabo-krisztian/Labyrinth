using EnumsNM;
using GameModelNM;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ViewModelNM;

namespace LabGame;

public partial class App : Application
{
    #region Private fields
    private GameModel _model = null!;
    private MainWindow _window = null!;
    private ViewModel _viewModel = null!;
    private DispatcherTimer _timer = null!;
    #endregion

    public App()
    {
        Startup += new StartupEventHandler(App_Startup);
    }

    private void App_Startup(object? sender, StartupEventArgs info)
    {
        _model = new();
        
        _viewModel = new(_model);
        _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
        _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);
        _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
        _viewModel.PauseGame += new EventHandler(ViewModel_PauseGame);
        _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
        _model.playerWon += new EventHandler<EventArgs>(GameModel_PlayerWon);

        _window = new MainWindow
        {
            DataContext = _viewModel
        };
        _window.Show();
        _window.KeyDown += new KeyEventHandler(Window_KeyDown);

        MapSize mapSize = MapSize.Small;
        _model.StartNewGame(mapSize);

        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _timer.Tick += _viewModel.Timer_Tick;
        _timer.Start();
    }

    private void Window_KeyDown(object? sender, KeyEventArgs info)
    {
        bool validInput = info.Key == Key.A || info.Key == Key.S || info.Key == Key.D || info.Key == Key.W;
        if (validInput)
        {
            Arrow direction = GetInputData(info);
            _viewModel.UserKeyInput(direction);
        }
    }

    private static Arrow GetInputData(KeyEventArgs info)
    {
        return info.Key switch
        {
            Key.A => Arrow.Left,
            Key.S => Arrow.Down,
            Key.D => Arrow.Right,
            Key.W => Arrow.Up,
            _ => throw new Exception("Invalid key")
        };
    }

    private void GameModel_PlayerWon(object? sender, EventArgs info)
    {
        MessageBox.Show("Győztél!");
        _window.KeyDown -= Window_KeyDown;
        _timer.Stop();
    }

    #region ViewModel EventHandlers
    private void ViewModel_NewGame(object? sender, EventArgs e)
    {
        MapSize mapSize = GetMapSize();
        _model.StartNewGame(mapSize);

        _window.KeyDown -= Window_KeyDown;
        _window.KeyDown += Window_KeyDown;
        _timer.Start();
    }

    private MapSize GetMapSize()
    {
        if (_viewModel.IsGameEasy)
        {
            return MapSize.Small;
        }
        if (_viewModel.IsGameMedium)
        {
            return MapSize.Medium;
        }
        if (_viewModel.IsGameHard)
        {
            return MapSize.Large;
        }
        throw new Exception("Invalid difficulty");
    }

    private void ViewModel_LoadGame(object? sender, EventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "Labirintus betöltése",
            Filter = "Szövegfájlok (*.txt)|*.txt|összes fájl (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            _model.LoadNewGame(openFileDialog.FileName);
            
            _window.KeyDown -= Window_KeyDown;
            _window.KeyDown += Window_KeyDown;
            _timer.Start();
        }
    }

    private void ViewModel_SaveGame(object? sender, EventArgs e)
    {
        try
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Labirintus betöltése",
                Filter = "Szövegfájlok (*.txt)|*.txt|összes fájl (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            
            if (saveFileDialog.ShowDialog() == true)
            {
                _model.SaveGame(saveFileDialog.FileName);
            }
        }
        catch
        {
            MessageBox.Show("A fájl mentése sikertelen!", "Sudoku", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ViewModel_PauseGame(object? sender, EventArgs e)
    {
        if (_viewModel.IsGamePaused)
        {
            _timer.Stop();
            _window.KeyDown -= Window_KeyDown;
        }
        else
        {
            _timer.Start();
            _window.KeyDown += Window_KeyDown;
        }
    }

    private void ViewModel_ExitGame(object? sender, EventArgs e)
    {
        _window.Close();
    }
    #endregion
}

