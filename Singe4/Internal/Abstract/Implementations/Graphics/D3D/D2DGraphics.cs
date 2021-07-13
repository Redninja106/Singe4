using Singe4.Internal.Abstract.Implementations.Window.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Graphics.Direct2D;
using Windows.Win32.Graphics.Direct3D11;
using Windows.Win32.Graphics.Dxgi;

namespace Singe4.Internal.Abstract.Implementations.Graphics.D3D
{
    internal sealed unsafe class D2DGraphics : IGraphics
    {
        private Win32Platform platform;
        private GraphicsStyle currentStyle = new GraphicsStyle(); // set to default style

        private IDXGISwapChain4* pSwapChain;
        private ID3D11Device* pDevice;
        private ID3D11DeviceContext* pContext;

        private ID2D1Factory* pD2DFactory;
        private ID2D1HwndRenderTarget* pRenderTarget;
        private ID2D1SolidColorBrush* pFillBrush;
        private ID2D1SolidColorBrush* pBorderBrush;

        public D2DGraphics(Win32Platform platform)
        {
            this.platform = platform;
            var window = platform.GetPrimaryWindow();

            //fixed (ID3D11Device** ppDevice = &pDevice)
            //    fixed (ID3D11DeviceContext** ppContext = &pContext)
            //        D3D11.D3D11CreateDevice(null, D3D_DRIVER_TYPE.D3D_DRIVER_TYPE_HARDWARE, null, D3D11_CREATE_DEVICE_FLAG.D3D11_CREATE_DEVICE_DEBUG, null, Constants.D3D11_SDK_VERSION, ppDevice, null, ppContext).ThrowOnFailure();

            var factoryOptions = new D2D1_FACTORY_OPTIONS { debugLevel = D2D1_DEBUG_LEVEL.D2D1_DEBUG_LEVEL_INFORMATION };
            
            var guid = typeof(ID2D1Factory).GUID;
            
            fixed (ID2D1Factory** ppD2DFactory = &pD2DFactory)
                d2d1.D2D1CreateFactory(D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_SINGLE_THREADED, &guid, &factoryOptions, (void**)ppD2DFactory);

            var rtProps = new D2D1_RENDER_TARGET_PROPERTIES
            {
                pixelFormat = new D2D1_PIXEL_FORMAT
                {
                    alphaMode = D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_IGNORE,
                    format = DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_TYPELESS
                },
                type = D2D1_RENDER_TARGET_TYPE.D2D1_RENDER_TARGET_TYPE_HARDWARE,
                usage = D2D1_RENDER_TARGET_USAGE.D2D1_RENDER_TARGET_USAGE_NONE
            };

            var windowSizeF = platform.GetWindowSize(window);
            var windowSize = new D2D_SIZE_U 
            { 
                height = (uint)windowSizeF.X, 
                width = (uint)windowSizeF.Y 
            };

            var hwndRtProps = new D2D1_HWND_RENDER_TARGET_PROPERTIES 
            { 
                hwnd = platform.GetHWND(window), 
                pixelSize = windowSize, 
                presentOptions = D2D1_PRESENT_OPTIONS.D2D1_PRESENT_OPTIONS_IMMEDIATELY 
            };

            fixed (ID2D1HwndRenderTarget** ppRenderTarget = &pRenderTarget)
                pD2DFactory->CreateHwndRenderTarget(&rtProps, &hwndRtProps, ppRenderTarget);

        }

        public void Clear(Color color)
        {
            var colorAsFloats = color.ToVector4RGBA();
            pRenderTarget->Clear((D2D1_COLOR_F*)&colorAsFloats);
        }

        public void Dispose()
        {
            pDevice->Release();
            pSwapChain->Release();
            pContext->Release();
            pRenderTarget->Release();
        }

        public void DrawEllipse(float x, float y, float w, float h)
        {
            D2D1_ELLIPSE ellipse = new D2D1_ELLIPSE()
            {
                point = new D2D_POINT_2F { x = x, y = y },
                radiusX = w * .5f,
                radiusY = h * .5f,
            };

            ID2D1Brush* pBrush = null;
            var guid = typeof(ID2D1Brush).GUID;

            if (pBorderBrush->QueryInterface(&guid, (void**)&pBrush).Succeeded)
            {
                pRenderTarget->DrawEllipse(&ellipse, pBrush, currentStyle.BorderWidth, null);
            }
        }

        public void DrawRectangle(float x, float y, float w, float h)
        {
            var rectangle = new D2D_RECT_F { left = x, top = y, right = x + w, bottom = w + h };

            ID2D1Brush* pBrush = null;
            var guid = typeof(ID2D1Brush).GUID;

            if (pFillBrush->QueryInterface(&guid, (void**)&pBrush).Succeeded)
            {
                pRenderTarget->DrawRectangle(&rectangle, pBrush, currentStyle.BorderWidth, null);
            }
        }

        public void FillEllipse(float x, float y, float w, float h)
        {
            D2D1_ELLIPSE ellipse = new D2D1_ELLIPSE()
            {
                point = new D2D_POINT_2F { x = x, y = y },
                radiusX = w * .5f,
                radiusY = h * .5f,
            };

            ID2D1Brush* pBrush = null;
            var guid = typeof(ID2D1Brush).GUID;

            if (pFillBrush->QueryInterface(&guid, (void**)&pBrush).Succeeded)
            {
                pRenderTarget->FillEllipse(&ellipse, pBrush);
            }
        }

        public void FillRectangle(float x, float y, float w, float h)
        {
            var rectangle = new D2D_RECT_F { left = x, top = y, right = x + w, bottom = w + h };

            ID2D1Brush* pBrush = null;
            var guid = typeof(ID2D1Brush).GUID;
            
            if (pFillBrush->QueryInterface(&guid, (void**)&pBrush).Succeeded)
            {
                pRenderTarget->FillRectangle(&rectangle, pBrush);
            }
        }

        public string GetAdapterName()
        {
            throw new NotImplementedException();
        }

        public ulong GetAvailableVideoMemory()
        {
            throw new NotImplementedException();
        }

        public ulong GetTotalVideoMemory()
        {
            throw new NotImplementedException();
        }

        public void SetStyle(GraphicsStyle style)
        {
            throw new NotImplementedException();
        }

        private void UpdateBrushes()
        {
            D2D1_BRUSH_PROPERTIES brushProps;

            var transform = this.currentStyle.Transformation;

            // update fill brush to match current style
            var fillColorF = this.currentStyle.FillColor.ToVector4RGBA();
            
            brushProps = new D2D1_BRUSH_PROPERTIES { opacity = fillColorF.W, transform = *(D2D_MATRIX_3X2_F*)&transform, };

            fixed (ID2D1SolidColorBrush** ppFillBrush = &this.pFillBrush)
                pRenderTarget->CreateSolidColorBrush((D2D1_COLOR_F*)&fillColorF, &brushProps, ppFillBrush);

            // update border brush to match current style
            var borderColorF = this.currentStyle.FillColor.ToVector4RGBA();
            
            brushProps = new D2D1_BRUSH_PROPERTIES { opacity = borderColorF.W, transform = *(D2D_MATRIX_3X2_F*)&transform, };

            fixed (ID2D1SolidColorBrush** ppBorderBrush = &this.pBorderBrush)
                pRenderTarget->CreateSolidColorBrush((D2D1_COLOR_F*)&borderColorF, &brushProps, ppBorderBrush);
        }
    }
}
