using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Singe4.Internal.Abstract.Implementations
{
    internal static class Utility
    {
        public static Vector4 ToVector4RGBA(this Color color)
        {
            return new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }
        public static Vector4 ToVector4ARGB(this Color color)
        {
            return new Vector4(color.A / 255f, color.R / 255f, color.G / 255f, color.B / 255f);
        }
    }
}
