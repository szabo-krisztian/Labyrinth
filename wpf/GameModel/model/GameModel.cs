using System.Drawing;
using MapNM;
using AlgorithmNM;
using PlayerNM;
using EventArgsNM;
using EnumsNM;
using DataAccessNM;

namespace GameModelNM;

public class GameModel
{
    #region Events
    public event EventHandler<NewGameEventArgs> newGame = null!;
    public event EventHandler<PlayerMovedEventArgs> playerMoved = null!;
    public event EventHandler<EventArgs> playerWon = null!;
    #endregion

    #region Event caller functions
    private void OnNewGame()
    {
        newGame?.Invoke(this, new NewGameEventArgs(
            _map.MAP_SIZE,
            _map.GetMap(),
            _player.Position)
        );

        OnPlayerMoved();
    }

    private void OnPlayerMoved()
    {
        HashSet<Point> cellsToLight = _algo.GetCellsToLight(_player.Position);

        playerMoved?.Invoke(this, new PlayerMovedEventArgs(
            new Point(_player.Position.X, _player.Position.Y),
            cellsToLight,
            _cellsToFree)
        );

        ModifyCellsToFree(cellsToLight);
    }

    private void OnPlayerWon()
    {
        playerWon?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Private fields
    private Map _map = null!;
    private Player _player = null!;
    private Algorithm _algo = null!;
    private readonly HashSet<Point> _cellsToFree = null!;
    private MapSize _mapSize;
    #endregion

    #region Starting the game methods
    public GameModel()
    {
        _cellsToFree = new HashSet<Point>();
    }

    public void StartNewGame(MapSize mapSize)
    {
        _cellsToFree.Clear();

        _map = Map.Create(mapSize);
        _mapSize = mapSize;
        _algo = new Algorithm(_map);
        _player = new Player(new Point(0, _map.MAP_SIZE - 1));

        OnNewGame();
    }

    public void LoadNewGame(string path)
    {
        _cellsToFree.Clear();

        DataAccess.LoadFromFile(path, out Point playerPosition, out MapSize mapSize);
        
        _map = Map.Create(mapSize);
        _mapSize = mapSize;
        _algo = new Algorithm(_map);
        _player = new Player(playerPosition);

        OnNewGame();
    }

    public void SaveGame(string path)
    {
        DataAccess.SaveFile(path, _map, _player.Position);
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
        bool playerCanMove = _algo.PlayerCanMove(_player.Position, direction);
        if (playerCanMove)
        {
            _player.MoveOnX(direction.X);
            _player.MoveOnY(direction.Y);

            OnPlayerMoved();

            bool playerWon = _player.Position.X == _map.MAP_SIZE - 1 && _player.Position.Y == 0;
            if (playerWon)
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
    public Point PlayerPosition => _player.Position;
    public int MapSize => _map.MAP_SIZE;
    public Map GameMap => _map;

    public MapSize MapSizeType => _mapSize;
    #endregion
}
