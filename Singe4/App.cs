using Singe4.Internal.Abstract;
using Singe4.Internal.Abstract.Implementations.Window.Win32;
using System;

namespace Singe4
{
    public static class App
    {
        static IPlatform platform;
        static IGraphics graphics;
        static uint window;

        static App()
        {
            
        }

        public static void Start(Game game)
        {
            platform = CreatePlatform();

            platform.CreateWindow("Singe4 App", 100, 100, -1, -1, out window);
            
            graphics = platform.CreateGraphics();


            if (window == 0)
            {
                throw new Exception("Error creating window!");
            }
            
            game.OnCreate();

            platform.ToggleWindowVisible(window);

            while (! platform.IsWindowClosed(window))
            {
                platform.HandleWindowEvents(window);

                game.OnDraw();

                game.OnUpdate();
            }
            
            platform.DestroyWindow(window);

            Destroy();
        }

        internal static IGraphics GetGraphics()
        {
            return graphics;
        }

        private static IPlatform CreatePlatform()
        {
            return new Win32Platform();
        }

        private static void Destroy()
        {
            platform.Dispose();
        }
    }
}
