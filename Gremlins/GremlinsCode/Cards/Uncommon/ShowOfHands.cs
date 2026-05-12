using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class ShowOfHands : GremlinsCardModel
{
    public ShowOfHands() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(0, 1);
        WithCalculatedBlock(0, 2, Calc);
    }

    private static decimal Calc(CardModel card, Creature? arg2) =>
        card.Owner.PlayerCombatState?.Hand.Cards.Count(e => card != e) ?? 0;

    protected override async Task PlayEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.Draw(this, ctx);
        await MyCommonActions.CardCalculatedBlock(this, cardPlay);
    }
}