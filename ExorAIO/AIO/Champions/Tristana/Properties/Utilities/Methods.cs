using LeagueSharp;

namespace ExorAIO.Champions.Tristana
{
    /// <summary>
    ///     The methods class.
    /// </summary>
    internal class Methods
    {
        /// <summary>
        ///     Sets the methods.
        /// </summary>
        public static void Initialize()
        {
            Game.OnUpdate += Tristana.OnUpdate;
        }
    }
}