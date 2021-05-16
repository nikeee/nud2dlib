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
using HANDLE = System.IntPtr;

namespace unvell.D2DLib
{
    public class D2DPen : D2DObject
    {
        public D2DDevice Device { get; private set; }

        public D2DColor Color { get; private set; }

        public D2DDashStyle DashStyle { get; private set; }

        public float[]? CustomDashes { get; private set; }

        public float DashOffset { get; private set; }

        internal D2DPen(D2DDevice device, HANDLE handle, D2DColor color, D2DDashStyle dashStyle = D2DDashStyle.Solid, float[] customDashes = null, float dashOffset = 0f)
            : base(handle)
        {
            Device = device;
            Color = color;
            DashStyle = dashStyle;
            CustomDashes = customDashes;
            DashOffset = dashOffset;
        }

        public override void Dispose()
        {
            if (Handle != IntPtr.Zero)
            {
                Device.DestroyPen(this);
                handle = IntPtr.Zero;
            }
        }
    }
}
