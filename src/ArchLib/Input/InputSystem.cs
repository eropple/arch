using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#if MOBILE
using Microsoft.Xna.Framework.Input.Touch;
#endif

namespace ArchLib.Input
{
    public class InputSystem
    {
        private KeyboardState _lastKeyboardState;
        private MouseState _lastMouseState;
        private Vector2 _lastRealMousePosition = Vector2.Zero;
        private Vector2 _lastEffectiveMousePosition = Vector2.Zero;
        private Dictionary<PlayerIndex, GamePadState> _lastGamePadStates;
        private HashSet<PlayerIndex> _watchedGamePads; 
#if MOBILE
        private TouchCollection _lastTouchCollection;
#endif

        /// <summary>
        /// The input handler for your game. Defaults to Arch.Screens.
        /// </summary>
        public IInputHandler InputHandler { get; set; }

        internal InputSystem()
        {
            _lastGamePadStates = new Dictionary<PlayerIndex, GamePadState>(4);
            _watchedGamePads = new HashSet<PlayerIndex>();
        }

        public Vector2 MousePosition { get { return _lastEffectiveMousePosition; } }
        public Vector2 RealMousePosition { get { return _lastRealMousePosition; } }

        public void Initialize()
        {
            _lastKeyboardState = Keyboard.GetState();
            _lastMouseState = Mouse.GetState();
#if MOBILE
            _lastTouchCollection = TouchPanel.GetState();
            if (TouchPanel.GetCapabilities().IsConnected)
            {
                TouchPanel.EnabledGestures = GestureType.None;
            }
#endif
        }

        public void WatchGamePad(PlayerIndex index)
        {
            if (_lastGamePadStates.ContainsKey(index)) return;

            _lastGamePadStates.Add(index, GamePad.GetState(index));
            _watchedGamePads.Add(index);
        }

        public void UnwatchGamePad(PlayerIndex index)
        {
            if (!_lastGamePadStates.ContainsKey(index)) return;

            _lastGamePadStates.Remove(index);
            _watchedGamePads.Remove(index);
        }

        public ISet<PlayerIndex> WatchedGamePads
        {
            get { return _watchedGamePads; }
        }

        
        public void Update()
        {
#if DESKTOP
            UpdateKeyboard();
            UpdateMouse();
#endif
#if !MOBILE || ANDROID
            UpdateGamePads();
#endif
#if MOBILE
            if (TouchPanel.GetCapabilities().IsConnected) UpdateTouch();
#endif
        }

        private void UpdateKeyboard()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            Keys[] currentDownKeys = keyboardState.GetPressedKeys();
            Keys[] lastDownKeys = _lastKeyboardState.GetPressedKeys();

            Boolean control = keyboardState.IsKeyDown(Keys.LeftControl) ||
                              keyboardState.IsKeyDown(Keys.RightControl);
            Boolean alt = keyboardState.IsKeyDown(Keys.LeftAlt) ||
                          keyboardState.IsKeyDown(Keys.RightAlt);
            Boolean shift = keyboardState.IsKeyDown(Keys.LeftShift) ||
                            keyboardState.IsKeyDown(Keys.RightShift);



            for (int i = 0; i < lastDownKeys.Length; ++i)
            {
                Keys k = lastDownKeys[i];

                if (k == Keys.LeftControl || k == Keys.RightControl ||
                    k == Keys.LeftShift || k == Keys.RightShift ||
                    k == Keys.LeftAlt || k == Keys.RightAlt) continue;

                if (!keyboardState.IsKeyDown(k))
                {
                    // key has been raised
                    InputHandler.KeyUp(k);
                }
            }

            for (int i = 0; i < currentDownKeys.Length; ++i)
            {
                Keys k = currentDownKeys[i];

                if (k == Keys.LeftControl || k == Keys.RightControl ||
                    k == Keys.LeftShift || k == Keys.RightShift ||
                    k == Keys.LeftAlt || k == Keys.RightAlt) continue;

                if (!_lastKeyboardState.IsKeyDown(k))
                {
                    // key wasn't down last frame, fire keydown
                    InputHandler.KeyDown(k, control, shift, alt);
                }
            }

            _lastKeyboardState = keyboardState;
        }

