using System;
using System.Collections.Generic;
using ArchLib.ControlFlow.Screens;

namespace ArchLib.Options
{
    /// <summary>
    /// Passed into Arch.Start() to govern basic behaviors of the game.
    /// </summary>
    public class StartupOptions
    {
        /// <summary>
        /// Invoked to create the initial screen for your Arch game.
        /// </summary>
        public readonly InitialScreenDelegate InitialScreenDelegate;

        /// <summary>
        /// If true, pressing F6 will load Arch's debug overlay.
        /// </summary>
        public Boolean AllowDebugOverlay = Arch.IsDebug;

        public StartupOptions(InitialScreenDelegate initialScreenDelegate)
        {
            InitialScreenDelegate = initialScreenDelegate;
        }
    }


    public delegate Screen InitialScreenDelegate();
}
