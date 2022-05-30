using System;
using System.Collections.Generic;
using System.Text;

namespace GameProj.Model
{
     class Window
    {
        public static int Height { get; private set; }
        public static int Width { get; private set; }
        public Window(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
