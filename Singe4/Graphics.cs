using Singe4.Internal;
using Singe4.Internal.Abstract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singe4
{
    public static partial class Graphics
    {
        private static IGraphics graphics;

        private static Stack<GraphicsStyle> styles;

        private static GraphicsStyle currentStyle => styles.Peek();

        static Graphics()
        {
            styles = new Stack<GraphicsStyle>();
            styles.Push(new GraphicsStyle());
            graphics = App.GetGraphics();
        }

        internal static void Init(IGraphics graphics)
        {
            Graphics.graphics = graphics;
        }

        private static void UpdateStyle()
        {
            graphics!.SetStyle(styles.Peek());
        }

        #region Api

        public static void Rectangle(float x, float y, float width, float height)
        {
            if (currentStyle.FillColor.A != 0)
            {
                graphics.FillRectangle(x, y, width, height);
            }

            if (currentStyle.BorderColor.A != 0)
            {
                graphics.DrawRectangle(x, y, width, height);
            }
        }

        public static void Ellipse(float x, float y, float width, float height)
        {
            if (currentStyle.FillColor.A != 0)
            {
                graphics.FillEllipse(x, y, width, height);
            }

            if (currentStyle.BorderColor.A != 0)
            {
                graphics.FillEllipse(x, y, width, height);
            }
        }

        public static void Clear(Color color)
        {
            graphics!.Clear(color);
        }

        public static void PushStyle()
        {
            styles.Push(styles.Peek().Duplicate());

            UpdateStyle();
        }

        public static void PopStyle()
        {
            styles.Pop();

            UpdateStyle();
        }


        
        #endregion
    }
}
