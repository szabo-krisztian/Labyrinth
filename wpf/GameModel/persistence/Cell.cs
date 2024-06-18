using System.Drawing;

namespace CellNM
{
    public class Cell
    {
        #region Properties
        public Point Position => _position;
        public bool IsWall => _isWall;
        #endregion

        #region private help members
        private readonly bool _isWall;
        private readonly Point _position;
        #endregion

        public Cell(char value, int x, int y)
        {
            _isWall = value == '#';
            _position = new Point(x, y);
        }
    }
}
