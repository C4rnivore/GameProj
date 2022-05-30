namespace GameProj.Model
{
    public class HitBox
    {
        public float X { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }
        public HitBox(float x, float top, float bottom)
        {
            X = x;
            Top = top;
            Bottom = bottom;
        }
    }
}
