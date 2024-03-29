﻿/*
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
using FLOAT = System.Single;
using HANDLE = System.IntPtr;

namespace nud2dlib
{
    public class D2DPathGeometry : D2DGeometry
    {
        internal D2DPathGeometry(HANDLE deviceHandle, HANDLE pathHandle)
            : base(deviceHandle, pathHandle) { }

        public void SetStartPoint(FLOAT x, FLOAT y) => SetStartPoint(new Vector2(x, y));

        public void SetStartPoint(Vector2 startPoint) => D2D.SetPathStartPoint(Handle, startPoint);

        public void AddLines(Vector2[] points) => D2D.AddPathLines(Handle, points);

        public void AddBeziers(D2DBezierSegment[] bezierSegments) => D2D.AddPathBeziers(Handle, bezierSegments);

        // TODO: unnecessary API and it doesn't work very well, consider to remove
        //public void AddEllipse(D2DEllipse ellipse)
        //{
        //	D2D.AddPathEllipse(Handle, ref ellipse);
        //}

        public void AddArc(Vector2 endPoint, D2DSize size, FLOAT sweepAngle,
                D2DArcSize arcSize = D2DArcSize.Small,
                D2DSweepDirection sweepDirection = D2DSweepDirection.Clockwise)
        {
            D2D.AddPathArc(Handle, endPoint, size, sweepAngle, arcSize, sweepDirection);
        }

        public bool FillContainsPoint(Vector2 point)
        {
            return D2D.PathFillContainsPoint(Handle, point);
        }

        public bool StrokeContainsPoint(Vector2 point, FLOAT width = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
        {
            return D2D.PathStrokeContainsPoint(Handle, point, width, dashStyle);
        }

        public void ClosePath() => D2D.ClosePath(Handle);

        public override void Dispose()
        {
            if (handle != IntPtr.Zero)
                D2D.DestroyPathGeometry(handle);
            handle = IntPtr.Zero;
        }
    }
}
