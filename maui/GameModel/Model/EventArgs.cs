using System.Drawing;
using Persistence;

namespace Args
{
    public class NewGameEventArgs : EventArgs
    {
        #region Properties
        public int MapSize => _mapSize;
        public Cell[,] Map => _map;
        public Point PlayerPosition => _playerPosition;
        #endregion

        #region private help members
        private readonly int _mapSize;
        private readonly Cell[,] _map;
        private readonly Point _playerPosition;
        #endregion

        public NewGameEventArgs(int mapSize, Cell[,] map, Point playerPosition)
        {
            _mapSize = mapSize;
            _map = map;
            _playerPosition = playerPosition;
        }
    }

    public class PlayerMovedEventArgs : EventArgs
    {
        #region Properties
        public Point PlayerPosition => _playerPosition;
        public HashSet<Point> CellsToLight => _cellsToLight;
        public HashSet<Point> CellsToFree => _cellsToFree;
        #endregion

        #region private help members
        private readonly HashSet<Point> _cellsToLight;
        private readonly Point _playerPosition;
        private readonly HashSet<Point> _cellsToFree;
        #endregion

        public PlayerMovedEventArgs(Point playerPosition, HashSet<Point> cellsToLight, HashSet<Point> cellsToFree)
        {
            _playerPosition = playerPosition;
            _cellsToLight = cellsToLight;
            _cellsToFree = cellsToFree;
        }
    }
}
