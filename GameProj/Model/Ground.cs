using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GameProj.Model
{
    public class Ground
    {
        public  int X { get; set; }
        public  int Y { get; set; }
        public static Image Texture { get; set; }
        public int XScale { get; set; }
        public int YScale { get; set; }




        public Ground(int x, int y)
        {
            X = x;
            Y = y;
            Texture = new Bitmap("C:\\Users\\Kirill\\Desktop\\Game\\Images\\groundL.jpg");
            XScale = 2000;
            YScale = 200;
        }
    }
}
