using System;
using ArchLib.Content;
using ArchLib.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArchLib.ControlFlow.Screens
{
    public abstract class Screen : IInputHandler, IDisposable
    {
        private Boolean _contentLoaded;

        protected readonly ContentContext GlobalContent;
        protected readonly ContentContext LocalContent;

        protected Screen()
        {
            GlobalContent = Arch.GlobalContent;
            LocalContent = GlobalContent.CreateChildContext();
        }

        public abstract Boolean ShouldSoakDraws { get; }
        public abstract Boolean ShouldSoakInput { get; }
        public abstract Boolean ShouldSoakUpdates { get; }


        public void DoLoadContent()
        {
            if (_contentLoaded == false)
            {
                _contentLoaded = true;
                LoadContent();
            }
        }

        public Boolean IsContentLoaded { get { return _contentLoaded; } }

        public abstract void LoadContent();

        public abstract void Update(Double delta, Boolean topOfStack);
        public abstract void Draw(Double delta, SpriteBatch batch, Boolean topOfStack);
        

        public void Dispose()
        {
            DoDispose();
        }
        public virtual void DoDispose() { }


        public virtual void LostFocus(Screen newTopScreen)
        {
        }
        public virtual void GotFocus(Screen oldTopScreen)
        {
        }

        public virtual Boolean KeyDown(Keys k, Boolean control, Boolean shift, Boolean alt) { return true; }
        public virtual Boolean KeyUp(Keys k) { return true; }

        public virtual Boolean MouseButtonPressed(MouseButton button, Vector2 position, Boolean control, Boolean shift, Boolean alt) { return true; }
        public virtual Boolean MouseButtonReleased(MouseButton button, Vector2 position) { return true; }
        public virtual Boolean MouseScrollWheel(Int32 scrollDirection, Vector2 position) { return true; }
        public virtual Boolean MouseMoved(Vector2 position, Vector2 delta) { return true; }

        public virtual Boolean TouchPressed(Int32 id, Vector2 position) { return true; }
        public virtual Boolean TouchMoved(Int32 id, Vector2 position, Vector2 delta) { return true; }
        public virtual Boolean TouchReleased(Int32 id, Vector2 position) { return true; }

        public virtual Boolean GamePadThumbstickMoved(PlayerIndex index, Thumbstick stick, Vector2 position, Vector2 delta) { return true; }
        public virtual Boolean GamePadTriggerMoved(PlayerIndex index, Trigger trigger, Single position, Single delta) { return true; }
        public virtual Boolean GamePadEventDown(PlayerIndex index, GamePadEvent eventType) { return true; }
        public virtual Boolean GamePadEventUp(PlayerIndex index, GamePadEvent eventType) { return true; }
    }
}
