using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gremlins.GremlinsCode.Powers;

public class AgonyPower : GremlinsPowerModel
{

    public AgonyPower() : base(PowerType.Debuff)
    {
        WithVar("DamageDecrease", 0.2M);
    }

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, 
        Creature? dealer, CardModel? cardSource)
        => dealer != Owner || !props.IsPoweredAttack() ? 1 : DynamicVars["DamageDecrease"].BaseValue;
   

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != CombatSide.Enemy)
            return;
        await PowerCmd.TickDownDuration(this);
    }
}