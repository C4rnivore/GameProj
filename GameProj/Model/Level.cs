using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using GameProj.Model;

namespace GameProj.Model
{
     public class Level
    {
        public  Image BackgroundImage { get; private set; }
        public  Image GroundImage { get; private set; }
        public Ground Ground { get; private set; }
        public Player Player { get; private set; }
        public Enemy[] Enemies { get; private set; }
        public  Platform[] Platforms { get; private set; }

        public Level(int levelNum)
        {
            InitializeLevel(levelNum);
        }

        public void DrawLevel(Graphics g)
        {
            g.DrawImage(Ground.Texture, Ground.X, Ground.Y, Ground.XScale, Ground.YScale);

            foreach (var platform in Platforms)
            {
                g.DrawImage(platform.Texture, platform.X, platform.Y, platform.XScale, platform.YScale);
            }

            foreach (var enemy in Enemies)
            {
                g.DrawImage(enemy.Texture, enemy.X, enemy.Y, enemy.XScale, enemy.YScale);
            }
        }

        public void InitializeLevel(int levelNumber)
        {
            switch (levelNumber)
            {
                case (0):
                    {
                        Platforms = new Platform[]
                        {
                            new Platform(100, 200),
                            new Platform(300, 400),
                            new Platform(500, 500),
                            new Platform(700, 500),
                        };
                        Enemies = new Enemy[]
                        {
                            new Enemy(200,200)
                        };
                        Ground = new Ground(0, 750);
                        Player = new Player(100, 750 - Player.Height);

                        BackgroundImage = new Bitmap("C:\\Users\\Kirill\\Desktop\\Game\\Images\\0.jpg");
                        GroundImage = Ground.Texture;
                        return;
                    }

                case (1):
                    {
                        Platforms = new Platform[]
                        {
                            new Platform(100, 500),
                            new Platform(300, 300),
                            new Platform(500, 500),
                            new Platform(700, 300)

                        };
                        Ground = new Ground(0, 650);
                        Enemies = new Enemy[]
                        {
                            new Enemy(800, Ground.Y - 100),
                            new Enemy(900, Ground.Y - 100)
                        };
                        Player = new Player(0, 500);

                        BackgroundImage = new Bitmap("C:\\Users\\Kirill\\Desktop\\Game\\Images\\1.jpg");
                        GroundImage = new Bitmap("C:\\Users\\Kirill\\Desktop\\Game\\Images\\ground.jpg");
                        return;
                    }
            }
        }



    }
}
