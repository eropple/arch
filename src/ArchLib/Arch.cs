using System;
using System.Collections.Generic;
using ArchLib.Content;
using ArchLib.ControlFlow;
using ArchLib.Graphics;
using ArchLib.Input;
using ArchLib.Options;
using ArchLib.Runners;
using ArchLib.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchLib
{
    public static class Arch
    {
#if DEBUG
        public static readonly Boolean IsDebug = true;
#else
        public static readonly Boolean IsDebug = false;
#endif
#if TRACE
        public static readonly Boolean IsTrace = true;
#else
        public static readonly Boolean IsTrace = false;
#endif

        /// <summary>
        /// Called after all Arch systems are initialized. It is safe to use any methods
        /// on the Arch static class during the methods invoked by OnInitialization.
        /// </summary>
        public static event InitializationDelegate OnInitialization;
        /// <summary>
        /// Called after all initialization is done. Analogous to BeginRun in XNA.
        /// </summary>
        public static event PreStartDelegate BeforeStart;

        /// <summary>
        /// The underlying Microsoft.Xna.Framework.Game being used by Arch.
        /// </summary>
        public static XnaGame XnaGame { get; private set; }
        /// <summary>
        /// The current game's GraphicsDeviceManager.
        /// </summary>
        public static GraphicsDeviceManager Graphics { get; private set; }
        /// <summary>
        /// The control flow manager for Arch.
        /// </summary>
        public static ScreenManager Screens { get; private set; }

        public static InputSystem Input { get; private set; }
        /// <summary>
        /// Handles computations for the scaling/resolution independence system.
        /// </summary>
        public static Scaling Scaling { get; private set; }

        public static ContentContext GlobalContent { get; private set; }


        /// <summary>
        /// Helper class to make creating some objects simpler.
        /// </summary>
        public static Factory Factory { get; private set; }



        internal static StartupOptions Options { get; private set; }

        public static void Start(StartupOptions options)
        {
            Options = options;

            XnaGame g = new XnaGame(options);

            Initialize(g);
            if (OnInitialization != null) OnInitialization();

            using (g)
            {
                g.Run();
            }
        }

        private static void Initialize(XnaGame game)
        {
            XnaGame = game;
            Graphics = game.GraphicsDeviceManager;
            Scaling = new Scaling(Graphics);
            Factory = new Factory();
            Input = new InputSystem();

            Screens = new ScreenManager();
            GlobalContent = new ContentContext(null);

        }

        internal static void FireBeforeStart()
        {
            Scaling.BeforeStart();
            Screens.BeforeStart();
            Input.InputHandler = Screens;
            if (BeforeStart != null) BeforeStart();
        }


        internal static void Update(Double delta)
        {
            Input.Update();
            Screens.Update(delta);
        }

        internal static void Draw(Double delta)
        {
            Screens.Draw(delta);
        }
    }

    public delegate void InitializationDelegate();

    public delegate void PreStartDelegate();
}
