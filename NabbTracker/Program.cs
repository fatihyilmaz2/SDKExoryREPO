﻿using LeagueSharp.SDK;

namespace NabbTracker
{
    /// <summary>
    ///     The application class.
    /// </summary>
    internal class Program
    {
        /// <summary>
        ///     The entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            /// <summary>
            ///     Loads the Bootstrap.
            /// </summary>
            Bootstrap.Init();

            Events.OnLoad += (sender, eventArgs) =>
            {
                /// <summary>
                ///     Loads the Update checker.
                /// </summary>
                //Updater.Check();

				/// <summary>
				///     Loads the assembly.
				/// </summary>
				Tracker.OnLoad();
            };
        }
    }
}