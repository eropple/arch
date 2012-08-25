using System;
using System.Collections.Generic;
using ArchLib.ControlFlow.Screens;
using ArchLib.Input;
using ArchLib.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArchLib.ControlFlow
{
    public sealed class ScreenManager : IInputHandler
    {
        private readonly List<Screen> _screens = new List<Screen>(50); // should never hit that capacity!

        private Boolean _skipDraw;
        private SpriteBatch _batch;

        public ScreenManager()
        {
            _batch = Arch.Factory.BuildSpriteBatch();

            this.Push(Arch.Options.InitialScreenDelegate());
        }

        public void Update(Double delta)
        {
            _skipDraw = false;

            Int32 count = _screens.Count;
            Int32 last = count - 1;

            Screen topScreen = _screens[last];

            for (Int32 i = last; i >= 0; --i)
            {
                Screen s = _screens[i];
                if (s == null) continue;

                s.Update(delta, i == last);
                if (s.ShouldSoakUpdates) break;
            }
        }
        
        public void Draw(Double delta)
        {
            if (_skipDraw) return;

            Int32 count = _screens.Count;
            Int32 last = count - 1;

            Int32 drawSoak = 0;

            if (count < 1) return;

            for (Int32 i = last; i >= 0; --i)
            {
                Screen s = _screens[i];
                if (s == null) continue;

                if (s.ShouldSoakDraws)
                {
                    drawSoak = i;
                    break;
                }
            }

            if (drawSoak < 0) drawSoak = 0;

            for (Int32 i = drawSoak; i < count; ++i)
            {
                Screen s = _screens[i];
                if (s == null) continue;

                _batch.BeginScaled();
                s.Draw(delta, _batch, i == last);
                _batch.End();
            }

//            if (Core.Settings.ShowOverlay)
//            {
//                BitmapFont font = Core.GetOverlayFont();
//
//                _batch.BeginScaled();
//                String text = String.Format("# Screens: {0}   Top: {1}", _screens.Count, _screens[_screens.Count - 1]);
//                font.DrawString(_batch, text, 5, 0 + font.MeasureStringHeight(), Color.White);
//                _batch.End();
//            }
        }


        public void SkipDrawThisFrame()
        {
            _skipDraw = true;
        }

        public Int32 Count { get { return _screens.Count; } }


        
        public void Push(Screen newScreen)
        {
            Int32 last = _screens.Count - 1;

            newScreen.DoLoadContent();
            _screens.Add(newScreen);

            if (last >= 0) // has any states
            {
                _screens[last].LostFocus(newScreen);
            }

            _skipDraw = true;
        }
        
        public void Pop()
        {
            Int32 last = _screens.Count - 1;
            Screen oldScreen = _screens[last];
            _screens.RemoveAt(last);

            if (_screens.Count > 0)
            {
                Int32 newLast = _screens.Count - 1;
                Screen newScreen = _screens[newLast];

                newScreen.GotFocus(oldScreen);
                oldScreen.Dispose();
                _skipDraw = true;
            }
            else
            {
                Environment.Exit(0);
            }
        }





        
        public Boolean KeyDown(Keys k, Boolean control, Boolean shift, Boolean alt)
        {
            Int32 count = _screens.Count;
            Int32 last = count - 1;

            for (Int32 i = last; i >= 0; --i)
            {
                Screen s = _screens[i];
                if (s == null) continue;

                if (s.KeyDown(k, control, shift, alt)) return true;
                if (s.ShouldSoakInput) break;
            }
            return false;
        }

        public Boolean KeyUp(Keys k)
        {
            Int32 count = _screens.Count;
            Int32 last = count - 1;

            for (Int32 i = last; i >= 0; --i)
            {
                Screen s = _screens[i];
                if (s == null) continue;

                if (s.KeyUp(k)) return true;
                if (s.ShouldSoakInput) break;
            }
            return false;
        }

        public Boolean MouseButtonPressed(MouseButton button, Vector2 position, Boolean control, Boolean shift, Boolean alt)
        {
            Int32 count = _screens.Count;
            Int32 last = count - 1;

            for (Int32 i = last; i >= 0; --i)
            {
                Screen s = _screens[i];
                if (s == null) continue;

                if (s.MouseButtonPressed(button, position, control, shift, alt)) return true;
                if (s.ShouldSoakInput) break;
            }
            return false;
        }

        public Boolean MouseButtonReleased(MouseButton button, Vector2 position)
        {
            Int32 count = _screens.Count;
            Int32 last = count - 1;

            for (Int32 i = last; i >= 0; --i)
            {
                Screen s = _screens[i];
                if (s == null) continue;

                if (s.MouseButtonReleased(button, position)) return true;
                if (s.ShouldSoakInput) break;
            }
            return false;
        }

        public Boolean MouseScrollWheel(Int32 scrollDirection, Vector2 position)
        {
            Int32 count = _screens.Count;
            Int32 last = count - 1;

            for (Int32 i = last; i >= 0; --i)
            {
                Screen s = _screens[i];
                if (s == null) continue;

                if (s.MouseScrollWheel(scrollDirection, position)) return true;
                if (s.ShouldSoakInput) break;
            }
            return false;
        }

        public Boolean MouseMoved(Vector2 position, Vector2 delta)
        {
            Int32 count = _screens.Count;
            Int32 last = count - 1;

            for (Int32 i = last; i >= 0; --i)
            {
                Screen s = _screens[i];
                if (s == null) continue;

                if (s.MouseMoved(position, delta)) return true;
                if (s.ShouldSoakInput) break;
            }
            return false;
        }

        public Boolean TouchPressed(Int32 id, Vector2 position)
        {
            Int32 count = _screens.Count;
            Int32 last = count - 1;

            for (Int32 i = last; i >= 0; --i)
            {
                Screen s = _screens[i];
                if (s == null) continue;

                if (s.TouchPressed(id, position)) return true;
                if (s.ShouldSoakInput) break;
            }
            return false;
        }

        public Boolean TouchMoved(Int32 id, Vector2 position, Vector2 delta)
        {
            Int32 count = _screens.Count;
            Int32 last = count - 1;

            for (Int32 i = last; i >= 0; --i)
            {
                Screen s = _screens[i];
                if (s == null) continue;

                if (s.TouchMoved(id, position, delta)) return true;
                if (s.ShouldSoakInput) break;
            }
            return false;
        }

        public Boolean TouchReleased(Int32 id, Vector2 position)
        {
            Int32 count = _screens.Count;
            Int32 last = count - 1;

            for (Int32 i = last; i >= 0; --i)
            {
                Screen s = _screens[i];
                if (s == null) continue;

                if (s.TouchReleased(id, position)) return true;
                if (s.ShouldSoakInput) break;
            }
            return false;
        }

        public Boolean GamePadThumbstickMoved(PlayerIndex index, Thumbstick stick, Vector2 position, Vector2 delta)
        {
            Int32 count = _screens.Count;
            Int32 last = count - 1;

            for (Int32 i = last; i >= 0; --i)
            {
                Screen s = _screens[i];
                if (s == null) continue;

                if (s.GamePadThumbstickMoved(index, stick, position, delta)) return true;
                if (s.ShouldSoakInput) break;
            }
            return false;
        }

        public Boolean GamePadTriggerMoved(PlayerIndex index, Trigger trigger, Single position, Single delta)
        {
            Int32 count = _screens.Count;
            Int32 last = count - 1;

            for (Int32 i = last; i >= 0; --i)
            {
                Screen s = _screens[i];
                if (s == null) continue;

                if (s.GamePadTriggerMoved(index, trigger, position, delta)) return true;
                if (s.ShouldSoakInput) break;
            }
            return false;
        }

        public Boolean GamePadEventDown(PlayerIndex index, GamePadEvent eventType)
        {
            Int32 count = _screens.Count;
            Int32 last = count - 1;

            for (Int32 i = last; i >= 0; --i)
            {
                Screen s = _screens[i];
                if (s == null) continue;

                if (s.GamePadEventDown(index, eventType)) return true;
                if (s.ShouldSoakInput) break;
            }
            return false;
        }

        public Boolean GamePadEventUp(PlayerIndex index, GamePadEvent eventType)
        {
            Int32 count = _screens.Count;
            Int32 last = count - 1;

            for (Int32 i = last; i >= 0; --i)
            {
                Screen s = _screens[i];
                if (s == null) continue;

                if (s.GamePadEventUp(index, eventType)) return true;
                if (s.ShouldSoakInput) break;
            }
            return false;
        }
    }
}
