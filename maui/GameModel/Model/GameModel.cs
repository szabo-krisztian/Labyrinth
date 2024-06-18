using System.Drawing;
using Args;
using Enums;
using Persistence;

namespace Game;

public class GameModel
{
    #region Events
    public event EventHandler<NewGameEventArgs> NewGame = null!;
    public event EventHandler<PlayerMovedEventArgs> PlayerMoved = null!;
    public event EventHandler<EventArgs> PlayerWon = null!;
    #endregion

    #region Event caller functions
    private void OnNewGame()
    {
        NewGame?.Invoke(this, new NewGameEventArgs(
            _map.MAP_SIZE,
            _map.GetMap(),
            _map.Player.Position)
        );

        OnPlayerMoved();
    }

    private void OnPlayerMoved()
    {
        HashSet<Point> cellsToLight = _algo.GetCellsToLight(_map.Player.Position);

        PlayerMoved?.Invoke(this, new PlayerMovedEventArgs(
            new Point(_map.Player.Position.X, _map.Player.Position.Y),
            cellsToLight,
            _cellsToFree)
        );

        ModifyCellsToFree(cellsToLight);
    }

    private void OnPlayerWon()
    {
        PlayerWon?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Private fields
    private Map _map = null!;
    private Algorithm _algo = null!;
    private readonly HashSet<Point> _cellsToFree = null!;
    private readonly IDataAccess _dataAccess;
    #endregion

    #region Starting the game methods
    public GameModel(IDataAccess dataAccess)
    {
        _cellsToFree = new HashSet<Point>();
        _dataAccess = dataAccess;
    }


    public async Task LoadGameAsync(string path)
    {
        _cellsToFree.Clear();
        _map = await _dataAccess.LoadAsync(path);
        _algo = new Algorithm(_map);
        OnNewGame();
    }

    public async Task LoadGameAsync(Stream path)
    {
        _cellsToFree.Clear();
        _map = await _dataAccess.LoadAsync(path);
        _algo = new Algorithm(_map);
        OnNewGame();
    }

    public async Task SaveGameAsync(string path)
    {
        await _dataAccess.SaveAsync(path, _map);
    }
    #endregion

    #region Player movement methods
    public void MovePlayer(Arrow arrow)
    {
        switch (arrow)
        {
            case Arrow.Up:
                TryMovement(new Point(0, -1));
                break;
            case Arrow.Down:
                TryMovement(new Point(0, 1));
                break;
            case Arrow.Left:
                TryMovement(new Point(-1, 0));
                break;
            case Arrow.Right:
                TryMovement(new Point(1, 0));
                break;
        }
    }

    private void TryMovement(Point direction)
    {
        bool playerCanMove = _algo.PlayerCanMove(_map.Player.Position, direction);
        if (playerCanMove)
        {
            

            _map.Player.MoveOnX(direction.X);
            _map.Player.MoveOnY(direction.Y);

            OnPlayerMoved();

            bool PlayerWon = _map.Player.Position.X == _map.MAP_SIZE - 1 && _map.Player.Position.Y == 0;
            if (PlayerWon)
            {
                OnPlayerWon();
            }
        }
    }

    private void ModifyCellsToFree(HashSet<Point> cellsToLight)
    {
        _cellsToFree.Clear();
        foreach (Point pair in cellsToLight)
        {
            _cellsToFree.Add(pair);
        }
    }
    #endregion

    #region public getters for Testing
    public Point PlayerPosition => _map.Player.Position;
    public int MapSize => _map.MAP_SIZE;
    public Map GameMap => _map;
    #endregion
}
