using Singe4;
using System;
using System.Drawing;

namespace Tanks
{
    abstract class GameObject
    {
        public float X, Y, Rotation;
        public virtual void OnDraw()
        {
        }
        
        public virtual void OnUpdate()
        {

        }
    }

    class Tank : GameObject
    {
        public override void OnDraw()
        {
            //Graphics.FillRectangle(X, Y, 100, 100, Color.Red);
        }

        public override void OnUpdate()
        {
            
        }
    }


    class Program : Game
    {
        //Tank t;
        static void Main(string[] args)
        {
            App.Start(new Program());
        }

        public override void OnCreate()
        {
            //t = new();
        }

        public override void OnDraw()
        {
            //t.OnDraw();
            Graphics.Rectangle(100, 100, 100, 100);
        }

        public override void OnUpdate()
        {
            //t.OnUpdate();
        }
    }
}
