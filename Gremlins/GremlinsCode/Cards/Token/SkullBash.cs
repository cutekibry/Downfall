using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Gremlins.GremlinsCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class SkullBash : GremlinsCardModel
{
    public SkullBash() : base(1, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
    {
        WithKeywords(CardKeyword.Ethereal, CardKeyword.Exhaust);
        WithDamage(6, 2);
        WithPower<VulnerablePower>(2);

    }
    
    protected override async Task PlayEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        await MyCommonActions.Apply<VulnerablePower>(ctx, this, cardPlay);
    }
}