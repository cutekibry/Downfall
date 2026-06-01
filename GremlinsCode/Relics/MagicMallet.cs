using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

namespace Gremlins.GremlinsCode.Relics;

[Pool(typeof(GremlinsRelicPool))]
public class MagicMallet : GremlinsRelicModel
{
    private int _usesLeft;
    public override bool ShowCounter => CombatManager.Instance.IsInProgress;

    
    public override int DisplayAmount => 3;

    public MagicMallet() : base(RelicRarity.Uncommon)
    {
        WithPower<WizPower>(1);
        WithTip<WeakPower>();
        WithVar("MaxUses", 3);
    }
    
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext ctx, PowerModel power, decimal amount, Creature? applier,
        CardModel? cardSource)
    {
        if (applier != Owner.Creature || power is not WeakPower || _usesLeft <= 0) return;
        _usesLeft--;
        InvokeDisplayAmountChanged();
        await MyCommonActions.ApplySelf<WizPower>(ctx, this);
    }
    
    public override Task BeforeCombatStart()
    {
        _usesLeft = DynamicVars["MaxUses"].IntValue;
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }
}