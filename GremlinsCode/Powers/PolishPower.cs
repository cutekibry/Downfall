using Gremlins.GremlinsCode.Cards.Token;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gremlins.GremlinsCode.Powers;

public class PolishPower : GremlinsPowerModel
{
    public PolishPower()
    {
        WithTip(StaticHoverTip.Block);
        WithTip<Ward>();
    }


    public override decimal ModifyBlockAdditive(Creature target, decimal block, ValueProp props, CardModel? card,
        CardPlay? cardPlay)
    {
        return Owner != target || !props.IsPoweredAttack() || card is not Ward ? 0M : Amount;
    }
}