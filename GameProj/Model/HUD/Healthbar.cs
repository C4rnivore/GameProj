using System;
using System.Collections.Generic;
using System.Text;

namespace GameProj.Model.HUD
{
    class Healthbar
    {
        public float X { get; set; }
        public float Y{ get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public Healthbar(float x, float y, float height)
        {
            X = x;
            Y = y;
            Width = Player.Health;
            Height = height;
        }

        public void UpdateInfo()
        {
            Width = Player.Health;
        }

    }
}
