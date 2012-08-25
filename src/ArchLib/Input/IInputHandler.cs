using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ArchLib.Input
{
    public interface IInputHandler
    {
        Boolean KeyDown(Keys k, Boolean control, Boolean shift, Boolean alt);
        Boolean KeyUp(Keys k);

        Boolean MouseButtonPressed(MouseButton button, Vector2 position, Boolean control, Boolean shift, Boolean alt);
        Boolean MouseButtonReleased(MouseButton button, Vector2 position);
        Boolean MouseScrollWheel(Int32 scrollDirection, Vector2 position);
        Boolean MouseMoved(Vector2 position, Vector2 delta);

        Boolean TouchPressed(Int32 id, Vector2 position);
        Boolean TouchMoved(Int32 id, Vector2 position, Vector2 delta);
        Boolean TouchReleased(Int32 id, Vector2 position);

        Boolean GamePadThumbstickMoved(PlayerIndex index, Thumbstick stick, Vector2 position, Vector2 delta);
        Boolean GamePadTriggerMoved(PlayerIndex index, Trigger trigger, Single position, Single delta);
        Boolean GamePadEventDown(PlayerIndex index, GamePadEvent eventType);
        Boolean GamePadEventUp(PlayerIndex index, GamePadEvent eventType);
    }
}
