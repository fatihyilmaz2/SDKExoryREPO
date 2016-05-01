using System;
using System.Linq;
using ExorAIO.Utilities;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.UI;

namespace ExorAIO.Champions.Ezreal
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Logics
    {
        /// <summary>
        ///     Called when the game updates itself.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public static void Harass(EventArgs args)
        {
            if (!Targets.Target.IsValidTarget() ||
                Bools.HasAnyImmunity(Targets.Target))
            {
                return;
            }

            /// <summary>
            ///     The Q Harass Logic.
            /// </summary>
            if (Vars.Q.IsReady() &&
                Targets.Target.IsValidTarget(Vars.Q.Range) &&
                GameObjects.Player.ManaPercent > ManaManager.NeededQMana &&
                Vars.Menu["spells"]["q"]["harass"].GetValue<MenuBool>().Value)
            {
                if (!Vars.Q.GetPrediction(Targets.Target).CollisionObjects.Any(c => c is Obj_AI_Minion))
                {
                    Vars.Q.Cast(Vars.Q.GetPrediction(Targets.Target).UnitPosition);
                    return;
                }
            }

            /// <summary>
            ///     The W Harass Logic.
            /// </summary>
            if (Vars.W.IsReady() &&
                Targets.Target.IsValidTarget(Vars.W.Range) &&
                GameObjects.Player.ManaPercent > ManaManager.NeededWMana &&
                Vars.Menu["spells"]["w"]["harass"].GetValue<MenuBool>().Value)
            {
                Vars.W.Cast(Vars.W.GetPrediction(Targets.Target).CastPosition);
            }
        }
    }
}