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
using HANDLE = System.IntPtr;
using HWND = System.IntPtr;
using UINT = System.UInt32;

namespace nud2dlib
{
    public class D2DDevice : IDisposable
    {
        internal HANDLE Handle { get; private set; }

        internal D2DDevice(HANDLE deviceHandle) => Handle = deviceHandle;

        public static D2DDevice FromHwnd(HWND hwnd)
        {
            var contextHandle = D2D.CreateContext(hwnd);
            return new D2DDevice(contextHandle);
        }

        public void Resize()
        {
            if (Handle != HANDLE.Zero)
                D2D.ResizeContext(Handle);
        }

        public D2DPen CreatePen(D2DColor color, D2DDashStyle dashStyle = D2DDashStyle.Solid, float[] customDashes = null, float dashOffset = 0.0f)
        {
            HANDLE handle = D2D.CreatePen(
                Handle,
                color,
                dashStyle,
                customDashes,
                customDashes != null ? (uint)customDashes.Length : 0,
                dashOffset
            );

            return handle == HANDLE.Zero
                ? null
                : new D2DPen(this, handle, color, dashStyle, customDashes, dashOffset);
        }

        internal void DestroyPen(D2DPen pen)
        {
            if (pen == null)
                throw new ArgumentNullException(nameof(pen));
            D2D.DestroyPen(pen.Handle);
        }

        public D2DSolidColorBrush CreateSolidColorBrush(D2DColor color)
        {
            var handle = D2D.CreateSolidColorBrush(Handle, color);
            return handle == HANDLE.Zero
                ? null
                : new D2DSolidColorBrush(handle, color);
        }

        public D2DLinearGradientBrush CreateLinearGradientBrush(Vector2 startPoint, Vector2 endPoint, D2DGradientStop[] gradientStops)
        {
            var handle = D2D.CreateLinearGradientBrush(Handle, startPoint, endPoint, gradientStops, (uint)gradientStops.Length);
            return new D2DLinearGradientBrush(handle, gradientStops);
        }

        public D2DRadialGradientBrush CreateRadialGradientBrush(Vector2 origin, Vector2 offset, FLOAT radiusX, FLOAT radiusY, D2DGradientStop[] gradientStops)
        {
            var handle = D2D.CreateRadialGradientBrush(Handle, origin, offset, radiusX, radiusY, gradientStops, (uint)gradientStops.Length);
            return new D2DRadialGradientBrush(handle, gradientStops);
        }

        public D2DRectangleGeometry CreateRectangleGeometry(FLOAT width, FLOAT height)
        {
            var rect = new D2DRect(0, 0, width, height);
            return CreateRectangleGeometry(rect);
        }

        public D2DRectangleGeometry CreateRectangleGeometry(D2DRect rect)
        {
            var rectGeometryHandle = D2D.CreateRectangleGeometry(Handle, ref rect);
            return new D2DRectangleGeometry(Handle, rectGeometryHandle);
        }

        public D2DPathGeometry CreatePathGeometry()
        {
            var geoHandle = D2D.CreatePathGeometry(Handle);
            return new D2DPathGeometry(Handle, geoHandle);
        }

        public D2DGeometry CreateEllipseGeometry(Vector2 origin, Vector2 radius)
        {
            var ellipse = new D2DEllipse(origin, radius);
            return new D2DGeometry(Handle, D2D.CreateEllipseGeometry(Handle, ref ellipse));
        }

