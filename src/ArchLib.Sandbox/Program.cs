using System;
using ArchLib.ControlFlow.Screens;
using ArchLib.Options;
using ArchLib.Sandbox.Screens;
using Microsoft.Xna.Framework;

namespace ArchLib.Sandbox
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var options = new StartupOptions(FirstScreen)
                              {
                                  WindowWidth = 1282,
                                  WindowHeight = 900,
                                  LetterboxColor = new Color(0.1f, 0.1f, 0.1f),
                                  ShowMouseCursor = true
                              };

            Arch.Start(options);
        }


        static Screen FirstScreen()
        {
            return new TestScreen();
        }
    }
#endif
}

