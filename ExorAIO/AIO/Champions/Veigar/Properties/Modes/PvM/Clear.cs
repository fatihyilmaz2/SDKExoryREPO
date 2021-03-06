using System;
using System.Linq;
using ExorAIO.Utilities;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.UI;

namespace ExorAIO.Champions.Veigar
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Logics
    {
        /// <summary>
        ///     Fired when the game is updated.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public static void Clear(EventArgs args)
        {
            if (Bools.HasSheenBuff())
            {
                return;
            }

            /// <summary>
            ///     The Q Clear Logic.
            /// </summary>
            if (Vars.Q.IsReady() &&
                GameObjects.Player.ManaPercent >
                    ManaManager.GetNeededMana(Vars.Q.Slot, Vars.Menu["spells"]["q"]["clear"]) &&
                Vars.Menu["spells"]["q"]["clear"].GetValue<MenuSliderButton>().BValue)
            {
                if (Targets.Minions.Any())
                {
                    if (Vars.Q.GetLineFarmLocation(Targets.Minions.Where(
                        m =>
                            Vars.GetRealHealth(m) <
                                (float)GameObjects.Player.GetSpellDamage(m, SpellSlot.Q)).ToList(), Vars.Q.Width).MinionsHit == 2)
                    {
                        Vars.Q.Cast(Vars.Q.GetLineFarmLocation(Targets.Minions.Where(
                            m =>
                                Vars.GetRealHealth(m) <
                                    (float)GameObjects.Player.GetSpellDamage(m, SpellSlot.Q)).ToList(), Vars.Q.Width).Position);
                    }
                }
                else if (Targets.JungleMinions.Any(m => m.Health <
                    (float)GameObjects.Player.GetSpellDamage(m, SpellSlot.Q)))
                {
                    Vars.Q.Cast(Targets.JungleMinions.FirstOrDefault(
                        m =>
                            m.Health < (float)GameObjects.Player.GetSpellDamage(m, SpellSlot.Q)).ServerPosition);
                }
            }

            /// <summary>
            ///     The W Clear Logics.
            /// </summary>
            if (Vars.W.IsReady())
            {
                /// <summary>
                ///     The W LaneClear Logic.
                /// </summary>
                if (Targets.Minions.Any())
                {
                    if (GameObjects.Player.ManaPercent >
                            ManaManager.GetNeededMana(Vars.W.Slot, Vars.Menu["spells"]["w"]["laneclear"]) &&
                        Vars.Menu["spells"]["w"]["laneclear"].GetValue<MenuSliderButton>().BValue)
                    {
                        if (Vars.W.GetCircularFarmLocation(Targets.Minions, Vars.W.Width).MinionsHit >=
                                Vars.Menu["spells"]["w"]["minionshit"].GetValue<MenuSlider>().Value)
                        {
                            Vars.W.Cast(Vars.W.GetCircularFarmLocation(Targets.Minions, Vars.W.Width).Position);
                        }
                    }
                }

                /// <summary>
                ///     The W JungleClear Logic.
                /// </summary>
                else if (Targets.JungleMinions.Any())
                {
                    if (GameObjects.Player.ManaPercent >
                            ManaManager.GetNeededMana(Vars.W.Slot, Vars.Menu["spells"]["w"]["jungleclear"]) &&
                        Vars.Menu["spells"]["w"]["jungleclear"].GetValue<MenuSliderButton>().BValue)
                    {
                        if (!Targets.JungleMinions.Any(m => m.Health < (float)GameObjects.Player.GetSpellDamage(m, SpellSlot.W)))
                        {
                            Vars.W.Cast(Targets.JungleMinions[0].ServerPosition);
                        }
                    }
                }
            }
        }
    }
}