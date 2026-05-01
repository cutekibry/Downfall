using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Common;

[Pool(typeof(GremlinsCardPool))]
public class ChangeO : GremlinsCardModel
{
    public ChangeO() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    // TODO: Implement
    protected override async Task PlayEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
    }
}