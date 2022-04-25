using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GameProj.Model
{
    class Enemy
    {
        //coords
        public float X { get; set; }
        public float Y { get; set; }

        //movement
        public float XSpeed { get; set; }
        public float YSpeed { get; set; }


        //texture
        public Image Texture { get; set; }
        public int XScale { get; set; }
        public int YScale { get; set; }

        //size
        public int Height { get; set; }
        public int Width { get; set; }


        public Enemy(int x, int y)
        {
            X = x;
            Y = y;
            Texture = new Bitmap("C:\\Users\\Kirill\\Desktop\\Game\\Images\\enemy.jpg");
            XScale = Texture.Width / 2;
            YScale = Texture.Height / 2;
            Width = Texture.Width / 2;
            Height = Texture.Height / 2;
        }

        public void SimulateEnemiesBehaviour()
        {
            if (Y == Player.Y)
            {
                if (X < Player.X)
                {
                    XSpeed = 4;
                }
                else
                {
                    XSpeed = -4;
                }
            }
            else
                XSpeed = 0;
        }
        public void SimulateGravity()
        {
            Y += YSpeed;
            YSpeed += 1;
            if (YSpeed > 25)
                YSpeed = 25;

            if (Y + Height >= Ground.Y)
            {
                Y = Ground.Y - Height;
                YSpeed = 0;
            }
        }

        public bool CanSeePlayer()
        {
            return (Math.Abs(Player.X - X) < 200);
        }
        public void SimuateAgressiveBehaviour()
        {
            if (CanSeePlayer())
            {
                if (X < Player.X)
                {
                    XSpeed = 4.5f;
                }
                else if (X > Player.X)
                {
                    XSpeed = -4.5f;
                }
                if (Y >= Player.Y && Math.Abs(X - Player.X) < Player.Width)
                    XSpeed = 0;

            }
            else
                XSpeed = 0;
        }
        public void MovmentSimulation()
        {
            SimulateGravity();
            SimuateAgressiveBehaviour();


            X += XSpeed;

            if (X < 0)
                X = 0;
            if (X + Width > Window.Width)
                X = Window.Width - Width;


        }


    }
}
