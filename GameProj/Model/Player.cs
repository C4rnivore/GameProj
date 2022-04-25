using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace GameProj.Model
{
     public class Player
    {
        //coords
        public static float X { get; set; }
        public static float Y { get; set; }
        public static float RightSide { get; set; }
        public static float LeftSide { get; set; }
        public static float Top { get; set; }
        public static float Bottom { get; set; }


        //movement
        public static float XSpeed { get; set; }
        public static float YSpeed { get; set; }


        //texture
        public Image Texture { get; set; }
        public int XScale { get; set; }
        public int YScale { get; set; }

        //size
        public static int Height { get; set; }
        public static int Width { get; set; }

        public static float Health { get; set; }

        public Player(int x, int y)
        {
            X = x;
            Y = y;
            Health = 150;
            Texture = new Bitmap("C:\\Users\\Kirill\\Desktop\\Game\\Images\\player.jpg");
            XScale = Texture.Width / 2;
            YScale = Texture.Height / 2;
            Width = Texture.Width / 2;
            Height = Texture.Height / 2;
        }


        public void PlayerParameters()
        {
            SimulateGravity();
            SimulatePlatformInteractions();

            RightSide = X + Width;
            LeftSide = X;
            Top = Y;
            Bottom = Y + Height;



            Health--;//healthbar test

            //Horizontal movment
            X += XSpeed;
            if (X < 0)
                X = 0;
            if (X + Width > Window.Width)
                X = Window.Width - Width;

        }

        public void Jump(int jumpforce)
        {
            YSpeed = CanJump() ? -jumpforce : YSpeed;
        }

        public bool CanJump()
        {
            return (Y + Height >= Ground.Y || 
                   ( Y + Height == Platform.Top && X + Width > Platform.LeftSide && X < Platform.RightSide));
        }

        public void SimulateGravity()
        {
            //gravity
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
        public void SimulatePlatformInteractions()
        {
            if (Y + Height >= Platform.Top && X + Width > Platform.LeftSide && X < Platform.RightSide)
            {
                if (Y + Height < Platform.Bottom)
                {
                    Y = Platform.Top - Height;
                    YSpeed = 0;
                }
            }
        }
    }
}
