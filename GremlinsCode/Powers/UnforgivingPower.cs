using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Gremlins.GremlinsCode.Powers;

public class UnforgivingPower() : GremlinsPowerModel(PowerType.Buff, PowerStackType.Single)
{
    public override decimal ModifyPowerAmountGivenMultiplicative(PowerModel power, Creature giver, decimal amount, Creature? target,
        CardModel? cardSource)
    {
        return power is StrengthPower && power.Owner == Owner && amount < 0 ? 0 : 1;
    }
}