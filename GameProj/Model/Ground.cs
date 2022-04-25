using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GameProj.Model
{
    class Ground
    {
        public static int X { get; set; }
        public static int Y { get; set; }
        public int XScale { get; set; }
        public int YScale { get; set; }

        public Image Texture { get; set; }


        public Ground(int x, int y)
        {
            X = x;
            Y = y;
            Texture = new Bitmap("C:\\Users\\Kirill\\Desktop\\Game\\Images\\ground.jpg");
            XScale = 2000;
            YScale = 200;
        }
    }
}