        private void UpdateMouse()
        {
            MouseState mouseState = Mouse.GetState();

            Vector2 realMousePosition = new Vector2(mouseState.X, mouseState.Y);
            Vector2 effectiveMousePosition = Arch.Scaling.RealCoordsToVirtual(realMousePosition);

            Boolean control = _lastKeyboardState.IsKeyDown(Keys.LeftControl) ||
                              _lastKeyboardState.IsKeyDown(Keys.RightControl);
            Boolean alt = _lastKeyboardState.IsKeyDown(Keys.LeftAlt) ||
                          _lastKeyboardState.IsKeyDown(Keys.RightAlt);
            Boolean shift = _lastKeyboardState.IsKeyDown(Keys.LeftShift) ||
                            _lastKeyboardState.IsKeyDown(Keys.RightShift);

            Int32 detents = mouseState.ScrollWheelValue - _lastMouseState.ScrollWheelValue;
            if (detents != 0)
            {
                InputHandler.MouseScrollWheel(Convert.ToInt32(Math.Ceiling((Single)detents / 120.0f)), effectiveMousePosition);
            }


            if (effectiveMousePosition.X != _lastEffectiveMousePosition.X ||
                effectiveMousePosition.Y != _lastEffectiveMousePosition.Y)
            {
                InputHandler.MouseMoved(effectiveMousePosition, new Vector2(effectiveMousePosition.X - _lastEffectiveMousePosition.X, 
                    effectiveMousePosition.Y - _lastEffectiveMousePosition.Y));
            }


            if (mouseState.LeftButton == ButtonState.Released &&
                _lastMouseState.LeftButton == ButtonState.Pressed)
            {
                InputHandler.MouseButtonReleased(MouseButton.Left, effectiveMousePosition);
            }
            else if (mouseState.LeftButton == ButtonState.Pressed &&
                _lastMouseState.LeftButton == ButtonState.Released)
            {
                InputHandler.MouseButtonPressed(MouseButton.Left, effectiveMousePosition, control, shift, alt);
            }

            if (mouseState.MiddleButton == ButtonState.Released &&
                _lastMouseState.MiddleButton == ButtonState.Pressed)
            {
                InputHandler.MouseButtonReleased(MouseButton.Middle, effectiveMousePosition);
            }
            else if (mouseState.MiddleButton == ButtonState.Pressed &&
                _lastMouseState.MiddleButton == ButtonState.Released)
            {
                InputHandler.MouseButtonPressed(MouseButton.Middle, effectiveMousePosition, control, shift, alt);
            }

            if (mouseState.RightButton == ButtonState.Released &&
                _lastMouseState.RightButton == ButtonState.Pressed)
            {
                InputHandler.MouseButtonReleased(MouseButton.Right, effectiveMousePosition);
            }
            else if (mouseState.RightButton == ButtonState.Pressed &&
                _lastMouseState.RightButton == ButtonState.Released)
            {
                InputHandler.MouseButtonPressed(MouseButton.Right, effectiveMousePosition, control, shift, alt);
            }

            if (mouseState.XButton1 == ButtonState.Released &&
                _lastMouseState.XButton1 == ButtonState.Pressed)
            {
                InputHandler.MouseButtonReleased(MouseButton.XButton1, effectiveMousePosition);
            }
            else if (mouseState.XButton1 == ButtonState.Pressed &&
                _lastMouseState.XButton1  == ButtonState.Released)
            {
                InputHandler.MouseButtonPressed(MouseButton.XButton1, effectiveMousePosition, control, shift, alt);
            }

            if (mouseState.XButton2 == ButtonState.Released &&
                _lastMouseState.XButton2 == ButtonState.Pressed)
            {
                InputHandler.MouseButtonReleased(MouseButton.XButton2, effectiveMousePosition);
            }
            else if (mouseState.XButton2 == ButtonState.Pressed &&
                _lastMouseState.XButton2 == ButtonState.Released)
            {
                InputHandler.MouseButtonPressed(MouseButton.XButton2, effectiveMousePosition, control, shift, alt);
            }

            _lastMouseState = mouseState;
            _lastRealMousePosition = realMousePosition;
            _lastEffectiveMousePosition = effectiveMousePosition;
        }

#if MOBILE
        private void UpdateTouch()
        {
            TouchCollection touchCollection = TouchPanel.GetState();

            foreach (TouchLocation tl in touchCollection)
            {
                if (tl.State == TouchLocationState.Pressed)
                {
                    InputHandler.TouchPressed(tl.Id, tl.Position);
                }
                else if (tl.State == TouchLocationState.Released)
                {
                    InputHandler.TouchReleased(tl.Id, tl.Position);
                }
                else if (tl.State == TouchLocationState.Moved)
                {
                    TouchLocation lastTouch;
                    Vector2 delta = Vector2.Zero;
                    if (tl.TryGetPreviousLocation(out lastTouch))
                    {
                        delta = tl.Position - lastTouch.Position;
                    }
                    InputHandler.TouchMoved(tl.Id, tl.Position, delta);
                }
            }

            _lastTouchCollection = touchCollection;
        }
#endif

