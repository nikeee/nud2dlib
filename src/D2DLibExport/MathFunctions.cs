﻿namespace unvell.D2DLib
{
    public class MathFunctions
    {

        public static int Clamp(int v, int min = 0, int max = 255)
        {
            return v < min ? min : (v > max ? max : v);
        }

        public static float Clamp(float v, float min = 0, float max = 1)
        {
            return v < min ? min : (v > max ? max : v);
        }

        public static D2DColor Clamp(D2DColor c)
        {
            return new D2DColor(Clamp(c.A), Clamp(c.R), Clamp(c.G), Clamp(c.B));
        }
    }
}
