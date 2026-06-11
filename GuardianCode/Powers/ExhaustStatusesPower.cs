using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.Events;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Guardian.GuardianCode.Powers;

public class ExhaustStatusesPower : GuardianPowerModel, IAfterCardEntersStasis
{
    public override bool ShouldReceiveCombatHooks => true;

    public async Task AfterCardEntersStasis(PlayerChoiceContext ctx, CardModel card, AbstractModel source)
    {
        if (card.Owner.Creature == Owner && card.Keywords.Contains(CardKeyword.Unplayable))
        {
            await CardCmd.Exhaust(ctx, card);
            await CardPileCmd.Draw(ctx, Amount, Owner.Player!);
        }
    }
}