        private void UpdateGamePads()
        {
            Dictionary<PlayerIndex, GamePadState> newStateDict = new Dictionary<PlayerIndex, GamePadState>();

            foreach (PlayerIndex index in _lastGamePadStates.Keys)
            {
                GamePadState newState = GamePad.GetState(index);
                GamePadState lastState = _lastGamePadStates[index];

                Vector2 leftStick = newState.ThumbSticks.Left - lastState.ThumbSticks.Left;
                Vector2 rightStick = newState.ThumbSticks.Right - lastState.ThumbSticks.Right;

                Single leftTrigger = newState.Triggers.Left - lastState.Triggers.Left;
                Single rightTrigger = newState.Triggers.Right - lastState.Triggers.Right;

                if (Math.Abs(leftStick.X) > Single.Epsilon || Math.Abs(leftStick.Y) > Single.Epsilon)
                {
                    InputHandler.GamePadThumbstickMoved(index, Thumbstick.Left, newState.ThumbSticks.Left, leftStick);
                }
                if (Math.Abs(rightStick.X) > Single.Epsilon || Math.Abs(rightStick.Y) > Single.Epsilon)
                {
                    InputHandler.GamePadThumbstickMoved(index, Thumbstick.Right, newState.ThumbSticks.Right, rightStick);
                }

                if (Math.Abs(leftTrigger) > Single.Epsilon)
                {
                    InputHandler.GamePadTriggerMoved(index, Trigger.Left, newState.Triggers.Left, leftTrigger);
                }
                if (Math.Abs(rightTrigger) > Single.Epsilon)
                {
                    InputHandler.GamePadTriggerMoved(index, Trigger.Right, newState.Triggers.Right, rightTrigger);
                }

                // below are discrete events mapped to the thumbsticks, triggers, DPad, and buttons.
                GamePadEvent leftStickDiscrete = GetDirection(newState.ThumbSticks.Left, false);
                GamePadEvent oldLeftStickDiscrete = GetDirection(lastState.ThumbSticks.Left, false);
                GamePadEvent rightStickDiscrete = GetDirection(newState.ThumbSticks.Right, true);
                GamePadEvent oldRightStickDiscrete = GetDirection(lastState.ThumbSticks.Right, true);

                GamePadEvent leftTriggerDiscrete = newState.Triggers.Left > 0.9f
                                                       ? GamePadEvent.LeftTrigger
                                                       : GamePadEvent.None;
                GamePadEvent oldLeftTriggerDiscrete = lastState.Triggers.Left > 0.9f
                                                          ? GamePadEvent.LeftTrigger
                                                          : GamePadEvent.None;

                GamePadEvent rightTriggerDiscrete = newState.Triggers.Right > 0.9f
                                                        ? GamePadEvent.RightTrigger
                                                        : GamePadEvent.None;
                GamePadEvent oldRightTriggerDiscrete = lastState.Triggers.Right > 0.9f
                                                           ? GamePadEvent.RightTrigger
                                                           : GamePadEvent.None;

                if (leftTriggerDiscrete != oldLeftTriggerDiscrete)
                {
                    if (leftTriggerDiscrete == GamePadEvent.LeftTrigger)
                        InputHandler.GamePadEventDown(index, GamePadEvent.LeftTrigger);
                    else // oldLeftTriggerDiscrete must == LeftTrigger
                        InputHandler.GamePadEventUp(index, GamePadEvent.LeftTrigger);
                }
                if (rightTriggerDiscrete != oldRightTriggerDiscrete)
                {
                    if (rightTriggerDiscrete == GamePadEvent.RightTrigger)
                        InputHandler.GamePadEventDown(index, GamePadEvent.RightTrigger);
                    else // oldRightTriggerDiscrete must == LeftTrigger
                        InputHandler.GamePadEventUp(index, GamePadEvent.RightTrigger);
                }

                if (leftStickDiscrete != oldLeftStickDiscrete)
                {
                    if (oldLeftStickDiscrete != GamePadEvent.None)
                    {
                        // all "up" events
                        switch (oldLeftStickDiscrete)
                        {
                            case GamePadEvent.LeftStickUp:
                                InputHandler.GamePadEventUp(index, GamePadEvent.LeftStickUp);
                                break;
                            case GamePadEvent.LeftStickDown:
                                InputHandler.GamePadEventUp(index, GamePadEvent.LeftStickDown);
                                break;
                            case GamePadEvent.LeftStickRight:
                                InputHandler.GamePadEventUp(index, GamePadEvent.LeftStickRight);
                                break;
                            case GamePadEvent.LeftStickLeft:
                                InputHandler.GamePadEventUp(index, GamePadEvent.LeftStickLeft);
                                break;
                        }
                    }
                    if (leftStickDiscrete != GamePadEvent.None)
                    {
                        // all "down" events
                        switch (leftStickDiscrete)
                        {
                            case GamePadEvent.LeftStickUp:
                                InputHandler.GamePadEventDown(index, GamePadEvent.LeftStickUp);
                                break;
                            case GamePadEvent.LeftStickDown:
                                InputHandler.GamePadEventDown(index, GamePadEvent.LeftStickDown);
                                break;
                            case GamePadEvent.LeftStickRight:
                                InputHandler.GamePadEventDown(index, GamePadEvent.LeftStickRight);
                                break;
                            case GamePadEvent.LeftStickLeft:
                                InputHandler.GamePadEventDown(index, GamePadEvent.LeftStickLeft);
                                break;
                        }
                    }
                }

                if (rightStickDiscrete != oldRightStickDiscrete)
                {
                    if (oldRightStickDiscrete != GamePadEvent.None)
                    {
                        // all "up" events
                        switch (oldRightStickDiscrete)
                        {
                            case GamePadEvent.RightStickUp:
                                InputHandler.GamePadEventUp(index, GamePadEvent.RightStickUp);
                                break;
                            case GamePadEvent.RightStickDown:
                                InputHandler.GamePadEventUp(index, GamePadEvent.RightStickDown);
                                break;
                            case GamePadEvent.RightStickRight:
                                InputHandler.GamePadEventUp(index, GamePadEvent.RightStickRight);
                                break;
                            case GamePadEvent.RightStickLeft:
                                InputHandler.GamePadEventUp(index, GamePadEvent.RightStickLeft);
                                break;
                        }
                    }
                    if (rightStickDiscrete != GamePadEvent.None)
                    {
                        // all "down" events
                        switch (rightStickDiscrete)
                        {
                            case GamePadEvent.RightStickUp:
                                InputHandler.GamePadEventDown(index, GamePadEvent.RightStickUp);
                                break;
                            case GamePadEvent.RightStickDown:
                                InputHandler.GamePadEventDown(index, GamePadEvent.RightStickDown);
                                break;
                            case GamePadEvent.RightStickRight:
                                InputHandler.GamePadEventDown(index, GamePadEvent.RightStickRight);
                                break;
                            case GamePadEvent.RightStickLeft:
                                InputHandler.GamePadEventDown(index, GamePadEvent.RightStickLeft);
                                break;
                        }
                    }
                }

                if (newState.DPad.Up != lastState.DPad.Up)
                {
                    if (newState.DPad.Up == ButtonState.Pressed)
                        InputHandler.GamePadEventDown(index, GamePadEvent.DPadUp);
                    else
                        InputHandler.GamePadEventUp(index, GamePadEvent.DPadUp);
                }
                if (newState.DPad.Down != lastState.DPad.Down)
                {
                    if (newState.DPad.Down == ButtonState.Pressed)
                        InputHandler.GamePadEventDown(index, GamePadEvent.DPadDown);
                    else
                        InputHandler.GamePadEventUp(index, GamePadEvent.DPadDown);
                }
                if (newState.DPad.Left != lastState.DPad.Left)
                {
                    if (newState.DPad.Left == ButtonState.Pressed)
                        InputHandler.GamePadEventDown(index, GamePadEvent.DPadLeft);
                    else
                        InputHandler.GamePadEventUp(index, GamePadEvent.DPadLeft);
                }
                if (newState.DPad.Right != lastState.DPad.Right)
                {
                    if (newState.DPad.Right == ButtonState.Pressed)
                        InputHandler.GamePadEventDown(index, GamePadEvent.DPadRight);
                    else
                        InputHandler.GamePadEventUp(index, GamePadEvent.DPadRight);
                }

                if (newState.Buttons.A != lastState.Buttons.A)
                {
                    if (newState.Buttons.A == ButtonState.Pressed)
                        InputHandler.GamePadEventDown(index, GamePadEvent.A);
                    else
                        InputHandler.GamePadEventUp(index, GamePadEvent.A);
                }
                if (newState.Buttons.B != lastState.Buttons.B)
                {
                    if (newState.Buttons.B == ButtonState.Pressed)
                        InputHandler.GamePadEventDown(index, GamePadEvent.B);
                    else
                        InputHandler.GamePadEventUp(index, GamePadEvent.B);
                }
                if (newState.Buttons.X != lastState.Buttons.X)
                {
                    if (newState.Buttons.X == ButtonState.Pressed)
                        InputHandler.GamePadEventDown(index, GamePadEvent.X);
                    else
                        InputHandler.GamePadEventUp(index, GamePadEvent.X);
                }
                if (newState.Buttons.Y != lastState.Buttons.Y)
                {
                    if (newState.Buttons.Y == ButtonState.Pressed)
                        InputHandler.GamePadEventDown(index, GamePadEvent.Y);
                    else
                        InputHandler.GamePadEventUp(index, GamePadEvent.Y);
                }

                if (newState.Buttons.Back != lastState.Buttons.Back)
                {
                    if (newState.Buttons.Back == ButtonState.Pressed)
                        InputHandler.GamePadEventDown(index, GamePadEvent.Back);
                    else
                        InputHandler.GamePadEventUp(index, GamePadEvent.Back);
                }
                if (newState.Buttons.Start != lastState.Buttons.Start)
                {
                    if (newState.Buttons.Start == ButtonState.Pressed)
                        InputHandler.GamePadEventDown(index, GamePadEvent.Start);
                    else
                        InputHandler.GamePadEventUp(index, GamePadEvent.Start);
                }

                if (newState.Buttons.LeftShoulder != lastState.Buttons.LeftShoulder)
                {
                    if (newState.Buttons.LeftShoulder == ButtonState.Pressed)
                        InputHandler.GamePadEventDown(index, GamePadEvent.LeftBumper);
                    else
                        InputHandler.GamePadEventUp(index, GamePadEvent.LeftBumper);
                }
                if (newState.Buttons.RightShoulder != lastState.Buttons.RightShoulder)
                {
                    if (newState.Buttons.RightShoulder == ButtonState.Pressed)
                        InputHandler.GamePadEventDown(index, GamePadEvent.RightBumper);
                    else
                        InputHandler.GamePadEventUp(index, GamePadEvent.RightBumper);
                }

                if (newState.Buttons.LeftStick != lastState.Buttons.LeftStick)
                {
                    if (newState.Buttons.LeftStick == ButtonState.Pressed)
                        InputHandler.GamePadEventDown(index, GamePadEvent.LeftStickPressed);
                    else
                        InputHandler.GamePadEventUp(index, GamePadEvent.LeftStickPressed);
                }
                if (newState.Buttons.RightStick != lastState.Buttons.RightStick)
                {
                    if (newState.Buttons.RightStick == ButtonState.Pressed)
                        InputHandler.GamePadEventDown(index, GamePadEvent.RightStickPressed);
                    else
                        InputHandler.GamePadEventUp(index, GamePadEvent.RightStickPressed);
                }

                newStateDict[index] = newState;
            }

            _lastGamePadStates = newStateDict;
        }



