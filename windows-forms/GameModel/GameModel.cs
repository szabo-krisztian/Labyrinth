using PlayerNM;
using System.Drawing;
using GameModelArgs;
using AlgorithmNM;
using EnumsNM;
using DataAccessNM;
using GameModel.persistence;

namespace GameModelNM;

public class GameModel
{
    #region Events
    public event EventHandler<NewGameArgs> newGame = null!;
    public event EventHandler<PlayerMovedArgs> playerMoved = null!;
    public event EventHandler<EventArgs> playerWon = null!;
    #endregion

    #region Private fields
    private Map _map = null!;
    private Player _player = null!;
    private Algorithm _algo = null!;
    private readonly HashSet<Point> _cellsToFree = null!;
    private Gamemode _gamemode;
    #endregion

    #region Starting the game methods

    public GameModel()
    {
        _cellsToFree = new HashSet<Point>();
    }

    public void StartNewGame(MapSize mapSize, Gamemode _gamemode)
    {
        // clear lights from previous step
        _cellsToFree.Clear();

        _map = Map.Create(mapSize);
        CreateGameMode(_gamemode);
        
        // set player to starting point
        _player = new Player(_map.MAP_SIZE);

        OnNewGame();
    }

    public void LoadNewGame(string path)
    {
        // Clear lights from the previous step
        _cellsToFree.Clear();

        Gamemode _gamemode = DataAccess.LoadFromFile(path, out Point playerPosition, out MapSize mapSize);

        _map = Map.Create(mapSize);
        CreateGameMode(_gamemode);

        // Set the player to the previous location
        _player = new Player(_map.MAP_SIZE);
        _player.SetPosition(playerPosition);

        OnNewGame();
    }


    private void CreateGameMode(Gamemode _gamemode)
    {
        this._gamemode = _gamemode;
        switch (_gamemode)
        {
            case Gamemode.Normal:
                _algo = new Normal(_map);
                break;
            case Gamemode.Laser:
                _algo = new Laser(_map);
                break;
            case Gamemode.Recursion:
                _algo = new Recursion(_map);
                break;
        }
    }

    private void OnNewGame()
    {
        if (newGame != null)
        {
            newGame(this, new NewGameArgs(new Size(_map.MAP_SIZE * _map.CELL_SIZE, _map.MAP_SIZE * _map.CELL_SIZE), new Point(0, (_map.MAP_SIZE - 1) * _map.CELL_SIZE), new Size(_map.CELL_SIZE, _map.CELL_SIZE), _map.MAP_SIZE, _map.CELL_SIZE, _map.GetMap()));
        }
        OnPlayerMoved();
    }

    #endregion

    #region Player movement methods

    public void PlayerWantsToMove(Arrow arrow)
    {
        switch(arrow)
        {
            case Arrow.Up:
                MovePlayer(new Point(0, -1));           
                break;
            case Arrow.Down:
                MovePlayer(new Point(0, 1));
                break;
            case Arrow.Left:
                MovePlayer(new Point(-1, 0));
                break;
            case Arrow.Right:
                MovePlayer(new Point(1, 0));
                break;
        }
    }

    private void MovePlayer(Point direction)
    {
        if (_algo.PlayerCanMove(_player.position, direction))
        {
            _player.SetPosition(new Point(_player.position.X + direction.X, _player.position.Y + direction.Y));

            OnPlayerMoved();

            bool playerWon = _player.position.X == _map.MAP_SIZE - 1 && _player.position.Y == 0;
            if (playerWon)
            {
                OnPlayerWon();
            }
        }
    }

    private void OnPlayerMoved()
    {
        HashSet<LightPair> cellsToLight = _algo.GetCellsToLight(_player.position);
        

        if (playerMoved != null)
        {
            playerMoved(this, new PlayerMovedArgs(new Point(_player.position.X * _map.CELL_SIZE, _player.position.Y * _map.CELL_SIZE), cellsToLight, _cellsToFree));
        }

        ModifyCellsToFree(cellsToLight);
    }

    private void ModifyCellsToFree(HashSet<LightPair> cellsToLight)
    {
        _cellsToFree.Clear();
        foreach (LightPair pair in cellsToLight)
        {
            _cellsToFree.Add(pair.cellLocation);
        }
    }

    #endregion

    private void OnPlayerWon()
    {
        if (playerWon != null)
        {
            playerWon(this, EventArgs.Empty);
        }
    }

    public void SaveGame(string path)
    {
        DataAccess.SaveFile(path, _map.MAP_SIZE, _gamemode, _player.position);
    }

    #region public getters for Testing


    public Point PlayerLocation
    {
        get
        {
            return _player.position;
        }
    }

    public int MapSize
    {
        get
        {
            return _map.MAP_SIZE;
        }
    }

    public Map MapInstance
    {
        get
        {
            return _map;
        }
    }

    #endregion
}
