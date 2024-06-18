using EnumsNM;
using GameModelArgs;

namespace GameView;

public partial class Form1 : Form
{
    #region Private fields
    private GameModelNM.GameModel game = null!;
    private Panel background = null!;
    private Panel player = null!;
    private Panel[,] cells = null!;

    private System.Windows.Forms.Timer timer = null!;
    private TimeSpan elapsedTime;
    private Label elapsedTimeLabel = null!;

    private Button saveGameButton = null!;
    private Button backToMenuButton = null!;
    private CheckBox pauseTimeButton = null!;
    #endregion

    #region Initialization

    public Form1()
    {
        InitializeComponent();
    }

    private void FormLoadEventHandler(object sender, EventArgs info)
    {
        background = new Panel();
        background.BackColor = Color.Black;

        player = new Panel();
        player.BackColor = Color.Pink;

        game = new GameModelNM.GameModel();
        game.newGame += Game_GameStarted__SettingUpPanels!;
        game.playerMoved += Game_PlayerMoved__ActivateLights!;
        game.playerWon += Game_PlayerWon__ShowDialogWindow!;

        KeyDown += KeyHitEventHandler!;

        //timer
        elapsedTimeLabel = new Label();
        elapsedTimeLabel.Font = new Font("Unispace", 18.0f, FontStyle.Bold);
        elapsedTimeLabel.Size = new Size(250, 75);
        timer = new System.Windows.Forms.Timer();
        timer.Interval = 1000;
        timer.Tick += TimerTickEventHandler!;

        //save game button
        saveGameButton = new Button();
        saveGameButton.Font = new Font("Unispace", 15.0f, FontStyle.Bold);
        saveGameButton.Size = new Size(100, 100);
        saveGameButton.Text = "Save game";
        saveGameButton.Click += SaveGameEventHandler!;

        //back to menu buton
        backToMenuButton = new Button();
        backToMenuButton.Font = new Font("Unispace", 15.0f, FontStyle.Bold);
        backToMenuButton.Size = new Size(100, 100);
        backToMenuButton.Text = "Menu";
        backToMenuButton.Click += BackToMenuEventHandler!;

        //pause time button
        pauseTimeButton = new CheckBox();
        pauseTimeButton.Font = new Font("Unispace", 15.0f, FontStyle.Bold);
        pauseTimeButton.Size = new Size(225, 100);
        pauseTimeButton.Text = "Pause";
        pauseTimeButton.Appearance = Appearance.Button;
        pauseTimeButton.Click += PauseTimeEventHandler!;
    }

    #endregion

    #region Menu phase private methods

    private void StartHitEventHandler(object sender, EventArgs info)
    {
        bool mapSizeSelected = smallMapButton.Checked || mediumMapButton.Checked || largeMapButton.Checked;
        bool gamemodeSelected = normalGamemodeButton.Checked || laserGamemodeButton.Checked || recursiveGamemodeButton.Checked;

        if (mapSizeSelected && gamemodeSelected)
        {
            MapSize mapSize = GetMapSize();
            Gamemode gamemode = GetGamemode();
            game.StartNewGame(mapSize, gamemode);
        }
    }

    private MapSize GetMapSize()
    {
        if (smallMapButton.Checked)
        {
            smallMapButton.Checked = false;
            return MapSize.Small;
        }
        if (mediumMapButton.Checked)
        {
            mediumMapButton.Checked = false;
            return MapSize.Medium;
        }
        if (largeMapButton.Checked)
        {
            largeMapButton.Checked = false;
            return MapSize.Large;
        }
        throw new Exception();
    }

    private Gamemode GetGamemode()
    {
        if (normalGamemodeButton.Checked)
        {
            normalGamemodeButton.Checked = false;
            return Gamemode.Normal;
        }
        if (laserGamemodeButton.Checked)
        {
            laserGamemodeButton.Checked = false;
            return Gamemode.Laser;
        }
        if (recursiveGamemodeButton.Checked)
        {
            recursiveGamemodeButton.Checked = false;
            return Gamemode.Recursion;
        }
        throw new Exception();
    }

    private void QuitHitEventHandler(object sender, EventArgs info)
    {
        Dispose();
    }