        // You could make this user-configurable.
        const float Deadzone = 0.8f;
        const float DiagonalAvoidance = 0.2f;

        private static GamePadEvent GetDirection(Vector2 gamepadThumbStick, Boolean isRightStick)
        {
            // Get the length and prevent something from happening
            // if it's in our deadzone.
            var length = gamepadThumbStick.Length();
            if (length < Deadzone)
                return GamePadEvent.None;

            var absX = Math.Abs(gamepadThumbStick.X);
            var absY = Math.Abs(gamepadThumbStick.Y);
            var absDiff = Math.Abs(absX - absY);

            // We don't like values that are too close to each other
            // i.e. borderline diagonal.
            if (absDiff < length * DiagonalAvoidance)
                return GamePadEvent.None;

            if (absX > absY)
            {
                if (gamepadThumbStick.X > 0)
                    return isRightStick ? GamePadEvent.RightStickRight : GamePadEvent.LeftStickRight;
                else
                    return isRightStick ? GamePadEvent.RightStickLeft : GamePadEvent.LeftStickLeft;
            }
            else
            {
                if (gamepadThumbStick.Y > 0)
                    return isRightStick ? GamePadEvent.RightStickUp : GamePadEvent.LeftStickUp;
                
                else
                    return isRightStick ? GamePadEvent.RightStickDown : GamePadEvent.LeftStickDown;
            }
        }
    }
}
