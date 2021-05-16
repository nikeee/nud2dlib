/*
* MIT License
*
* Copyright (c) 2009-2021 Jingwood, unvell.com. All right reserved.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using System;
using System.Numerics;
using System.Runtime.InteropServices;

using FLOAT = System.Single;

namespace unvell.D2DLib
{
    #region Color

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct D2DColor
    {
        public FLOAT R;
        public FLOAT G;
        public FLOAT B;
        public FLOAT A;

        public D2DColor(FLOAT r, FLOAT g, FLOAT b) : this(1, r, g, b) { }
        public D2DColor(FLOAT alpha, D2DColor color)
            : this(alpha, color.R, color.G, color.B) { }
        public D2DColor(FLOAT a, FLOAT r, FLOAT g, FLOAT b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public static D2DColor operator *(D2DColor c, float s) => new D2DColor(c.A, c.R * s, c.G * s, c.B * s);

        public static bool operator ==(D2DColor c1, D2DColor c2)
        {
            return c1.R == c2.R && c1.G == c2.G && c1.B == c2.B && c1.A == c2.A;
        }

        public static bool operator !=(D2DColor c1, D2DColor c2)
        {
            return c1.R != c2.R || c1.G != c2.G || c1.B != c2.B || c1.A != c2.A;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is D2DColor)) return false;
            var c2 = (D2DColor)obj;

            return R == c2.R && G == c2.G && B == c2.B && A == c2.A;
        }

        public override int GetHashCode() => HashCode.Combine(R, G, B, A);

        public static D2DColor FromGDIColor(System.Drawing.Color gdiColor) => new D2DColor(gdiColor.A / 255f, gdiColor.R / 255f, gdiColor.G / 255f, gdiColor.B / 255f);

        public static System.Drawing.Color ToGDIColor(D2DColor d2color)
        {
            var c = MathFunctions.Clamp(d2color * 255);
            return System.Drawing.Color.FromArgb((int)c.A, (int)c.R, (int)c.G, (int)c.B);
        }

        public static readonly D2DColor Transparent = new D2DColor(0, 0, 0, 0);

        public static readonly D2DColor Black = new D2DColor(0, 0, 0);
        public static readonly D2DColor DimGray = D2DColor.FromGDIColor(System.Drawing.Color.DimGray);
        public static readonly D2DColor Gray = D2DColor.FromGDIColor(System.Drawing.Color.Gray);
        public static readonly D2DColor DarkGray = D2DColor.FromGDIColor(System.Drawing.Color.DarkGray);
        public static readonly D2DColor Silver = D2DColor.FromGDIColor(System.Drawing.Color.Silver);
        public static readonly D2DColor GhostWhite = D2DColor.FromGDIColor(System.Drawing.Color.GhostWhite);
        public static readonly D2DColor LightGray = D2DColor.FromGDIColor(System.Drawing.Color.LightGray);
        public static readonly D2DColor White = D2DColor.FromGDIColor(System.Drawing.Color.White);
        public static readonly D2DColor SlateGray = D2DColor.FromGDIColor(System.Drawing.Color.SlateGray);
        public static readonly D2DColor DarkSlateGray = D2DColor.FromGDIColor(System.Drawing.Color.DarkSlateGray);
        public static readonly D2DColor WhiteSmoke = D2DColor.FromGDIColor(System.Drawing.Color.WhiteSmoke);

        public static readonly D2DColor Red = D2DColor.FromGDIColor(System.Drawing.Color.Red);
        public static readonly D2DColor DarkRed = D2DColor.FromGDIColor(System.Drawing.Color.DarkRed);
        public static readonly D2DColor PaleVioletRed = D2DColor.FromGDIColor(System.Drawing.Color.PaleVioletRed);
        public static readonly D2DColor OrangeRed = D2DColor.FromGDIColor(System.Drawing.Color.OrangeRed);
        public static readonly D2DColor IndianRed = D2DColor.FromGDIColor(System.Drawing.Color.IndianRed);
        public static readonly D2DColor MediumVioletRed = D2DColor.FromGDIColor(System.Drawing.Color.MediumVioletRed);
        public static readonly D2DColor Coral = D2DColor.FromGDIColor(System.Drawing.Color.Coral);
        public static readonly D2DColor LightCoral = D2DColor.FromGDIColor(System.Drawing.Color.LightCoral);

        public static readonly D2DColor Beige = D2DColor.FromGDIColor(System.Drawing.Color.Beige);
        public static readonly D2DColor Bisque = D2DColor.FromGDIColor(System.Drawing.Color.Bisque);
        public static readonly D2DColor LightYellow = D2DColor.FromGDIColor(System.Drawing.Color.LightYellow);
        public static readonly D2DColor Yellow = D2DColor.FromGDIColor(System.Drawing.Color.Yellow);
        public static readonly D2DColor Gold = D2DColor.FromGDIColor(System.Drawing.Color.Gold);
        public static readonly D2DColor Goldenrod = D2DColor.FromGDIColor(System.Drawing.Color.Goldenrod);
        public static readonly D2DColor LightGoldenrodYellow = D2DColor.FromGDIColor(System.Drawing.Color.LightGoldenrodYellow);
        public static readonly D2DColor Orange = D2DColor.FromGDIColor(System.Drawing.Color.Orange);
        public static readonly D2DColor DarkOrange = D2DColor.FromGDIColor(System.Drawing.Color.DarkOrange);
        public static readonly D2DColor BurlyWood = D2DColor.FromGDIColor(System.Drawing.Color.BurlyWood);
        public static readonly D2DColor Chocolate = D2DColor.FromGDIColor(System.Drawing.Color.Chocolate);

        public static readonly D2DColor LawnGreen = D2DColor.FromGDIColor(System.Drawing.Color.LawnGreen);
        public static readonly D2DColor LightGreen = D2DColor.FromGDIColor(System.Drawing.Color.LightGreen);
        public static readonly D2DColor LightSeaGreen = D2DColor.FromGDIColor(System.Drawing.Color.LightSeaGreen);
        public static readonly D2DColor MediumSeaGreen = D2DColor.FromGDIColor(System.Drawing.Color.MediumSeaGreen);
        public static readonly D2DColor DarkSeaGreen = D2DColor.FromGDIColor(System.Drawing.Color.DarkSeaGreen);
        public static readonly D2DColor Green = D2DColor.FromGDIColor(System.Drawing.Color.Green);
        public static readonly D2DColor DarkGreen = D2DColor.FromGDIColor(System.Drawing.Color.DarkGreen);
        public static readonly D2DColor DarkOliveGreen = D2DColor.FromGDIColor(System.Drawing.Color.DarkOliveGreen);
        public static readonly D2DColor ForestGreen = D2DColor.FromGDIColor(System.Drawing.Color.ForestGreen);
        public static readonly D2DColor GreenYellow = D2DColor.FromGDIColor(System.Drawing.Color.GreenYellow);

        public static readonly D2DColor AliceBlue = D2DColor.FromGDIColor(System.Drawing.Color.AliceBlue);
        public static readonly D2DColor LightBlue = D2DColor.FromGDIColor(System.Drawing.Color.LightBlue);
        public static readonly D2DColor Blue = D2DColor.FromGDIColor(System.Drawing.Color.Blue);
        public static readonly D2DColor DarkBlue = D2DColor.FromGDIColor(System.Drawing.Color.DarkBlue);
        public static readonly D2DColor SkyBlue = D2DColor.FromGDIColor(System.Drawing.Color.SkyBlue);
        public static readonly D2DColor SteelBlue = D2DColor.FromGDIColor(System.Drawing.Color.SteelBlue);
        public static readonly D2DColor BlueViolet = D2DColor.FromGDIColor(System.Drawing.Color.BlueViolet);
        public static readonly D2DColor CadetBlue = D2DColor.FromGDIColor(System.Drawing.Color.CadetBlue);
        public static readonly D2DColor BlanchedAlmond = D2DColor.FromGDIColor(System.Drawing.Color.BlanchedAlmond);
        public static readonly D2DColor PowderBlue = D2DColor.FromGDIColor(System.Drawing.Color.PowderBlue);
        public static readonly D2DColor CornflowerBlue = D2DColor.FromGDIColor(System.Drawing.Color.CornflowerBlue);

        public static readonly D2DColor Cyan = D2DColor.FromGDIColor(System.Drawing.Color.Cyan);
        public static readonly D2DColor DarkCyan = D2DColor.FromGDIColor(System.Drawing.Color.DarkCyan);
        public static readonly D2DColor LightCyan = D2DColor.FromGDIColor(System.Drawing.Color.LightCyan);

        public static readonly D2DColor Cornsilk = D2DColor.FromGDIColor(System.Drawing.Color.Cornsilk);
        public static readonly D2DColor Thistle = D2DColor.FromGDIColor(System.Drawing.Color.Thistle);
        public static readonly D2DColor Tomato = D2DColor.FromGDIColor(System.Drawing.Color.Tomato);

        public static readonly D2DColor Pink = D2DColor.FromGDIColor(System.Drawing.Color.Pink);
        public static readonly D2DColor DeepPink = D2DColor.FromGDIColor(System.Drawing.Color.DeepPink);
        public static readonly D2DColor HotPink = D2DColor.FromGDIColor(System.Drawing.Color.HotPink);
        public static readonly D2DColor LightPink = D2DColor.FromGDIColor(System.Drawing.Color.LightPink);
    }

    #endregion

    #region Rect

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct D2DRect
    {
        public FLOAT Left;
        public FLOAT Top;
        public FLOAT Right;
        public FLOAT Bottom;

        public D2DRect(Vector2 origin, D2DSize size)
            : this(origin.X - size.Width * 0.5f, origin.Y - size.Height * 0.5f, size.Width, size.Height)
        { }
        public D2DRect(float left, float top, float width, float height)
        {
            Left = left;
            Top = top;
            Right = left + width;
            Bottom = top + height;
        }

        public Vector2 Location
        {
            get => new Vector2(Left, Top);
            set
            {
                FLOAT width = Right - Left;
                FLOAT height = Bottom - Top;
                Left = value.X;
                Right = value.X + width;
                Top = value.Y;
                Bottom = value.Y + height;
            }
        }

        public FLOAT Width
        {
            get => Right - Left;
            set => Right = Left + value;
        }

        public FLOAT Height
        {
            get => Bottom - Top;
            set => Bottom = Top + value;
        }

        public void Offset(FLOAT x, FLOAT y)
        {
            Left += x;
            Right += x;
            Top += y;
            Bottom += y;
        }

        public FLOAT X
        {
            get => Left;
            set
            {
                FLOAT width = Right - Left;
                Left = value;
                Right = value + width;
            }
        }

        public FLOAT Y
        {
            get => Top;
            set
            {
                FLOAT height = Bottom - Top;
                Top = value;
                Bottom = value + height;
            }
        }

        public D2DSize Size
        {
            get => new D2DSize(Width, Height);
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public static implicit operator D2DRect(System.Drawing.Rectangle rect) => new D2DRect(rect.X, rect.Y, rect.Width, rect.Height);
        public static implicit operator D2DRect(System.Drawing.RectangleF rect) => new D2DRect(rect.X, rect.Y, rect.Width, rect.Height);
        public static implicit operator System.Drawing.RectangleF(D2DRect rect) => new System.Drawing.RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
        public static explicit operator System.Drawing.Rectangle(D2DRect rect) => System.Drawing.Rectangle.Round(rect);
    }

    #endregion

    #region Rounded Rect

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct D2DRoundedRect
    {
        public D2DRect Bounds;
        public Vector2 Radius;
    }

    #endregion

    #region Size

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct D2DSize
    {
        public static readonly D2DSize Empty = new D2DSize(0, 0);

        public FLOAT Width;
        public FLOAT Height;

        public D2DSize(FLOAT width, FLOAT height)
        {
            Width = width;
            Height = height;
        }

        public static implicit operator D2DSize(System.Drawing.Size wsize) => new D2DSize(wsize.Width, wsize.Height);
        public static implicit operator D2DSize(System.Drawing.SizeF wsize) => new D2DSize(wsize.Width, wsize.Height);
        public static implicit operator System.Drawing.SizeF(D2DSize s) => new System.Drawing.SizeF(s.Width, s.Height);
        public static explicit operator System.Drawing.Size(D2DSize s) => System.Drawing.Size.Round(s);

        public override string ToString() => $"D2DSize({Width}, {Height})";
    }

    #endregion

    #region Ellipse

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct D2DEllipse
    {
        /// <summary> AKA center </summary>
        public Vector2 Origin;
        public Vector2 Radius;

        public D2DEllipse(Vector2 center, Vector2 radius)
        {
            Origin = center;
            Radius = radius;
        }

        public D2DEllipse(FLOAT x, FLOAT y, FLOAT rx, FLOAT ry) : this(new Vector2(x, y), new Vector2(rx, ry)) { }

        public static D2DEllipse CreateCircle(Vector2 center, FLOAT radius) => new D2DEllipse(center, new Vector2(radius));

        public FLOAT X
        {
            get => Origin.X;
            set => Origin.X = value;
        }
        public FLOAT Y
        {
            get => Origin.Y;
            set => Origin.Y = value;
        }
    }

    #endregion

    #region BezierSegment
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct D2DBezierSegment
    {
        public Vector2 Point1;
        public Vector2 Point2;
        public Vector2 Point3;

        public D2DBezierSegment(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
        }

        public D2DBezierSegment(FLOAT x1, FLOAT y1, FLOAT x2, FLOAT y2, FLOAT x3, FLOAT y3)
        {
            Point1 = new Vector2(x1, y1);
            Point2 = new Vector2(x2, y2);
            Point3 = new Vector2(x3, y3);
        }
    }
    #endregion
}
