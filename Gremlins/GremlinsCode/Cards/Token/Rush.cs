using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Gremlins.GremlinsCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class Rush : GremlinsCardModel
{
    public Rush() : base(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
    {
    }

    // TODO: Implement
    protected override async Task PlayEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
    }
}