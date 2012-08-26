using System;
using System.Collections.Generic;
using ArchLib.ControlFlow.Screens;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib.Sandbox.Screens
{
    public class TestScreen : Screen
    {
        public override bool ShouldSoakDraws
        {
            get { return true; }
        }

        public override bool ShouldSoakInput
        {
            get { return true; }
        }

        public override bool ShouldSoakUpdates
        {
            get { return true; }
        }

        public override void LoadContent()
        {
        }

        public override void Update(double delta, bool topOfStack)
        {
        }

        public override void Draw(double delta, SpriteBatch batch, bool topOfStack)
        {
        }
    }
}
