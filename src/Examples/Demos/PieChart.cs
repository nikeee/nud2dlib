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
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;
using nud2dlib.Windows.Forms;

namespace nud2dlib.Examples.Demos
{
	public partial class PieChart : DemoForm
	{
		private static readonly Random rand = new Random();
		List<PieInfo> pies = new List<PieInfo>();

		public PieChart()
		{
			Size = new Size(800, 1000);
			Text = "PieChart Demo - d2dlib Examples";

			CreateChart();
		}

		void CreateChart()
		{
			pies.Clear();

			// define the figure origin and size
			var figureOrigin = new Vector2(300, 300);
			var figureSize = new D2DSize(300, 300);

			var records = new float[] { .6f, .3f, .1f };

			float currentAngle = 0;

			// create pie geometries from records
			foreach (var record in records)
			{
				var angleSpan = record * 360;

				var path = Device.CreatePieGeometry(figureOrigin, figureSize, currentAngle, currentAngle + angleSpan);
				pies.Add(new PieInfo { path = path, color = RandomColor() });

				currentAngle += angleSpan;
			}

		}

		protected override void OnRender(D2DGraphics g)
		{
			base.OnRender(g);

			// draw background
			g.FillRectangle(100, 100, 400, 400, D2DColor.LightYellow);

			// draw pie geometries
			foreach (var pie in pies)
			{
				g.FillPath(pie.path, pie.color);
				g.DrawPath(pie.path, D2DColor.LightYellow, 2);
			}

			g.DrawText("Click to change color", D2DColor.Black, 250, 550);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			CreateChart();
			Invalidate();
		}

		/// <summary>Create color by randomly color components. </summary>
		public static D2DColor RandomColor()
		{
			return new D2DColor(1, (float)rand.NextDouble(), (float)rand.NextDouble(),
				(float)rand.NextDouble());
		}
	}

	class PieInfo
	{
		public D2DGeometry path;
		public D2DColor color;
	}
}
