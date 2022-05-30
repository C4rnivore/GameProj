using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GameProj.Model
{
     public class Platform
    {
        public  int X { get; set; }
        public  int Y { get; set; }
        public  int Top { get; set; }
        public  int Bottom { get; set; }
        public  int LeftSide { get; set; }
        public  int RightSide { get; set; }

        public  Image Texture { get; set; }
        public  int XScale { get; set; }
        public  int YScale { get; set; }

        public Platform(int x, int y)
        {
            X = x;
            Y = y;
            Texture = new Bitmap("C:\\Users\\Kirill\\Desktop\\Game\\Images\\platform.jpg");
            XScale = Texture.Width / 2;
            YScale = Texture.Height / 2;
            Top = Y;
            Bottom = Y + YScale;
            LeftSide = X;
            RightSide = X + XScale;

        }
    }
}
