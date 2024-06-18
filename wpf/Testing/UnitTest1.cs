using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnumsNM;
using System.Drawing;
using System;
using System.IO;
using MapNM;

namespace Testing
{
    [TestClass]
    public class UnitTest1
    {
        private GameModelNM.GameModel game = null!;
        private MapSize mapSize;
        private readonly string saveFilePath = "../../../paths/save.txt";

        [TestInitialize]
        public void Init()
        {
            game = new GameModelNM.GameModel();
            mapSize = MapSize.Small;
            game.StartNewGame(mapSize);
        }

        [TestMethod]
        public void GameModel_StartNewGame_MapIsCreated()
        {
            bool nullRef = false;
            bool testVariable;
            Map map = game.GameMap;

            try
            {
                for (int i = 0; i < map.MAP_SIZE; i++)
                {
                    for (int j = 0; j < map.MAP_SIZE; j++)
                    {
                        testVariable = map.GetMap()[i, j].IsWall;
                    }
                }
            }
            catch(NullReferenceException)
            {
                nullRef = true;
            }

            Assert.AreEqual(nullRef, false);
        }

        [TestMethod]
        public void GameModel_StartNewGame_PlayerIsAtLeftCorner()
        {
            Point playerLocation = game.PlayerPosition;
            Point leftCorner = new(0, game.MapSize - 1);
            Assert.AreEqual(leftCorner, playerLocation);
        }

        [TestMethod]
        public void GameModel_PlayerWantsToMove_PlayerStaysInMap()
        {
            Arrow outOfMap = Arrow.Left;
            Point leftCorner = new(0, game.MapSize - 1);  
            game.MovePlayer(outOfMap);
            Point playerLocation = game.PlayerPosition;
            Assert.AreEqual(leftCorner, playerLocation);
        }

        [TestMethod]
        public void GameModel_PlayerWantsToMove_PlayerMoves()
        {
            Arrow up = Arrow.Up;
            Point playerNewLocation = new(0, game.MapSize - 2);
            game.MovePlayer(up);
            Point playerLocation = game.PlayerPosition;
            Assert.AreEqual(playerNewLocation, playerLocation);
        }

        [TestMethod] 
        public void GameModel_PlayerWantsToMove_PlayerMovesToWall()
        {
            for (int i = 0; i < 3; i++)
            {
                game.MovePlayer(Arrow.Up);
            }

            Point playerPosition = game.PlayerPosition;
            bool playerCantMoveThroughWalls = playerPosition.X == 0 && playerPosition.Y == game.MapSize - 2;
            Assert.IsTrue(playerCantMoveThroughWalls);
        }

        private bool gameIsWon = false;

        [TestMethod]
        public void GameModel_PlayerPassesThroughMap_GameIsWon()
        {
            game.playerWon += GameIsWonEventHandler!;

            Arrow up = Arrow.Up;
            Arrow right = Arrow.Right;

            for (int i = 0; i < 4; i++)
            {
                game.MovePlayer(right);
            }

            game.MovePlayer(up);

            for (int i = 0; i < 5; i++)
            {
                game.MovePlayer(right);
            }

            for (int i = 0; i < 9; i++)
            {
                game.MovePlayer(up);
            }

            game.MovePlayer(right);

            Assert.IsTrue(gameIsWon);
        }

        private void GameIsWonEventHandler(object sender, EventArgs info)
        {
            gameIsWon = true;   
        }

        [TestMethod]
        public void DataAccess_SaveFile_FileIsSaved()
        {
            game.SaveGame(saveFilePath);

            StreamReader reader = new(saveFilePath);
            string mapSize = reader.ReadLine()!;
            string playerPosition = reader.ReadLine()!;

            bool validWriting = true;
            if (mapSize != "11" || playerPosition != "0 10")
            {
                validWriting = false;
            }
            
            reader.Close();
            Assert.IsTrue(validWriting);
        }

        [TestMethod]
        public void DataAccess_LoadFromFile_FileIsLoaded()
        {
            game.MovePlayer(Arrow.Up);
            Point playerLocationBeforeLoad = game.PlayerPosition;
            int mapSizeBeforeLoad = game.MapSize;
            Map mapBeforeLoad = game.GameMap;

            game.SaveGame(saveFilePath);
            game.LoadNewGame(saveFilePath);
            
            Point playerLocationAfterLoad = game.PlayerPosition;
            int mapSizeAfterLoad = game.MapSize;
            Map mapAfterLoad = game.GameMap;

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
            
            for (int i = 0; i < game.MapSize; i++)
            {
                for (int j = 0; j < game.MapSize; j++)
                {
                    if (mapAfterLoad.GetMap()[i,j].IsWall != mapBeforeLoad.GetMap()[i,j].IsWall)
                    {
                        mapIsLoaded = true;
                    }
                }
            }
            
            Assert.IsTrue(mapIsLoaded);
        }
    }
}