using Game;
using Persistence;
using GameViewModelNM;
using StoredGameBrowserViewModelNM;
using StoredGameEventArgsNM;

namespace MauiModel;

public partial class AppShell : Shell
{
    #region Private fields
    private readonly GameModel _model;
    private readonly GameViewModel _viewModel;

    private readonly IStore _store;
    private readonly StoredGameBrowserModel _storedGameBrowserModel;
    private readonly StoredGameBrowserViewModel _storedGameBrowserViewModel;
    private readonly IDispatcherTimer _timer;
    #endregion

    public AppShell(
        IStore store,
        GameModel model,
        GameViewModel viewModel)
    {
        InitializeComponent();

        _store = store;
        _model = model;
        _viewModel = viewModel;

        _viewModel.ExitGame += ViewModel_ExitGame;
        _viewModel.LoadGame += ViewModel_LoadGame;
        _viewModel.SaveGame += ViewModel_SaveGame;
        _viewModel.NewGame += ViewModel_NewGame;
        _viewModel.TimePaused += ViewModel_TimePaused;
        _viewModel.PlayerWon += ViewModel_PlayerWon;
        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += _viewModel.Timer_Tick;
        _timer.Start();

        _storedGameBrowserModel = new StoredGameBrowserModel(_store);
        _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
        _storedGameBrowserViewModel.GameLoading += StoredGameBrowserViewModel_GameLoading;
        _storedGameBrowserViewModel.GameSaving += StoredGameBrowserViewModel_GameSaving;
    }

    private async void ViewModel_PlayerWon(object? sender, EventArgs info)
    {
        await DisplayAlert("Labirintus játék", "Nyertél!", "OK");
    }

    private void ViewModel_TimePaused(object? sender, EventArgs info)
    {
        if (!_viewModel.IsGamePaused)
        {
            _timer.Start();
        }
        else
        {
            _timer.Stop();
        }
    }

    private async void ViewModel_NewGame(object? sender, EventArgs info)
    {
        if (_viewModel.Difficulty.Difficulty == Enums.MapSize.Small)
        {
            await LoadMethod("small.txt");
        }
        else if (_viewModel.Difficulty.Difficulty == Enums.MapSize.Medium)
        {
            await LoadMethod("medium.txt");
        }
        else if (_viewModel.Difficulty.Difficulty == Enums.MapSize.Large)
        {
            await LoadMethod("large.txt");
        }
    }
    private async Task LoadMethod(String fileName)
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
        await _model.LoadGameAsync(stream);
    }

    private async void ViewModel_ExitGame(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage
        {
            BindingContext = _viewModel
        });
    }

    private async void ViewModel_LoadGame(object? sender, EventArgs e)
    {
        await _storedGameBrowserModel.UpdateAsync();
        await Navigation.PushAsync(new LoadGamePage
        {
            BindingContext = _storedGameBrowserViewModel
        });
    }

    private async void ViewModel_SaveGame(object? sender, EventArgs e)
    {
        await _storedGameBrowserModel.UpdateAsync();
        await Navigation.PushAsync(new SaveGamePage
        {
            BindingContext = _storedGameBrowserViewModel
        });
    }

    private async void StoredGameBrowserViewModel_GameLoading(object? sender, StoredGameEventArgs e)
    {
        await Navigation.PopAsync();

        try
        {
            await _model.LoadGameAsync(e.Name);
            await Navigation.PopAsync();
            await DisplayAlert("Labirintus játék", "Sikeres betöltés.", "OK");            
        }
        catch
        {
            await DisplayAlert("Labirintus játék", "Sikertelen betöltés.", "OK");
        }
    }

    private async void StoredGameBrowserViewModel_GameSaving(object? sender, StoredGameEventArgs e)
    {
        await Navigation.PopAsync();

        try
        {
            await _model.SaveGameAsync(e.Name);
            await DisplayAlert("Labirintus játék", "Sikeres mentés.", "OK");
        }
        catch
        {
            await DisplayAlert("Labirintus játék", "Sikertelen mentés.", "OK");
        }
    }
}

