using System;
using System.Linq;
using ExorAIO.Utilities;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.UI;
using LeagueSharp.SDK.Utils;

namespace ExorAIO.Champions.Sivir
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
        public static void Automatic(EventArgs args)
        {
            if (Bools.HasSheenBuff())
            {
                return;
            }

            /// <summary>
            ///     The Automatic Q Logic.
            /// </summary>
            if (Vars.Q.IsReady() &&
                Vars.Menu["spells"]["q"]["logical"].GetValue<MenuBool>().Value)
            {
                foreach (var target in GameObjects.EnemyHeroes.Where(
                    t =>
                        Bools.IsImmobile(t) &&
                        !Invulnerable.Check(t) &&
                        t.IsValidTarget(Vars.Q.Range)))
                {
                    Vars.Q.Cast(target.ServerPosition);
                }
            }
        }

        /// <summary>
        ///     Called while processing Spellcasting operations.
        ///     Port this berbb :^)
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="GameObjectProcessSpellCastEventArgs" /> instance containing the event data.</param>
        public static void AutoShield(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (Invulnerable.Check(GameObjects.Player, DamageType.True, false))
            {
                return;
            }

            if (sender.IsMe ||
                sender == null ||
                !sender.IsValid)
            {
                return;
            }

            /// <summary>
            ///     Special check for Kalista's E.
            /// </summary>
            if (args.SData.Name.Equals("KalistaExpungeWrapper"))
            {
                if (!ObjectManager.Player.HasBuff("KalistaExpungeMarker"))
                {
                    return;
                }
            }
            else
            {
                if (args.Target == null)
                {
                    return;
                }

                if (args.SData.Name.Equals("HextechGunblade") ||
                    args.SData.Name.Equals("BilgewaterCutlass") ||
                    args.SData.Name.Equals("ItemSwordOfFeastAndFamine"))
                {
                    return;
                }

                /// <summary>
                ///     Block Gangplank's Barrels.
                /// </summary>
                if ((sender as Obj_AI_Hero) != null &&
                    (sender as Obj_AI_Hero).ChampionName.Equals("Gangplank"))
				{
					if (AutoAttack.IsAutoAttack(args.SData.Name) ||
						args.SData.Name.Equals("GangplankQProceed"))
					{
						if ((args.Target as Obj_AI_Minion).Health == 1 &&
							(args.Target as Obj_AI_Minion).CharData.BaseSkinName.Equals("gangplankbarrel"))
						{
							if (GameObjects.Player.Distance(args.Target) < 450)
							{
								Vars.E.Cast();
							}
						}
					}
					else if (args.SData.Name.Equals("GangplankEBarrelFuseMissile"))
					{
						if (GameObjects.Player.Distance(args.End) < 450)
						{
							Vars.E.Cast();
						}
					}
				}

                if (!args.Target.IsMe)
                {
                    return;
                }

                /// <summary>
                ///     Block Dragon's AutoAttacks.
                /// </summary>
                if (sender is Obj_AI_Minion)
                {
                    if (!sender.CharData.BaseSkinName.Equals("SRU_Baron") &&
                        !sender.CharData.BaseSkinName.Contains("SRU_Dragon") &&
						!sender.CharData.BaseSkinName.Equals("SRU_RiftHerald"))
                    {
                        return;
                    }
                }

                if (!sender.IsEnemy ||
                    !(sender as Obj_AI_Hero).IsValidTarget())
                {
                    return;
                }

                /// <summary>
                ///     Special check for the AutoAttacks.
                /// </summary>
                if (AutoAttack.IsAutoAttack(args.SData.Name))
                {
					if (!sender.IsMelee)
					{
						if (!args.SData.Name.Contains("Card"))
						{
							return;
						}
					}
					else
					{
						if (sender.Buffs.Any(b => AutoAttack.IsAutoAttackReset(args.SData.Name)))
						{
							Vars.E.Cast();
						}
					}
                }

                /// <summary>
                ///     Special check for the Located AoE skillshots.
                /// </summary>
                if (args.SData.TargettingType.Equals(SpellDataTargetType.LocationAoe))
                {
                    if (args.SData.Name.Equals("GangplankE") ||
                        args.SData.Name.Equals("TrundleCircle") ||
                        args.SData.Name.Equals("TormentedSoil") ||
                        args.SData.Name.Equals("SwainDecrepify") ||
                        args.SData.Name.Equals("MissFortuneScattershot"))
                    {
                        return;
                    }
                }

                /// <summary>
                ///     Special check for the on target-position AoE spells.
                /// </summary>
                if (args.SData.TargettingType.Equals(SpellDataTargetType.SelfAoe))
                {
                    if (!args.SData.Name.Equals("MockingShout"))
                    {
                        return;
                    }
                }
            }

            if (args.Target.IsMe &&
                args.SData.TargettingType.Equals(SpellDataTargetType.Self))
            {
                /// <summary>
                ///     If the sender is Zed and the processed arg is a Targetted spell (His Ultimate), delay the shieldcasting by 200ms.
                /// </summary>
                DelayAction.Add(
                    sender.CharData.BaseSkinName.Equals("Zed")
                        ? 200
                        : sender.CharData.BaseSkinName.Equals("Caitlyn")
                            ? 1000
                            : sender.CharData.BaseSkinName.Equals("Nocturne") &&
                              args.SData.Name.Equals("NocturneUnspeakableHorror")
                                ? 500
                                : Vars.Menu["spells"]["e"]["delay"].GetValue<MenuSlider>().Value,
                () =>
					{
						Vars.E.Cast();
					}
				);
            }
            else
            {
                DelayAction.Add(
					Vars.Menu["spells"]["e"]["delay"].GetValue<MenuSlider>().Value,
				() =>
					{
						Vars.E.Cast();
					}
				);
            }
        }
    }
}