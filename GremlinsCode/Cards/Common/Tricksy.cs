using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Common;

[Pool(typeof(GremlinsCardPool))]
public class Tricksy : GremlinsCardModel
{
    public Tricksy() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithCards(4, 2);
    }

    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var cards = await CommonActions.Draw(this, ctx);
        await CardCmd.Discard(ctx, cards.Where(e => e.Type != CardType.Attack));
        await GremlinsCmd.SwapToType<SneakGremlin>(ctx, Owner);
    }
}