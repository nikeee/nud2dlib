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

namespace nud2dlib.Windows.Forms
{
    public class D2DControl : System.Windows.Forms.Control
    {
        private D2DDevice _device;
        public D2DDevice Device => _device ??= D2DDevice.FromHwnd(Handle);

        private D2DGraphics _graphics;

        private FpsCounter _fpsCounter = new();
        public bool DrawFps { get; set; }

        protected override void CreateHandle()
        {
            base.CreateHandle();

            DoubleBuffered = false;
            _device ??= D2DDevice.FromHwnd(Handle);
            _graphics = new D2DGraphics(_device);
        }

        private D2DBitmap? _backgroundImage = null;
        public new D2DBitmap? BackgroundImage
        {
            get => _backgroundImage;
            set
            {
                if (_backgroundImage == value)
                    return;
                _backgroundImage?.Dispose();
                _backgroundImage = value;
                Invalidate();
            }
        }

        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e) { }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (_backgroundImage == null)
                _graphics.BeginRender(D2DColor.FromGDIColor(BackColor));
            else
                _graphics.BeginRender(_backgroundImage);

            OnRender(_graphics);

            if (DrawFps)
                DrawAndCountFps();

            _graphics.EndRender();
        }

        private void DrawAndCountFps()
        {
            _fpsCounter.Update();

            var info = $"{_fpsCounter.FramesPerSecond} fps";
            var placeSize = new D2DSize(1000, 1000);
            var size = _graphics.MeasureText(info, Font.Name, Font.Size, placeSize);
            _graphics.DrawText(info, D2DColor.Black, ClientRectangle.Right - size.Width - 10, 5);
        }

        protected override void DestroyHandle()
        {
            base.DestroyHandle();
            _device?.Dispose();
        }

        protected virtual void OnRender(D2DGraphics g) { }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                //case (int)unvell.Common.Win32Lib.Win32.WMessages.WM_PAINT:
                //	base.WndProc(ref m);
                //	break;
                case (int)Win32.WMessages.WM_ERASEBKGND:
                    break;
                case (int)Win32.WMessages.WM_SIZE:
                    base.WndProc(ref m);
                    _device?.Resize();
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        public new void Invalidate() => base.Invalidate(false);
    }
}
