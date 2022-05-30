using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GameProj.Model;
using GameProj.Model.HUD;

namespace GameProj
{
    public partial class GameForm : Form
    {
        public Timer Timer { get; private set; }
        public Timer AnimationDelayTimer { get; private set; }
        public Player Player { get; private set; }
        public static Level Level { get; private set; }
        public bool LevelInitialized { get; private set; }
        public int CurrentLevelNum { get; private set; }
        private Window Window;
        private GameStages GameStage;
        private enum GameStages
        {
            NotStarted,
            Started,
            Ended
        }
        public GameForm()
        {
            Initialize();
        }
        private void Initialize()
        {
            Window = new Window(1920, 1080);
            SetWindowSettings();
            SetEvents();

            GameStage = GameStages.NotStarted;
            LevelInitialized = false;
            KeyPreview = true;
        }

        
        
        private void SwitchStage(object args, EventArgs e)
        {
            if (GameStage == GameStages.NotStarted)
                GameStage = GameStages.Started;
            else if (GameStage == GameStages.Started)
                GameStage = GameStages.Ended;
        }
        private void ChangeLevel()
        {
            CurrentLevelNum++;
            LevelInitialized = false;
        }
        private void Stop(object args, EventArgs e)
        {
            Application.Exit();
        }



        private void DrawMenu()
        {
            BackgroundImage = new Bitmap(@"C:\Users\Kirill\Desktop\Game\Images\main.gif");
            var buttonLength = 200;
            var buttonHeight = 50;

            Button StartButton = new Button()
            {
                Location = new Point(300, 300),
                Text = "Start Game",
                Size = new Size(buttonLength, buttonHeight),
                BackgroundImage = new Bitmap(@"C:\Users\Kirill\Desktop\Game\Images\platform.jpg"),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Georgia", 15, FontStyle.Bold, GraphicsUnit.Point)
            };
            Button QuitButton = new Button()
            {
                Location = new Point(300, StartButton.Bottom + 20),
                Text = "Quit Game",
                Size = new Size(buttonLength, buttonHeight),
                BackgroundImage = new Bitmap(@"C:\Users\Kirill\Desktop\Game\Images\platform.jpg"),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Georgia", 15, FontStyle.Bold, GraphicsUnit.Point)
            };

            StartButton.Click += new EventHandler(SwitchStage);
            QuitButton.Click += new EventHandler(Stop);
            Controls.Add(StartButton);
            Controls.Add(QuitButton);
        }
        private void DrawLevel(int currentLevelNum)
        {
            Level = new Level(currentLevelNum);
            Player = Level.Player;
            DrawLevelBackground(Level);
            LevelInitialized = true;
        }
        private void DrawLevelBackground(Level level)
        {
            BackgroundImage = level.BackgroundImage;
            BackgroundImageLayout = ImageLayout.Center;
        }

      

        private void OnKeyUp(object args, KeyEventArgs e)
        {
            if (GameStage == GameStages.Started)
            {
                if (e.KeyCode == Keys.D || e.KeyCode == Keys.A)
                {
                    Player.Stay();
                }
            }  
        }
        private void OnKeyDown(object args, KeyEventArgs e)
        {
            if (GameStage == GameStages.Started)
            {
                switch (e.KeyCode)
                {
                    case Keys.D:
                        {
                            Player.Run(1);
                            break;
                        }
                    case Keys.A:
                        {
                            Player.Run(-1);
                            break;
                        }
                    case Keys.Space:
                        {
                            Player.Jump(30);
                            break;
                        }
                }
            }
        }
        private void OnClick(object args, EventArgs e)
        {
            if(GameStage == GameStages.Started)
            Player.Attack(10,Level.Enemies);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            if (GameStage == GameStages.NotStarted)
            {

            }
            else if (GameStage == GameStages.Started)
            {
                if (LevelInitialized)
                {
                    Level.DrawLevel(g);
                    g.DrawImage(Player.Texture, Player.X, Player.Y, Player.XScale, Player.YScale);
                }
            }
            else if (GameStage == GameStages.Ended)
            {

            }
            else throw new NotImplementedException();
        }
        private void Update(object sender, EventArgs e)
        {
            if (GameStage == GameStages.NotStarted)
            {
                DrawMenu();
            }
            else if (GameStage == GameStages.Started)
            {
                if (!LevelInitialized)
                {
                    Controls.Clear();
                    DrawLevel(CurrentLevelNum);
                }
                foreach (var enemy in Level.Enemies)
                {
                    enemy.EnemyInteractions();
                }
                Player.PlayerInteractions();
            }
            else if (GameStage == GameStages.Ended)
            {

            }
            else throw new NotImplementedException();
            Invalidate();
        }
        private void UpdateAnimationFrame(object sender, EventArgs e)
        {
            if (GameStage == GameStages.Started && LevelInitialized)
            {
                Player.PlayCurrentAnimation();
                foreach (var enemy in Level.Enemies)
                    enemy.PlayCurrentAnimation();
            }
        }



        private void SetWindowSettings()
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            DoubleBuffered = true;
            Point windowCoords = PointToScreen(new Point(Window.Width, Window.Height));
            Size = new Size(windowCoords);
            this.WindowState = FormWindowState.Maximized; // fullscreen
        }
        private void SetEvents()
        {
            SetControlEvents();
            SetTimerEvents();
        }
        private void SetControlEvents()
        {
            KeyDown += new KeyEventHandler(OnKeyDown);
            KeyUp += new KeyEventHandler(OnKeyUp);
            Click += new EventHandler(OnClick);
        }
        private void SetTimerEvents()
        {
            Timer = new Timer { Interval = 10 };
            Timer.Tick += new EventHandler(Update);
            Timer.Start();

            AnimationDelayTimer = new Timer { Interval = 100 };
            AnimationDelayTimer.Tick += new EventHandler(UpdateAnimationFrame);
            AnimationDelayTimer.Start();
        }
    }
}
