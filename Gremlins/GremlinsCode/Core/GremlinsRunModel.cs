using System.Runtime.CompilerServices;
using BaseLib.Abstracts;
using Downfall.DownfallCode.Saves;
using Gremlins.GremlinsCode.Vfx;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace Gremlins.GremlinsCode.Core;

public class GremlinsRunModel() : CustomSingletonModel(false, true)
{
    public static readonly CustomMonsterModel[] StartingGremlins =
    [
        ModelDb.Monster<ShieldGremlin>(),
        ModelDb.Monster<MadGremlin>(),
        ModelDb.Monster<FatGremlin>(),
        ModelDb.Monster<SneakGremlin>(),
        ModelDb.Monster<WizardGremlin>()
    ];

    private static readonly ConditionalWeakTable<Player, GremlinState> States = new();

    public static GremlinState GetState(Player player)
    {
        if (States.TryGetValue(player, out var state)) return state;
        state = new GremlinState();
        States.Add(player, state);
        return state;
    }


    public override Task AfterActEntered()
    {
        var runState = RunManager.Instance.DebugOnlyGetState();
        if (runState is not { ActFloor: 1 }) return Task.CompletedTask;
        foreach (var player in runState.Players)
        {
            var saveData = DownfallSaveManager.GetPlayerData(player);
            if (player.Character is Gremlins)
            {
                saveData.GremlinStats = StartingGremlins.Select(m => new GremlinSaveData
                {
                    ModelId = m.Id,
                    Hp      = m.MaxInitialHp,
                    MaxHp   = m.MaxInitialHp,
                }).ToList();
            }
            else
            {
                saveData.GremlinStats = [];
            }
        }

        return Task.CompletedTask;
    }

    public override Task BeforeCombatStart()
    {
        var combatState = CombatManager.Instance.DebugOnlyGetState();
        if (combatState == null) return Task.CompletedTask;

        foreach (var player in combatState.Players)
        {
            if (player.Character is not Gremlins) continue;
            if (player.PlayerCombatState == null) continue;

            var state    = GetState(player);
            var saveData = DownfallSaveManager.GetPlayerData(player);
            state.Reset();

            foreach (var saved in saveData.GremlinStats)
            {
                var model = ModelDb.GetById<MonsterModel>(saved.ModelId);
                GremlinsCmd.AddGremlin(player, model, saved.Hp, saved.MaxHp);
            }

            var creatureNode = NCombatRoom.Instance?.GetCreatureNode(player.Creature);
            if (creatureNode?.Visuals is NGremlinsCreatureVisuals visuals)
                visuals.ArrangeGremlins(state.Gremlins);
            
            var active = state.Active;
            if (active == null) continue;
            player.Creature.SetMaxHpInternal(active.MaxHp);
            player.Creature.SetCurrentHpInternal(active.CurrentHp);
        }
        
        

        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        var combatState = CombatManager.Instance.DebugOnlyGetState();
        if (combatState == null) return base.AfterCombatEnd(room);

        foreach (var player in combatState.Players)
        {
            if (player.Character is not Gremlins) continue;
            var state    = GetState(player);
            var saveData = DownfallSaveManager.GetPlayerData(player);
            saveData.GremlinStats = state.ToSaveData(player);
            state.Reset();
        }
        return Task.CompletedTask;
    }
    
}