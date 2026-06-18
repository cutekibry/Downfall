using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Gremlins.GremlinsCode.Relics;

[Pool(typeof(GremlinsRelicPool))]
public class MobLeadersStaff : GremlinsRelicModel
{
    public MobLeadersStaff() : base(RelicRarity.Starter)
    {
        WithEnergy(1);
        WithCards(1);
    }
    
    
    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<MobLeadersCrown>();
    }
    
    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        return player != Owner || Owner.PlayerCombatState == null || Owner.PlayerCombatState.TurnNumber > 1 ? count : count + DynamicVars.Cards.BaseValue;
    }
    
    public override async Task AfterEnergyReset(Player player)
    {
        if (Owner != player || Owner.PlayerCombatState is not { TurnNumber: 1 }) return;
        Flash();
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext ctx, ICombatState combatState)
    {
        if (player != Owner || Owner.PlayerCombatState is not { TurnNumber: 1 }) return;
        Flash();
        await GremlinsCmd.SwapToNext(ctx, player);
    }
}