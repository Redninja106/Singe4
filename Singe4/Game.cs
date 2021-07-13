using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singe4
{
    public abstract class Game
    {
        public abstract void OnCreate();
        public abstract void OnDraw();
        public abstract void OnUpdate();
    }
}
