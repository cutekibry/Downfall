using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gremlins.GremlinsCode.Powers;

public class ScatterPower : GremlinsPowerModel
{
    public override Decimal ModifyHpLostAfterOstyLate(
        Creature target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        return target != Owner ? amount : 0M;
    }

    protected override async Task AfterModifyingHpLostAfterOsty(PlayerChoiceContext ctx)
    {
        if (Owner.Player == null) return;
        await GremlinsCmd.SwapToRandomGremlin(ctx, Owner.Player);
        await PowerCmd.Decrement(this);
    }

}