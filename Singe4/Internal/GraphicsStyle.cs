using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Singe4.Internal
{
    internal class GraphicsStyle
    {
        public Color BorderColor { get; set; } = Color.Black;
        public Color FillColor { get; set; } = Color.White;

        public float BorderWidth { get; set; } = 1f;
        public Matrix3x2 Transformation { get; set; } = Matrix3x2.Identity;

        public GraphicsStyle Duplicate()
        {
            return (GraphicsStyle)this.MemberwiseClone();
        }
    }
}
