using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ArchLib.Runners
{
    public class XnaGame : Game
    {
        public readonly GraphicsDeviceManager GraphicsDeviceManager;
        
        internal XnaGame()
            : base()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
        }
    }
}
