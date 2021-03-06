using ExorAIO.Utilities;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.UI;
using LeagueSharp.SDK.Utils;

namespace ExorAIO.Champions.Vayne
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Logics
    {
        /// <summary>
        ///     Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The args.</param>
        public static void Weaving(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!(args.Target is Obj_AI_Hero) ||
                Invulnerable.Check(args.Target as Obj_AI_Hero))
            {
                return;
            }

            /// <summary>
            ///     The Q Weaving Logic.
            /// </summary>
            if (Vars.Q.IsReady() &&
                Vars.Menu["spells"]["q"]["combo"].GetValue<MenuBool>().Value)
            {
                if (Vars.Menu["miscellaneous"]["wstacks"].GetValue<MenuBool>().Value)
                {
                    if ((args.Target as Obj_AI_Hero).GetBuffCount("vaynesilvereddebuff") != 1)
                    {
                        return;
                    }
                }

                if (!Vars.Menu["miscellaneous"]["alwaysq"].GetValue<MenuBool>().Value)
                {
                    if (GameObjects.Player.Distance(Game.CursorPos) > Vars.AARange &&
                        (args.Target as Obj_AI_Hero).Distance(
                            GameObjects.Player.Position.Extend(Game.CursorPos, Vars.Q.Range - Vars.AARange)) < Vars.AARange)
                    {
                        Vars.Q.Cast(Game.CursorPos);
                    }
                }
                else
                {
                    Vars.Q.Cast(Game.CursorPos);
                }
            }
        }
    }
}