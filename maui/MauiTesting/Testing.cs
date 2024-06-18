using System.Drawing;
using Enums;
using Persistence;
using Game;

namespace Testing
{
    [TestClass]
    public class Testing
    {
        private GameModel _model = null!;
        private IDataAccess _dataAccess = null!;
        private int _mapSize;
        private readonly String _saveGamePath = "./testing.txt";

        [TestInitialize]
        public async Task Init()
        {
            _dataAccess = new DataAccess();
            _model = new GameModel(_dataAccess);
            await _model.LoadGameAsync("../../../test.txt");
            _mapSize = _model.GameMap.MAP_SIZE;
        }

        [TestMethod]
        public void GameModel_StartNewGame_MapIsCreated()
        {
            bool nullRef = false;
            bool testVariable;
            Map map = _model.GameMap;

            try
            {
                for (int i = 0; i < map.MAP_SIZE; i++)
                {
                    for (int j = 0; j < map.MAP_SIZE; j++)
                    {
                        testVariable = map[i, j].IsWall;
                    }
                }
            }
            catch (NullReferenceException)
            {
                nullRef = true;
            }

            Assert.AreEqual(nullRef, false);
        }

        [TestMethod]
        public void GameModel_StartNewGame_PlayerIsAtLeftCorner()
        {
            Point playerLocation = _model.PlayerPosition;
            Point leftCorner = new(0, _mapSize - 1);
            Assert.AreEqual(leftCorner, playerLocation);
        }

        [TestMethod]
        public void GameModel_PlayerWantsToMove_PlayerStaysInMap()
        {
            Arrow outOfMap = Arrow.Left;
            Point leftCorner = new(0, _mapSize - 1);
            _model.MovePlayer(outOfMap);
            Point playerLocation = _model.PlayerPosition;
            Assert.AreEqual(leftCorner, playerLocation);
        }

        [TestMethod]
        public void GameModel_PlayerWantsToMove_PlayerMoves()
        {
            Arrow up = Arrow.Up;
            Point playerNewLocation = new(0, _mapSize - 1);
            _model.MovePlayer(up);
            Point playerLocation = _model.PlayerPosition;

            Assert.AreEqual(playerNewLocation, playerLocation);
        }

        [TestMethod]
        public void GameModel_PlayerWantsToMove_PlayerMovesToWall()
        {
            for (int i = 0; i < 3; i++)
            {
                _model.MovePlayer(Arrow.Left);
            }

            Point playerPosition = _model.PlayerPosition;
            bool playerCantMoveThroughWalls = playerPosition.X == 0 && playerPosition.Y == _mapSize - 1;
            Assert.IsTrue(playerCantMoveThroughWalls);
        }

        [TestMethod]
        public async Task DataAccess_SaveFile_FileIsSaved()
        {
            await _model.SaveGameAsync(_saveGamePath);

            StreamReader reader = new(_saveGamePath);
            string mapSize = reader.ReadLine()!;
            string playerPosition = reader.ReadLine()!;

            bool validWriting = true;
            if (mapSize != "15" || playerPosition != "0 14")
            {
                validWriting = false;
            }

            reader.Close();
            Assert.IsTrue(validWriting);
        }

        [TestMethod]
        public async Task DataAccess_LoadFromFile_FileIsLoaded()
        {
            _model.MovePlayer(Arrow.Up);
            Point playerLocationBeforeLoad = _model.PlayerPosition;
            int mapSizeBeforeLoad = _model.MapSize;
            Map mapBeforeLoad = _model.GameMap;

            await _model.SaveGameAsync(_saveGamePath);
            await _model.LoadGameAsync(_saveGamePath);

            Point playerLocationAfterLoad = _model.PlayerPosition;
            int mapSizeAfterLoad = _model.MapSize;
            Map mapAfterLoad = _model.GameMap;

            bool playerLocationLoaded = playerLocationBeforeLoad.X == playerLocationAfterLoad.X && playerLocationBeforeLoad.Y == playerLocationAfterLoad.Y;
            if (!playerLocationLoaded)
            {
                Assert.Fail("Player locations don't match!");
            }

            bool mapSizeLoaded = mapSizeBeforeLoad == mapSizeAfterLoad;
            if (!mapSizeLoaded)
            {
                Assert.Fail("Wrong map size!");
            }

            bool mapIsLoaded = true;

            for (int i = 0; i < _model.MapSize; i++)
            {
                for (int j = 0; j < _model.MapSize; j++)
                {
                    if (mapAfterLoad.GetMap()[i, j].IsWall != mapBeforeLoad.GetMap()[i, j].IsWall)
                    {
                        mapIsLoaded = true;
                    }
                }
            }

            Assert.IsTrue(mapIsLoaded);
        }
    }
}