using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace nud2dlib.Examples.SampleCode
{
    public partial class TextStyles : ExampleForm
    {
        public TextStyles()
        {
            Text = "Text Style Samples - d2dlib Examples";
            BackColor = Color.White;
        }

        static readonly D2DFontStyle[] SampleFontStyles = new D2DFontStyle[]{
             D2DFontStyle.Normal, D2DFontStyle.Oblique, D2DFontStyle.Italic
        };

        static readonly D2DFontWeight[] SampleFontWeights = new D2DFontWeight[]{
             D2DFontWeight.Normal, D2DFontWeight.Medium,
             D2DFontWeight.SemiLight, D2DFontWeight.Light, D2DFontWeight.UltraLight,
             D2DFontWeight.SemiBold, D2DFontWeight.Bold, D2DFontWeight.UltraBold,
        };

        static readonly D2DFontStretch[] SampleFontStretch = new D2DFontStretch[]{
             D2DFontStretch.Normal, D2DFontStretch.Medium, D2DFontStretch.Condensed,
        };

        protected override void OnRender(D2DGraphics g)
        {
            base.OnRender(g);

            var rect = new D2DRect(100, 20, 400, 20);

            int i = 0;
            foreach (var fontStyle in SampleFontStyles)
            {
                var styleKeyName = Enum.GetName(typeof(D2DFontStyle), fontStyle);

                foreach (var fontStretch in SampleFontStretch)
                {
                    var stretchKeyName = Enum.GetName(typeof(D2DFontStretch), fontStretch);

                    foreach (var fontWeight in SampleFontWeights)
                    {
                        var weightKeyName = Enum.GetName(typeof(D2DFontWeight), fontWeight);

                        g.DrawText((++i).ToString(), D2DColor.Black, "Arial", 10,
                            new D2DRect(rect.X, rect.Y - 10, 20, 20));

                        g.DrawText($"{styleKeyName}, {stretchKeyName}, {weightKeyName}", D2DColor.Black, "Arial", 16,
                            (D2DFontWeight)fontWeight, (D2DFontStyle)fontStyle, (D2DFontStretch)fontStretch,
                            rect);

                        rect.X += rect.Width + 100;

                        if (rect.X > ClientSize.Width - 100)
                        {
                            rect.X = 100;
                            rect.Y += 38;
                        }
                    }
                }
            }
        }
    }
}