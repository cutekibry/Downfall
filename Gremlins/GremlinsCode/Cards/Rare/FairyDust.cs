using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Rare;

[Pool(typeof(GremlinsCardPool))]
public class FairyDust : GremlinsCardModel
{
    public FairyDust() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    // TODO: Implement
    protected override async Task PlayEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
    }
}