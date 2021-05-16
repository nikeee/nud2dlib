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
using System.Windows.Forms;

namespace nud2dlib.Windows.Forms
{
    public class D2DForm : Form
    {
        private D2DGraphics _graphics = null!;
        private D2DDevice _device = null!;
        public D2DDevice Device => _device ??= D2DDevice.FromHwnd(Handle);

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

        private FpsCounter _fpsCounter = new();
        public bool DrawFps { get; set; }

        private readonly Timer _timer = new() { Interval = 10 };
        public bool EscapeKeyToClose { get; set; } = true;

        private bool animationDraw;
        public bool AnimationDraw
        {
            get { return this.animationDraw; }
            set
            {
                this.animationDraw = value;

                if (!this.animationDraw)
                {
                    if (_timer.Enabled) _timer.Stop();
                }
                else
                {
                    if (!_timer.Enabled) _timer.Start();
                }
            }
        }

        protected bool SceneChanged { get; set; }

        protected override void CreateHandle()
        {
            base.CreateHandle();

            DoubleBuffered = false;

            if (_device == null)
            {
                _device = D2DDevice.FromHwnd(Handle);
            }

            _graphics = new D2DGraphics(_device);
            _graphics.SetDPI(96, 96);

            _timer.Tick += (ss, ee) =>
            {
                if (AnimationDraw || SceneChanged)
                {
                    OnFrame();
                    Invalidate();
                    SceneChanged = false;
                }
            };
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // prevent the .NET windows form to paint the original background
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (DesignMode)
            {
                e.Graphics.Clear(System.Drawing.Color.Black);
                e.Graphics.DrawString("D2DLib windows form cannot render in design time.", Font, System.Drawing.Brushes.White, 10, 10);
                return;
            }

            if (_backgroundImage == null)
                _graphics.BeginRender(D2DColor.FromGDIColor(BackColor));
            else
                _graphics.BeginRender(_backgroundImage);

            OnRender(_graphics);

            if (DrawFps)
                DrawAndCountFps();

            _graphics.EndRender();

            if (animationDraw && !_timer.Enabled)
            {
                _timer.Start();
            }
        }

        private void DrawAndCountFps()
        {
            _fpsCounter.Update();

            var info = $"{_fpsCounter.FramesPerSecond} fps";
            var placeSize = new D2DSize(1000, 1000);
            var size = _graphics.MeasureText(info, Font.Name, Font.Size, placeSize);
            _graphics.DrawText(info, D2DColor.Silver, ClientRectangle.Right - size.Width - 10, 5);
        }


        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)Win32.WMessages.WM_ERASEBKGND:
                    break;

                case (int)Win32.WMessages.WM_SIZE:
                    base.WndProc(ref m);
                    if (this._device != null)
                    {
                        this._device.Resize();
                        Invalidate(false);
                    }
                    break;

                case (int)Win32.WMessages.WM_DESTROY:
                    if (this._backgroundImage != null) this._backgroundImage.Dispose();
                    if (this._device != null) this._device.Dispose();
                    base.WndProc(ref m);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        protected virtual void OnRender(D2DGraphics g) { }

        protected virtual void OnFrame() { }

        public new void Invalidate()
        {
            base.Invalidate(false);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    if (EscapeKeyToClose) Close();
                    break;
            }
        }
    }
}
