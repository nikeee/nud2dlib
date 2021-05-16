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

namespace nud2dlib
{
    public class D2DGraphics
    {
        internal HANDLE Handle { get; }

        public D2DDevice Device { get; }

        public D2DGraphics(D2DDevice context)
            : this(context.Handle)
        {
            Device = context;
        }

        public D2DGraphics(HANDLE handle) => Handle = handle;

        public void BeginRender() => D2D.BeginRender(Handle);
        public void BeginRender(D2DColor color) => D2D.BeginRenderWithBackgroundColor(Handle, color);
        public void BeginRender(D2DBitmap bitmap) => D2D.BeginRenderWithBackgroundBitmap(Handle, bitmap.Handle);
        public void EndRender() => D2D.EndRender(Handle);
        public void Flush() => D2D.Flush(Handle);

        private bool _antiAlias = true;

        public bool Antialias
        {
            get => _antiAlias;
            set
            {
                if (_antiAlias != value)
                {
                    D2D.SetContextProperties(
                        Handle,
                        value ? D2DAntialiasMode.PerPrimitive : D2DAntialiasMode.Aliased
                    );

                    _antiAlias = value;
                }
            }
        }

        public void DrawLine(
            FLOAT x1, FLOAT y1, FLOAT x2, FLOAT y2,
            D2DColor color,
            FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid, D2DCapStyle startCap = D2DCapStyle.Flat, D2DCapStyle endCap = D2DCapStyle.Flat
        )
        {
            DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), color, weight, dashStyle, startCap, endCap);
        }

        public void DrawLine(Vector2 start, Vector2 end, D2DColor color,
            FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid,
            D2DCapStyle startCap = D2DCapStyle.Flat, D2DCapStyle endCap = D2DCapStyle.Flat)
        {
            D2D.DrawLine(Handle, start, end, color, weight, dashStyle, startCap, endCap);
        }

        public void DrawLines(Vector2[] points, D2DColor color, FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
        {
            D2D.DrawLines(Handle, points, (uint)points.Length, color, weight, dashStyle);
        }

        public void DrawEllipse(FLOAT x, FLOAT y, FLOAT width, FLOAT height, D2DColor color,
            FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
        {
            var ellipse = new D2DEllipse(x, y, width / 2f, height / 2f);
            ellipse.Origin += ellipse.Radius;

            DrawEllipse(ellipse, color, weight, dashStyle);
        }

        public void DrawEllipse(Vector2 origin, Vector2 radius, D2DColor color, FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
        {
            var ellipse = new D2DEllipse(origin, radius);
            DrawEllipse(ellipse, color, weight, dashStyle);
        }

        public void DrawEllipse(Vector2 origin, FLOAT radialX, FLOAT radialY, D2DColor color, FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
        {
            var ellipse = new D2DEllipse(origin, new Vector2(radialX, radialY));
            DrawEllipse(ellipse, color, weight, dashStyle);
        }

        public void DrawEllipse(D2DEllipse ellipse, D2DColor color, FLOAT weight = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
        {
            D2D.DrawEllipse(Handle, ref ellipse, color, weight, dashStyle);
        }

        public void FillEllipse(Vector2 p, FLOAT radial, D2DColor color) => FillEllipse(p, radial, radial, color);

        public void FillEllipse(Vector2 p, FLOAT w, FLOAT h, D2DColor color)
        {
            D2DEllipse ellipse = new D2DEllipse(p, new Vector2(w / 2f, h / 2f));
            ellipse.Origin += ellipse.Radius;

            FillEllipse(ellipse, color);
        }

        public void FillEllipse(FLOAT x, FLOAT y, FLOAT radial, D2DColor color) => FillEllipse(new Vector2(x, y), radial, radial, color);
        public void FillEllipse(FLOAT x, FLOAT y, FLOAT w, FLOAT h, D2DColor color) => FillEllipse(new Vector2(x, y), w, h, color);
        public void FillEllipse(D2DEllipse ellipse, D2DColor color) => D2D.FillEllipse(Handle, ref ellipse, color);
        public void FillEllipse(D2DEllipse ellipse, D2DBrush brush) => D2D.FillEllipseWithBrush(Handle, ref ellipse, brush.Handle);

        public void DrawBeziers(D2DBezierSegment[] bezierSegments,
                                D2DColor strokeColor, FLOAT strokeWidth = 1,
                                D2DDashStyle dashStyle = D2DDashStyle.Solid)
        {
            D2D.DrawBeziers(Handle, bezierSegments, (uint)bezierSegments.Length, strokeColor, strokeWidth, dashStyle);
        }

        public void DrawPolygon(Vector2[] points,
            D2DColor strokeColor, FLOAT strokeWidth = 1f, D2DDashStyle dashStyle = D2DDashStyle.Solid)
        {
            DrawPolygon(points, strokeColor, strokeWidth, dashStyle, D2DColor.Transparent);
        }

        public void DrawPolygon(Vector2[] points,
            D2DColor strokeColor, FLOAT strokeWidth, D2DDashStyle dashStyle, D2DColor fillColor)
        {
            D2D.DrawPolygon(Handle, points, (uint)points.Length, strokeColor, strokeWidth, dashStyle, fillColor);
        }

        public void DrawPolygon(Vector2[] points,
            D2DColor strokeColor, FLOAT strokeWidth, D2DDashStyle dashStyle, D2DBrush fillBrush)
        {
            D2D.DrawPolygonWithBrush(Handle, points, (uint)points.Length, strokeColor, strokeWidth, dashStyle, fillBrush.Handle);
        }

        [Obsolete("FillPolygon will be removed from later versions. Use DrawPolygon instead")]
        public void FillPolygon(Vector2[] points, D2DColor fillColor)
        {
            DrawPolygon(points, D2DColor.Transparent, 0, D2DDashStyle.Solid, fillColor);
        }

        [Obsolete("FillPolygon will be removed from later versions. Use DrawPolygon instead")]
        public void FillPolygon(Vector2[] points, D2DBrush brush)
        {
            D2D.DrawPolygonWithBrush(Handle, points, (uint)points.Length, D2DColor.Transparent, 0, D2DDashStyle.Solid, brush.Handle);
        }

#if DEBUG
        public void TestDraw()
        {
            D2D.TestDraw(Handle);
        }
#endif // DEBUG

        public void PushClip(D2DRect rect) => D2D.PushClip(Handle, ref rect);

        public void PopClip() => D2D.PopClip(Handle);

        public D2DLayer PushLayer(D2DGeometry? geometry = null)
        {
            // FIXME: resolve to not use magic number
            var rectBounds = new D2DRect(-999999, -999999, 999999999, 999999999);

            return PushLayer(rectBounds, geometry);
        }

        public D2DLayer PushLayer(D2DRect rectBounds, D2DGeometry? geometry = null)
        {
            var layer = Device.CreateLayer();
            return PushLayer(layer, rectBounds, geometry);
        }

        public D2DLayer PushLayer(D2DLayer layer, D2DRect rectBounds, D2DGeometry? geometry = null)
        {
            D2D.PushLayer(Handle, layer.Handle, ref rectBounds, geometry?.Handle ?? IntPtr.Zero);
            return layer;
        }

        public void PopLayer() => D2D.PopLayer(Handle);
        public void SetTransform(Matrix3x2 mat) => D2D.SetTransform(Handle, ref mat);

        public Matrix3x2 GetTransform()
        {
            D2D.GetTransform(Handle, out var mat);
            return mat;
        }

        public void PushTransform() => D2D.PushTransform(Handle);
        public void PopTransform() => D2D.PopTransform(Handle);
        public void ResetTransform() => D2D.ResetTransform(Handle);
        public void RotateTransform(FLOAT angle) => D2D.RotateTransform(Handle, angle);
        public void RotateTransform(FLOAT angle, Vector2 center) => D2D.RotateTransform(Handle, angle, center);
        public void TranslateTransform(FLOAT x, FLOAT y) => D2D.TranslateTransform(Handle, x, y);
        public void ScaleTransform(FLOAT sx, FLOAT sy, [Optional] Vector2 center) => D2D.ScaleTransform(Handle, sx, sy, center);
        public void SkewTransform(FLOAT angleX, FLOAT angleY, [Optional] Vector2 center)
        {
            D2D.SkewTransform(Handle, angleX, angleY, center);
        }

        public void DrawRectangle(FLOAT x, FLOAT y, FLOAT w, FLOAT h, D2DColor color, FLOAT strokeWidth = 1,
            D2DDashStyle dashStyle = D2DDashStyle.Solid)
        {
            D2DRect rect = new D2DRect(x, y, w, h);
            D2D.DrawRectangle(Handle, ref rect, color, strokeWidth, dashStyle);
        }

        public void DrawRectangle(D2DRect rect, D2DColor color, FLOAT strokeWidth = 1,
            D2DDashStyle dashStyle = D2DDashStyle.Solid)
        {
            D2D.DrawRectangle(Handle, ref rect, color, strokeWidth, dashStyle);
        }

        public void DrawRectangle(Vector2 origin, D2DSize size, D2DColor color, FLOAT strokeWidth = 1,
            D2DDashStyle dashStyle = D2DDashStyle.Solid)
        {
            DrawRectangle(new D2DRect(origin, size), color, strokeWidth, dashStyle);
        }

        public void DrawRectangle(D2DRect rect, D2DPen strokePen, FLOAT strokeWidth = 1)
        {
            D2D.DrawRectangleWithPen(Handle, ref rect, strokePen.Handle, strokeWidth);
        }

        public void FillRectangle(float x, float y, float width, float height, D2DColor color)
        {
            var rect = new D2DRect(x, y, width, height);
            FillRectangle(rect, color);
        }

        public void FillRectangle(Vector2 origin, D2DSize size, D2DColor color) => FillRectangle(new D2DRect(origin, size), color);

        public void FillRectangle(D2DRect rect, D2DColor color) => D2D.FillRectangle(Handle, ref rect, color);

        public void FillRectangle(D2DRect rect, D2DBrush brush) => D2D.FillRectangleWithBrush(Handle, ref rect, brush.Handle);

        public void DrawRoundedRectangle(D2DRoundedRect roundedRect, D2DColor strokeColor, D2DColor fillColor,
            FLOAT strokeWidth = 1, D2DDashStyle dashStyle = D2DDashStyle.Solid)
        {
            D2D.DrawRoundedRect(Handle, ref roundedRect, strokeColor, fillColor, strokeWidth, dashStyle);
        }

        public void DrawRoundedRectangle(D2DRoundedRect roundedRect, D2DPen strokePen, D2DBrush fillBrush, FLOAT strokeWidth = 1)
        {
            D2D.DrawRoundedRectWithBrush(Handle, ref roundedRect, strokePen.Handle, fillBrush.Handle, strokeWidth);
        }

        public void DrawBitmap(D2DBitmap bitmap, D2DRect destRect, FLOAT opacity = 1,
            D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
        {
            var srcRect = new D2DRect(0, 0, bitmap.Width, bitmap.Height);
            DrawBitmap(bitmap, destRect, srcRect, opacity, interpolationMode);
        }

        public void DrawBitmap(D2DBitmap bitmap, D2DRect destRect, D2DRect srcRect, FLOAT opacity = 1,
            D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
        {
            D2D.DrawD2DBitmap(Handle, bitmap.Handle, ref destRect, ref srcRect, opacity, interpolationMode);
        }

        public void DrawBitmap(D2DBitmapGraphics bg, D2DRect rect, FLOAT opacity = 1,
            D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
        {
            D2D.DrawBitmapRenderTarget(Handle, bg.Handle, ref rect, opacity, interpolationMode);
        }

        public void DrawBitmap(D2DBitmapGraphics bg, FLOAT width, FLOAT height, FLOAT opacity = 1,
            D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
        {
            DrawBitmap(bg, new D2DRect(0, 0, width, height), opacity, interpolationMode);
        }

        public void DrawGDIBitmap(HANDLE hbitmap, D2DRect rect, D2DRect srcRect, FLOAT opacity = 1, bool alpha = false,
            D2DBitmapInterpolationMode interpolationMode = D2DBitmapInterpolationMode.Linear)
        {
            D2D.DrawGDIBitmapRect(Handle, hbitmap, ref rect, ref srcRect, opacity, alpha, interpolationMode);
        }

        public void DrawTextCenter(string text, D2DColor color, string fontName, float fontSize, D2DRect rect)
        {
            DrawText(text, color, fontName, fontSize, rect,
                DWriteTextAlignment.Center, DWriteParagraphAlignment.Center);
        }

        public void DrawText(string text, D2DColor color, string fontName, float fontSize, FLOAT x, FLOAT y,
          DWriteTextAlignment halign = DWriteTextAlignment.Leading,
          DWriteParagraphAlignment valign = DWriteParagraphAlignment.Near)
        {
            D2DRect rect = new D2DRect(x, y, 9999999, 9999999);  // FIXME: avoid magic number
            D2D.DrawText(Handle, text, color, fontName, fontSize, ref rect, halign, valign);
        }

        public void DrawText(string text, D2DColor color, string fontName, float fontSize, D2DRect rect,
                DWriteTextAlignment halign = DWriteTextAlignment.Leading,
                DWriteParagraphAlignment valign = DWriteParagraphAlignment.Near)
        {
            D2D.DrawText(Handle, text, color, fontName, fontSize, ref rect, halign, valign);
        }

        public D2DSize MeasureText(string text, string fontName, float fontSize, D2DSize placeSize)
        {
            D2DSize outputSize = placeSize;
            D2D.MeasureText(Handle, text, fontName, fontSize, ref outputSize);
            return outputSize;
        }

        public void DrawPath(D2DGeometry path, D2DColor strokeColor,
            FLOAT strokeWidth = 1f, D2DDashStyle dashStyle = D2DDashStyle.Solid)
        {
            D2D.DrawPath(path.Handle, strokeColor, strokeWidth, dashStyle);
        }

        public void DrawPath(D2DGeometry path, D2DPen strokePen, FLOAT strokeWidth = 1f) => D2D.DrawPathWithPen(path.Handle, strokePen.Handle, strokeWidth);
        public void FillPath(D2DGeometry path, D2DColor fillColor) => D2D.FillPathD(path.Handle, fillColor);
        public void Clear(D2DColor color) => D2D.Clear(Handle, color);
        public void GetDPI(out float dpiX, out float dpiY) => D2D.GetDPI(Device.Handle, out dpiX, out dpiY);
        public void SetDPI(float dpiX, float dpiY) => D2D.SetDPI(Device.Handle, dpiX, dpiY);
    }

}
