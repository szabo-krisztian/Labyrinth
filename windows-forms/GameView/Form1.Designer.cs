namespace GameView
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            menuPanel = new Panel();
            loadGameButton = new Button();
            quitGameButton = new Button();
            startGameButton = new Button();
            groupBox2 = new GroupBox();
            largeMapButton = new RadioButton();
            smallMapButton = new RadioButton();
            mediumMapButton = new RadioButton();
            label2 = new Label();
            chooseGamemodeLabel = new Label();
            groupBox1 = new GroupBox();
            recursiveGamemodeButton = new RadioButton();
            normalGamemodeButton = new RadioButton();
            laserGamemodeButton = new RadioButton();
            menuPanel.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // menuPanel
            // 
            menuPanel.Controls.Add(loadGameButton);
            menuPanel.Controls.Add(quitGameButton);
            menuPanel.Controls.Add(startGameButton);
            menuPanel.Controls.Add(groupBox2);
            menuPanel.Controls.Add(label2);
            menuPanel.Controls.Add(chooseGamemodeLabel);
            menuPanel.Controls.Add(groupBox1);
            menuPanel.Dock = DockStyle.Fill;
            menuPanel.Location = new Point(0, 0);
            menuPanel.Name = "menuPanel";
            menuPanel.Size = new Size(479, 664);
            menuPanel.TabIndex = 0;
            // 
            // loadGameButton
            // 
            loadGameButton.Anchor = AnchorStyles.None;
            loadGameButton.Font = new Font("Unispace", 15.7499981F, FontStyle.Bold, GraphicsUnit.Point);
            loadGameButton.Location = new Point(186, 12);
            loadGameButton.Name = "loadGameButton";
            loadGameButton.Size = new Size(100, 100);
            loadGameButton.TabIndex = 7;
            loadGameButton.Text = "Load game";
            loadGameButton.UseVisualStyleBackColor = true;
            loadGameButton.Click += LoadGameEventHandler;
            // 
            // quitGameButton
            // 
            quitGameButton.Anchor = AnchorStyles.None;
            quitGameButton.Font = new Font("Unispace", 15.7499981F, FontStyle.Bold, GraphicsUnit.Point);
            quitGameButton.Location = new Point(367, 12);
            quitGameButton.Name = "quitGameButton";
            quitGameButton.Size = new Size(100, 100);
            quitGameButton.TabIndex = 6;
            quitGameButton.Text = "Quit game";
            quitGameButton.UseVisualStyleBackColor = true;
            quitGameButton.Click += QuitHitEventHandler;
            // 
            // startGameButton
            // 
            startGameButton.Anchor = AnchorStyles.None;
            startGameButton.Font = new Font("Unispace", 15.7499981F, FontStyle.Bold, GraphicsUnit.Point);
            startGameButton.Location = new Point(12, 12);
            startGameButton.Name = "startGameButton";
            startGameButton.Size = new Size(100, 100);
            startGameButton.TabIndex = 5;
            startGameButton.Text = "Start game";
            startGameButton.UseVisualStyleBackColor = true;
            startGameButton.Click += StartHitEventHandler;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.None;
            groupBox2.Controls.Add(largeMapButton);
            groupBox2.Controls.Add(smallMapButton);
            groupBox2.Controls.Add(mediumMapButton);
            groupBox2.Location = new Point(59, 441);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(362, 179);
            groupBox2.TabIndex = 4;
            groupBox2.TabStop = false;
            // 
            // largeMapButton
            // 
            largeMapButton.Appearance = Appearance.Button;
            largeMapButton.BackgroundImage = (Image)resources.GetObject("largeMapButton.BackgroundImage");
            largeMapButton.Location = new Point(243, 37);
            largeMapButton.MaximumSize = new Size(110, 110);
            largeMapButton.MinimumSize = new Size(110, 110);
            largeMapButton.Name = "largeMapButton";
            largeMapButton.Size = new Size(110, 110);
            largeMapButton.TabIndex = 4;
            largeMapButton.TabStop = true;
            largeMapButton.UseVisualStyleBackColor = true;
            // 
            // smallMapButton
            // 
            smallMapButton.Appearance = Appearance.Button;
            smallMapButton.BackgroundImage = (Image)resources.GetObject("smallMapButton.BackgroundImage");
            smallMapButton.Location = new Point(11, 37);
            smallMapButton.MaximumSize = new Size(110, 110);
            smallMapButton.MinimumSize = new Size(110, 110);
            smallMapButton.Name = "smallMapButton";
            smallMapButton.Size = new Size(110, 110);
            smallMapButton.TabIndex = 3;
            smallMapButton.TabStop = true;
            smallMapButton.UseVisualStyleBackColor = true;
            // 
            // mediumMapButton
            // 
            mediumMapButton.Appearance = Appearance.Button;
            mediumMapButton.BackgroundImage = (Image)resources.GetObject("mediumMapButton.BackgroundImage");
            mediumMapButton.Location = new Point(127, 37);
            mediumMapButton.MaximumSize = new Size(110, 110);
            mediumMapButton.MinimumSize = new Size(110, 110);
            mediumMapButton.Name = "mediumMapButton";
            mediumMapButton.Size = new Size(110, 110);
            mediumMapButton.TabIndex = 2;
            mediumMapButton.TabStop = true;
            mediumMapButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.Font = new Font("Unispace", 15.7499981F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(111, 385);
            label2.Name = "label2";
            label2.Size = new Size(257, 52);
            label2.TabIndex = 2;
            label2.Text = "Choose map";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // chooseGamemodeLabel
            // 
            chooseGamemodeLabel.Anchor = AnchorStyles.None;
            chooseGamemodeLabel.Font = new Font("Unispace", 15.7499981F, FontStyle.Bold, GraphicsUnit.Point);
            chooseGamemodeLabel.Location = new Point(111, 130);
            chooseGamemodeLabel.Name = "chooseGamemodeLabel";
            chooseGamemodeLabel.Size = new Size(257, 52);
            chooseGamemodeLabel.TabIndex = 1;
            chooseGamemodeLabel.Text = "Choose gamemode";
            chooseGamemodeLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.None;
            groupBox1.Controls.Add(recursiveGamemodeButton);
            groupBox1.Controls.Add(normalGamemodeButton);
            groupBox1.Controls.Add(laserGamemodeButton);
            groupBox1.Location = new Point(59, 176);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(362, 172);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // recursiveGamemodeButton
            // 
            recursiveGamemodeButton.Appearance = Appearance.Button;
            recursiveGamemodeButton.BackgroundImage = (Image)resources.GetObject("recursiveGamemodeButton.BackgroundImage");
            recursiveGamemodeButton.Location = new Point(243, 37);
            recursiveGamemodeButton.MaximumSize = new Size(110, 110);
            recursiveGamemodeButton.MinimumSize = new Size(110, 110);
            recursiveGamemodeButton.Name = "recursiveGamemodeButton";
            recursiveGamemodeButton.Size = new Size(110, 110);
            recursiveGamemodeButton.TabIndex = 4;
            recursiveGamemodeButton.UseVisualStyleBackColor = true;
            // 
            // normalGamemodeButton
            // 
            normalGamemodeButton.Appearance = Appearance.Button;
            normalGamemodeButton.BackgroundImage = (Image)resources.GetObject("normalGamemodeButton.BackgroundImage");
            normalGamemodeButton.Location = new Point(11, 37);
            normalGamemodeButton.MaximumSize = new Size(110, 110);
            normalGamemodeButton.MinimumSize = new Size(110, 110);
            normalGamemodeButton.Name = "normalGamemodeButton";
            normalGamemodeButton.Size = new Size(110, 110);
            normalGamemodeButton.TabIndex = 3;
            normalGamemodeButton.UseVisualStyleBackColor = true;
            // 
            // laserGamemodeButton
            // 
            laserGamemodeButton.Appearance = Appearance.Button;
            laserGamemodeButton.BackgroundImage = (Image)resources.GetObject("laserGamemodeButton.BackgroundImage");
            laserGamemodeButton.Location = new Point(127, 37);
            laserGamemodeButton.MaximumSize = new Size(110, 110);
            laserGamemodeButton.MinimumSize = new Size(110, 110);
            laserGamemodeButton.Name = "laserGamemodeButton";
            laserGamemodeButton.Size = new Size(110, 110);
            laserGamemodeButton.TabIndex = 2;
            laserGamemodeButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(479, 664);
            Controls.Add(menuPanel);
            Name = "Form1";
            Text = "Form1";
            Load += FormLoadEventHandler;
            menuPanel.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel menuPanel;
        private GroupBox groupBox1;
        private RadioButton normalGamemodeButton;
        private RadioButton laserGamemodeButton;
        private Label chooseGamemodeLabel;
        private GroupBox groupBox2;
        private Label label2;
        private RadioButton smallMapButton;
        private RadioButton mediumMapButton;
        private Button quitGameButton;
        private Button startGameButton;
        private RadioButton recursiveGamemodeButton;
        private RadioButton largeMapButton;
        private Button loadGameButton;
    }
}