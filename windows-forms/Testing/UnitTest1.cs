using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnumsNM;
using GameModel.persistence;
using DataAccessNM;
using System.Drawing;
using System;
using System.IO;

namespace Testing
{
    [TestClass]
    public class UnitTest1
    {
        private GameModelNM.GameModel game = null!;
        private Gamemode gamemode;
        private MapSize mapSize;
        private readonly string saveFilePath = "..\\..\\..\\paths\\save.txt";

        [TestInitialize]
        public void Init()
        {
            game = new GameModelNM.GameModel();
            mapSize = MapSize.Small;
            gamemode = Gamemode.Normal;
            game.StartNewGame(mapSize, gamemode);
        }

        [TestMethod]
        public void GameModel_StartNewGame_MapIsCreated()
        {
            bool nullRef = false;
            bool testVariable;
            Map map = game.MapInstance;

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
            Point playerLocation = game.PlayerLocation;
            Point leftCorner = new Point(0, game.MapSize - 1);
            Assert.AreEqual(leftCorner, playerLocation);
        }

        [TestMethod]
        public void GameModel_PlayerWantsToMove_PlayerStaysInMap()
        {
            Arrow outOfMap = Arrow.Left;
            Point leftCorner = new Point(0, game.MapSize - 1);  
            game.PlayerWantsToMove(outOfMap);
            Point playerLocation = game.PlayerLocation;
            Assert.AreEqual(leftCorner, playerLocation);
        }

        [TestMethod]
        public void GameModel_PlayerWantsToMove_PlayerMoves()
        {
            Arrow up = Arrow.Up;
            Point playerNewLocation = new Point(0, game.MapSize - 2);
            game.PlayerWantsToMove(up);
            Point playerLocation = game.PlayerLocation;
            Assert.AreEqual(playerNewLocation, playerLocation);
        }

        [TestMethod] 
        public void GameModel_PlayerWantsToMove_PlayerMovesToWall()
        {
            for (int i = 0; i < 3; i++)
            {
                game.PlayerWantsToMove(Arrow.Up);
            }

            Point playerPosition = game.PlayerLocation;
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
                game.PlayerWantsToMove(right);
            }

            game.PlayerWantsToMove(up);

            for (int i = 0; i < 5; i++)
            {
                game.PlayerWantsToMove(right);
            }

            for (int i = 0; i < 9; i++)
            {
                game.PlayerWantsToMove(up);
            }

            game.PlayerWantsToMove(right);

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

            StreamReader reader = new StreamReader(saveFilePath);
            string gamemode = reader.ReadLine()!;
            string mapSize = reader.ReadLine()!;
            string playerPosition = reader.ReadLine()!;

            bool validWriting = true;
            if (gamemode != "Normal" || mapSize != "11" || playerPosition != "0 10")
            {
                validWriting = false;
            }
            
            reader.Close();
            Assert.IsTrue(validWriting);
        }

        [TestMethod]
        public void DataAccess_LoadFromFile_FileIsLoaded()
        {
            game.PlayerWantsToMove(Arrow.Up);
            Point playerLocationBeforeLoad = game.PlayerLocation;
            int mapSizeBeforeLoad = game.MapSize;
            Map mapBeforeLoad = game.MapInstance;

            game.SaveGame(saveFilePath);
            game.LoadNewGame(saveFilePath);
            
            Point playerLocationAfterLoad = game.PlayerLocation;
            int mapSizeAfterLoad = game.MapSize;
            Map mapAfterLoad = game.MapInstance;

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