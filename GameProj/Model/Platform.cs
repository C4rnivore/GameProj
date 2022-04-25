using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GameProj.Model
{
     public class Platform
    {
        public static int X { get; set; }
        public static int Y { get; set; }
        public static int Top { get; set; }
        public static int Bottom { get; set; }
        public static int LeftSide { get; set; }
        public static int RightSide { get; set; }

        public static Image Texture { get; set; }
        public static int XScale { get; set; }
        public static int YScale { get; set; }

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
