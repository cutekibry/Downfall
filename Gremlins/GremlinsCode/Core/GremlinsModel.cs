using BaseLib.Abstracts;
using Gremlins.GremlinsCode.Powers;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gremlins.GremlinsCode.Core;

public class GremlinsModel() : CustomSingletonModel(true, false)
{
    
    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext ctx, ICombatState combatState)
    {
        if (player.Character is not Gremlins || combatState.RoundNumber > 1) return;
        await PowerCmd.Apply<GremlinPower>(ctx, player.Creature, 1, player.Creature, null, true);
    }

    
    
    public override Task AfterDamageReceived(
        PlayerChoiceContext ctx, Creature target, DamageResult result,
        ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        var player = target.Player;
        if (player?.Character is not Gremlins) return Task.CompletedTask;

        var state = GremlinsRunModel.GetState(player);
        if (state.Active == null) return Task.CompletedTask;

        state.SaveHp(target.CurrentHp);
        state.Active.SetMaxHpInternal(target.MaxHp);
        state.Active.SetCurrentHpInternal(target.CurrentHp);
        return Task.CompletedTask;
    }
}

[HarmonyPatch(typeof(CreatureCmd), nameof(CreatureCmd.KillWithoutCheckingWinCondition))]
public static class PatchGremlinDeath
{
    static bool Prefix(Creature creature, bool force, ref Task __result)
    {
        if (force) return true;
        var player = creature.Player;
        if (player?.Character is not Gremlins) return true;

        var state = GremlinsRunModel.GetState(player);
        var dying = state.Active;
        if (dying == null) return true;
        state.Kill(dying);
        var next = state.Active;
        if (next == null) return true;
        __result = RunAsync(player, state, dying, next);
        return false;
    }

    static async Task RunAsync(Player player, GremlinState state, Creature dying, Creature next)
    {
        if (player.Creature.CombatState == null || dying.Monster == null) return;
        var hookCtx = new HookPlayerChoiceContext(dying.Monster, player.NetId, player.Creature.CombatState, GameActionType.Combat);
        await Cmd.Wait(1);
        var task = GremlinsCmd.ApplySwap(hookCtx, player, state, next, GremlinSwapType.Death);
        await hookCtx.AssignTaskAndWaitForPauseOrCompletion(task);
        dying.Monster.InvokeExecutionFinished();
        GremlinsCmd.KillGremlin(player.Creature, dying);
    }
}