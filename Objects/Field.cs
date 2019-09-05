namespace Objects
{
    public class Field
    {
        public int Left { get; }
        public int Right { get; }
        public int Top { get; }
        public int Down { get; }

        public int Height => Down - Top;
        public int Width => Right - Left;

        public Field(int height, int width) : this(0,0, width, height)
        {
        }

        public Field(int left, int top, int right, int down)
        {
            Down = down;
            Top = top;
            Left = left;
            Right = right;
        }
    }
}