using System;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib.ControlFlow.Screens.ActorModel
{
    public abstract class StageScreen : Screen
    {
        protected readonly Stage Stage = new Stage();

        public override void Update(Double delta, Boolean topOfStack)
        {
            Stage.Update(delta);
        }
        public override void Draw(Double delta, SpriteBatch batch, Boolean topOfStack)
        {
            Stage.Draw(delta, batch);
        }
    }
}
