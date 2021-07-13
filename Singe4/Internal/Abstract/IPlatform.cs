using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Singe4.Internal.Abstract
{
    internal interface IPlatform : IDisposable
    {
        // Windowing
        bool CreateWindow(string? title, int x, int y, int width, int height, out uint window);
        bool SetWindowTitle(uint window, string? title);
        bool MoveWindow(uint window, int x, int y);
        bool ResizeWindow(uint window, int width, int height);
        bool DestroyWindow(uint window);
        IEnumerable<uint> GetCurrentWindows();
        void HandleWindowEvents(uint window);
        bool IsWindowClosed(uint window);
        void ToggleWindowVisible(uint window);
        string GetWindowTitle(uint window);
        Vector2 GetWindowPosition(uint window);
        Vector2 GetWindowSize(uint window);

        // OS info
        string GetDeviceName();
        int GetCoreCount();
        ulong GetAvailableMemory();
        ulong GetTotalMemory();
        string GetOperatingSystem();
        string GetOperatingSystemVersion();

        // internal
        IGraphics CreateGraphics();
    }
}
