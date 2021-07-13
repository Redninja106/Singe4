using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Singe4.Internal.Abstract
{
    internal interface IGraphics : IDisposable
    {
        ulong GetAvailableVideoMemory();
        ulong GetTotalVideoMemory();

        string GetAdapterName();

        void Clear(Color color);
        void DrawRectangle(float x, float y, float w, float h);
        void FillRectangle(float x, float y, float w, float h);
        void DrawEllipse(float x, float y, float w, float h);
        void FillEllipse(float x, float y, float w, float h);
        void SetStyle(GraphicsStyle style);
    }
}
