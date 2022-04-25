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
        Timer Timer;
        Player Player;
        Window Window;
        Ground Ground;
        Platform Platform;
        Enemy Enemy1;
        Healthbar Healthbar;
        
        public void Initialize()
        {
            KeyPreview = true;
            Window = new Window(1200, 600);
            Player = new Player(100, 500);
            Enemy1 = new Enemy(200, 200);
            Ground = new Ground(0, 600);
            Platform = new Platform(350, 400);
            Healthbar = new Healthbar(10, 10, 20);
            SetTimer(10);
            SetWindowSettings();
        }

        public GameForm()
        {
            Initialize();
            KeyDown += new KeyEventHandler(OnKeyDown);
            KeyUp += new KeyEventHandler(OnKeyUp);
            Timer.Tick += new EventHandler(Update);
        }

        public void OnKeyDown(object args, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D:
                    {
                       Player.XSpeed = 7;
                       break;
                    }
                case Keys.A:
                    {
                        Player.XSpeed = -7;
                        break;
                    }
                case Keys.Space:
                    {
                        Player.Jump(20);
                        break;
                    }
            }
        }

        public void OnKeyUp(object args, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.A)
                Player.XSpeed = 0;
        }



        private void Update(object sender, EventArgs e)
        {
            Enemy1.MovmentSimulation();
            Player.PlayerParameters();
            Healthbar.UpdateInfo();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.DrawImage(Ground.Texture, Ground.X, Ground.Y, Ground.XScale, Ground.YScale);
            g.DrawImage(Player.Texture, Player.X, Player.Y, Player.XScale,Player.YScale);
            g.DrawImage(Enemy1.Texture, Enemy1.X, Enemy1.Y, Enemy1.XScale, Enemy1.YScale);
            g.DrawImage(Platform.Texture, Platform.X, Platform.Y, Platform.XScale, Platform.YScale);
            g.FillRectangle(new SolidBrush(Color.Red), Healthbar.X, Healthbar.Y, Healthbar.Width, Healthbar.Height);

        }

        private void SetWindowSettings()
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            DoubleBuffered = true;
            Point windowCoords = PointToScreen(new Point(Window.Width, Window.Height));
            this.Size = new Size(windowCoords);
            this.BackColor = Color.LightGray;

        }
        private void SetTimer(int interval)
        {
            Timer = new Timer();
            Timer.Interval = interval;
            Timer.Start();
        }
    }
}
