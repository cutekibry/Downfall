using Hexaghost.HexaghostCode.Core;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Hexaghost.HexaghostCode.Powers;

public class PoltergeistPower : HexaghostPowerModel
{
    public override async Task AfterCardPlayed(PlayerChoiceContext ctx, CardPlay play)
    {
        if (play.Card.Owner.Creature != Owner || !play.Card.Keywords.Contains(CardKeyword.Ethereal)) return;
        var creature = CombatState.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
        if (creature == null) return;
        await CreatureCmd.Damage(ctx, creature, Amount,
            ValueProp.Unblockable | ValueProp.Unpowered, Owner, null);
        Flash();
    }
}