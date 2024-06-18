using Game;
using Persistence;
using GameViewModelNM;
using Enums;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls.Shapes;

namespace MauiModel;

public partial class App : Application
{
    #region Private fields
    private readonly GameModel _model;
    private readonly IDataAccess _dataAccess;
    private readonly AppShell _appShell;
    private readonly IStore _store;
    private readonly GameViewModel _viewModel;
    #endregion

    public App()
    {
        InitializeComponent();

        _store = new Store();
        _dataAccess = new DataAccess(FileSystem.AppDataDirectory);
        _model = new GameModel(_dataAccess);
        _viewModel = new GameViewModel(_model);

        Task.Run(async () =>
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("medium.txt");
            await _model.LoadGameAsync(stream);
        }).Wait();

        _appShell = new AppShell(_store, _model, _viewModel)
        {
            BindingContext = _viewModel
        };
        MainPage = _appShell;
    }
}
