using System;
using System.Linq;
using ExorAIO.Utilities;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.UI;
using LeagueSharp.SDK.Utils;

namespace ExorAIO.Champions.Quinn
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
        public static void Killsteal(EventArgs args)
        {
            /// <summary>
            ///     The KillSteal Q Logic.
            /// </summary>
            if (Vars.Q.IsReady() &&
                Vars.Menu["spells"]["q"]["killsteal"].GetValue<MenuBool>().Value)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(
                    t =>
                        !Invulnerable.Check(t) &&
                        !t.IsValidTarget(Vars.AARange) &&
                        t.IsValidTarget(Vars.Q.Range - 100f) &&
                        Vars.GetRealHealth(t) <
                            (float)GameObjects.Player.GetSpellDamage(t, SpellSlot.Q)))
                {
                    if (!Vars.Q.GetPrediction(Targets.Target).CollisionObjects.Any(c => Targets.Minions.Contains(c)))
                    {
                        Vars.Q.Cast(Vars.Q.GetPrediction(Targets.Target).UnitPosition);
                    }
                }
            }

            /// <summary>
            ///     The KillSteal E Logic.
            /// </summary>
            if (Vars.E.IsReady() &&
                Vars.Menu["spells"]["e"]["killsteal"].GetValue<MenuBool>().Value)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(
                    t =>
                        !Invulnerable.Check(t) &&
                        t.IsValidTarget(Vars.E.Range) &&
                        !t.IsValidTarget(Vars.AARange) &&
                        Vars.GetRealHealth(t) <
                            GameObjects.Player.GetAutoAttackDamage(t)*2 +
                            (float)GameObjects.Player.GetSpellDamage(t, SpellSlot.E)))
                {
                    Vars.E.CastOnUnit(target);
                }
            }
        }
    }
}