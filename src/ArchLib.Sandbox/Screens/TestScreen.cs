using System;
using System.Collections.Generic;
using ArchLib.ControlFlow.Screens;
using ArchLib.Graphics;
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
        private TextureAtlas atlas;
        private BitmapFont font;

        public override void LoadContent()
        {
            tex = LocalContent.GetTexture("test_texture");
            atlas = LocalContent.GetTextureAtlas("test_atlas");
            font = LocalContent.GetFont("test_font");
        }

        public override void Update(double delta, bool topOfStack)
        {
        }

        public override void Draw(double delta, SpriteBatch batch, bool topOfStack)
        {
            tex.Draw(batch, new Vector2(100, 100));

            atlas["one"].Draw(batch, new Vector2(600, 600));

            font.DrawString(batch, "Hello, world!", 600, 200, Color.White);
        }
    }
}
