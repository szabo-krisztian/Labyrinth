using System.Collections.ObjectModel;
using DelegateCommandNM;
using Game;
using ViewModelBaseNM;
using GameViewModelNM;
using StoredGameEventArgsNM;

namespace StoredGameBrowserViewModelNM
{
    public class StoredGameBrowserViewModel : ViewModelBase
    {
        private readonly StoredGameBrowserModel _model;

        public event EventHandler<StoredGameEventArgs>? GameLoading;

        public event EventHandler<StoredGameEventArgs>? GameSaving;

        public DelegateCommand NewSaveCommand { get; private set; }

        public ObservableCollection<StoredGameViewModel> StoredGames { get; private set; }

        public StoredGameBrowserViewModel(StoredGameBrowserModel model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));


            _model = model;
            _model.StoreChanged += new EventHandler(Model_StoreChanged);

            NewSaveCommand = new DelegateCommand(param =>
            {
                string? fileName = Path.GetFileNameWithoutExtension(param?.ToString()?.Trim());
                if (!String.IsNullOrEmpty(fileName))
                {
                    fileName += ".stl";
                    OnGameSaving(fileName);
                }
            });
            StoredGames = new ObservableCollection<StoredGameViewModel>();
            UpdateStoredGames();
        }

        private void UpdateStoredGames()
        {
            StoredGames.Clear();

            foreach (StoredGameModel item in _model.StoredGames)
            {
                StoredGames.Add(new StoredGameViewModel
                {
                    Name = item.Name,
                    Modified = item.Modified,
                    LoadGameCommand = new DelegateCommand(param => OnGameLoading(param?.ToString() ?? "")),
                    SaveGameCommand = new DelegateCommand(param => OnGameSaving(param?.ToString() ?? ""))
                });
            }
        }

        private void Model_StoreChanged(object? sender, EventArgs e)
        {
            UpdateStoredGames();
        }

        private void OnGameLoading(String name)
        {
            GameLoading?.Invoke(this, new StoredGameEventArgs { Name = name });
        }

        private void OnGameSaving(String name)
        {
            GameSaving?.Invoke(this, new StoredGameEventArgs { Name = name });
        }
    }
}