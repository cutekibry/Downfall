using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Common;

[Pool(typeof(GremlinsCardPool))]
public class CatScratch : GremlinsCardModel
{
    public CatScratch() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(2);
        this.WithRepeat(3, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay, DynamicVars.Repeat.IntValue).Execute(ctx);
    }
}