﻿using System;
using System.Collections.Generic;
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
        /// <summary>
        /// The input handler for your game. Defaults to Arch.Screens.
        /// </summary>
        public static IInputHandler InputHandler { get; set; }
        /// <summary>
        /// Handles computations for the scaling/resolution independence system.
        /// </summary>
        public static Scaling Scaling { get; private set; }


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

            Screens = new ScreenManager();
            InputHandler = Screens;

        }

        internal static void FireBeforeStart()
        {
            Scaling.BeforeStart();
            Screens.BeforeStart();
            if (BeforeStart != null) BeforeStart();
        }
    }

    public delegate void InitializationDelegate();

    public delegate void PreStartDelegate();
}