    private void LoadGameEventHandler(object sender, EventArgs info)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Szövegfájlok (*.txt)|*.txt|Összes fájl (*.*)|*.*";

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            game.LoadNewGame(openFileDialog.FileName);
        }
    }

    #endregion

    #region Game phase private methods
    private void Game_GameStarted__SettingUpPanels(object sender, NewGameArgs info)
    {
        // delete menu panels
        Controls.Remove(menuPanel);

        // WINDOW
        Size = new Size(info.sizeOfMap.Width + 300, info.sizeOfMap.Height + 50);

        // Timer
        elapsedTime = TimeSpan.Zero;
        elapsedTimeLabel.Location = new Point(info.sizeOfMap.Width + 25, 50);
        timer.Start();
        Controls.Add(elapsedTimeLabel);

        // Save game button
        saveGameButton.Location = new Point(info.sizeOfMap.Width + 25, 150);
        Controls.Add(saveGameButton);

        // Back to menu button
        backToMenuButton.Location = new Point(info.sizeOfMap.Width + 25 + 125, 150);
        Controls.Add(backToMenuButton);

        // Pause time button
        pauseTimeButton.Location = new Point(info.sizeOfMap.Width + 25, 250);
        Controls.Add(pauseTimeButton);

        // Background
        background.Size = info.sizeOfMap;
        Controls.Add(background);

        // Coloring all the floors black
        cells = new Panel[info.MAP_SIZE, info.MAP_SIZE];

        for (int i = 0; i < info.MAP_SIZE; i++)
        {
            for (int j = 0; j < info.MAP_SIZE; j++)
            {
                bool cellIsFloor = !info.map[i, j].IsWall;
                if (cellIsFloor)
                {
                    ColoringTheFloorBlack(info, i, j);
                }
            }
        }

        // Player
        player.Size = info.sizeOfcell;
        player.Location = info.playerPosition;
        Controls.Add(player);
        player.BringToFront();

        BringToFront();
        KeyPreview = true;
        Focus();
    }

    private void KeyHitEventHandler(object sender, KeyEventArgs info)
    {
        switch (info.KeyCode)
        {
            case Keys.W:
                game.PlayerWantsToMove(Arrow.Up);
                break;
            case Keys.S:
                game.PlayerWantsToMove(Arrow.Down);
                break;
            case Keys.A:
                game.PlayerWantsToMove(Arrow.Left);
                break;
            case Keys.D:
                game.PlayerWantsToMove(Arrow.Right);
                break;
            default:
                break;
        }
    }

    private void ColoringTheFloorBlack(NewGameArgs info, int i, int j)
    {
        Panel panel = new Panel();
        panel.BackColor = Color.Black;
        panel.Size = info.sizeOfcell;
        panel.Location = new Point(info.map[i, j].Position.X * info.CELL_SIZE, info.map[i, j].Position.Y * info.CELL_SIZE);
        cells[i, j] = panel;
        Controls.Add(panel);
        panel.BringToFront();
    }

    private void TimerTickEventHandler(object sender, EventArgs info)
    {
        elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
        elapsedTimeLabel.Text = "Time elapsed:\n" + elapsedTime.ToString(@"mm\:ss");
    }

    private void Game_PlayerMoved__ActivateLights(object sender, PlayerMovedArgs info)
    {
        player.Location = info.newPosition;

        foreach (Point garbage in info.cellsToFree)
        {
            cells[garbage.Y, garbage.X].BackColor = Color.Black;
        }

        foreach (LightPair light in info.cellsToLight)
        {
            cells[light.cellLocation.Y, light.cellLocation.X].BackColor = light.RGB;
        }

        player.BringToFront();
    }

    private void Game_PlayerWon__ShowDialogWindow(object sender, EventArgs info)
    {
        DialogResult result = MessageBox.Show("You won!\nWant to player another game?", "Text", MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes)
        {
            BackToMenu();
        }
        else
        {
            Dispose();
        }
    }

    private void BackToMenu()
    {
        Controls.Remove(elapsedTimeLabel);
        Controls.Remove(saveGameButton);
        Controls.Remove(backToMenuButton);
        Controls.Remove(pauseTimeButton);
        Controls.Remove(player);

        foreach (Panel cell in cells)
        {
            this.Controls.Remove(cell);
        }

        Controls.Remove(background);

        KeyPreview = false;
        
        timer.Stop();
        
        Controls.Add(menuPanel);
    }

    private void SaveGameEventHandler(object sender, EventArgs info)
    {
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        saveFileDialog1.Filter = "Szövegfájlok (*.txt)|*.txt|Összes fájl (*.*)|*.*";
        saveFileDialog1.FilterIndex = 1;
        saveFileDialog1.RestoreDirectory = true;

        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        {
            game.SaveGame(saveFileDialog1.FileName);
        }
    }

    private void BackToMenuEventHandler(object sender, EventArgs info)
    {
        BackToMenu();
    }

    private void PauseTimeEventHandler(object sender, EventArgs info)
    {
        if (pauseTimeButton.Checked)
        {
            KeyPreview = false;
            timer.Stop();
        }
        else
        {
            KeyPreview = true;
            timer.Start();
        }
    }

    #endregion
}
