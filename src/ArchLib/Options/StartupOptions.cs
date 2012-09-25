using System;
using System.Collections.Generic;
using ArchLib.Content.Factories;
using ArchLib.ControlFlow.Screens;
using Microsoft.Xna.Framework;

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
        /// Width of your application window or full-screen resolution.
        /// </summary>
        public Int32 WindowWidth = 1280;
        /// <summary>
        /// Height of your application window or full-screen resolution.
        /// </summary>
        public Int32 WindowHeight = 720;
        /// <summary>
        /// If true, application opens in full-screen.
        /// </summary>
        public Boolean Fullscreen = false;
        /// <summary>
        /// Color to use to render any letterboxing. Useful for debugging.
        /// </summary>
        public Color LetterboxColor = Color.Black;

        /// <summary>
        /// Forces a scale factor for the game's assets. If set to 0, no
        /// scaling is performed. If set to 1, will always use @1x assets if they
        /// exist. If set to 2, will always use @2x assets if they exist. This
        /// overrides the standard selector based on real viewport size.
        /// </summary>
        public Int32 ForceScaleFactor = 0;

        
        /// <summary>
        /// Specify the file path (relative or absolute) from which to pull assets.
        /// </summary>
        public String ContentRoot = "Assets";

        /// <summary>
        /// If set to true, the OS mouse cursor is shown.
        /// </summary>
        public Boolean ShowMouseCursor = false;

        /// <summary>
        /// If true, pressing F6 will load Arch's debug overlay.
        /// </summary>
        public Boolean AllowDebugOverlay = false;

        /// <summary>
        /// The content context factory to use to populate GlobalContext and each screen's
        /// LocalContext. Leave null for standard ContentContexts.
        /// </summary>
        public ContentContextFactoryDelegate ContentContextFactoryDelegate = null;

        public StartupOptions(InitialScreenDelegate initialScreenDelegate)
        {
            InitialScreenDelegate = initialScreenDelegate;
        }
    }


    public delegate Screen InitialScreenDelegate();

    public delegate IContentContextFactory ContentContextFactoryDelegate();
}
