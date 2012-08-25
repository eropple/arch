using System;
using ArchLib.ControlFlow.Screens;
using ArchLib.Options;

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
            StartupOptions options = new StartupOptions(FirstScreen);

            Arch.Start(options);
        }


        static Screen FirstScreen()
        {
            return null;
        }
    }
#endif
}