        public D2DGeometry CreatePieGeometry(Vector2 origin, D2DSize size, float startAngle, float endAngle)
        {
            var path = CreatePathGeometry();

            var halfSize = new D2DSize(size.Width * 0.5f, size.Height * 0.5f);

            var sangle = startAngle * Math.PI / 180f;
            var eangle = endAngle * Math.PI / 180f;
            var angleDiff = endAngle - startAngle;

            var startPoint = new Vector2(
                (float)(origin.X + halfSize.Width * Math.Cos(sangle)),
                (float)(origin.Y + halfSize.Height * Math.Sin(sangle))
            );

            var endPoint = new Vector2(
                (float)(origin.X + halfSize.Width * Math.Cos(eangle)),
                (float)(origin.Y + halfSize.Height * Math.Sin(eangle))
            );

            path.AddLines(new Vector2[] { origin, startPoint });

            path.AddArc(
                endPoint,
                halfSize,
                angleDiff,
                angleDiff > 180
                    ? D2DArcSize.Large
                    : D2DArcSize.Small,
                D2DSweepDirection.Clockwise
            );

            path.ClosePath();

            return path;
        }

        public D2DBitmap LoadBitmap(byte[] buffer) => LoadBitmap(buffer, 0, (uint)buffer.Length);

        public D2DBitmap LoadBitmap(byte[] buffer, UINT offset, UINT length)
        {
            var bitmapHandle = D2D.CreateBitmapFromBytes(Handle, buffer, offset, length);
            return bitmapHandle == HWND.Zero
                ? null
                : new D2DBitmap(bitmapHandle);
        }

        public D2DBitmap LoadBitmap(string filepath) => CreateBitmapFromFile(filepath);

        public D2DBitmap CreateBitmapFromFile(string filepath)
        {
            var bitmapHandle = D2D.CreateBitmapFromFile(Handle, filepath);
            return bitmapHandle == HWND.Zero
                ? null
                : new D2DBitmap(bitmapHandle);
        }

        public D2DBitmap CreateBitmapFromMemory(UINT width, UINT height, UINT stride, IntPtr buffer, UINT offset, UINT length)
        {
            var d2dbmp = D2D.CreateBitmapFromMemory(Handle, width, height, stride, buffer, offset, length);
            return d2dbmp == HANDLE.Zero
                ? null
                : new D2DBitmap(d2dbmp);
        }

        public D2DLayer CreateLayer() => new D2DLayer(D2D.CreateLayer(Handle));

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr obj);

        public D2DBitmap CreateBitmapFromHBitmap(HWND hbmp, bool useAlphaChannel)
        {
            HANDLE d2dbmp = D2D.CreateBitmapFromHBitmap(Handle, hbmp, useAlphaChannel);
            return d2dbmp == HANDLE.Zero
                ? null
                : new D2DBitmap(d2dbmp);
        }

        public D2DBitmap CreateBitmapFromGDIBitmap(System.Drawing.Bitmap bmp)
        {
            bool useAlphaChannel = (bmp.PixelFormat & System.Drawing.Imaging.PixelFormat.Alpha) == System.Drawing.Imaging.PixelFormat.Alpha;

            return CreateBitmapFromGDIBitmap(bmp, useAlphaChannel);
        }

        public D2DBitmap CreateBitmapFromGDIBitmap(System.Drawing.Bitmap bmp, bool useAlphaChannel)
        {
            HANDLE d2dbmp = HANDLE.Zero;
            HANDLE hbitmap = bmp.GetHbitmap();

            if (hbitmap != HANDLE.Zero)
            {
                d2dbmp = D2D.CreateBitmapFromHBitmap(Handle, hbitmap, useAlphaChannel);
                DeleteObject(hbitmap);
            }

            return d2dbmp == HANDLE.Zero
                ? null
                : new D2DBitmap(d2dbmp);
        }

        public D2DBitmapGraphics CreateBitmapGraphics() => CreateBitmapGraphics(D2DSize.Empty);
        public D2DBitmapGraphics CreateBitmapGraphics(float width, float height) => CreateBitmapGraphics(new D2DSize(width, height));

        public D2DBitmapGraphics CreateBitmapGraphics(D2DSize size)
        {
            var bitmapRenderTargetHandle = D2D.CreateBitmapRenderTarget(Handle, size);
            return bitmapRenderTargetHandle == HANDLE.Zero
                ? null
                : new D2DBitmapGraphics(bitmapRenderTargetHandle);
        }

        public void Dispose() => D2D.DestroyContext(Handle);
    }
}
