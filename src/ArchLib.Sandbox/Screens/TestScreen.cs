using System;
using System.Collections.Generic;
using ArchLib.ControlFlow.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Texture = ArchLib.Graphics.Texture;

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

        private Texture tex;

        public override void LoadContent()
        {
            tex = LocalContent.GetTexture("test_texture");
        }

        public override void Update(double delta, bool topOfStack)
        {
        }

        public override void Draw(double delta, SpriteBatch batch, bool topOfStack)
        {
            tex.Draw(batch, new Vector2(100, 100));
        }
    }
}
