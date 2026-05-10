using BaseLib.Utils;
using Gremlins.GremlinsCode.Cards.Token;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class Whiz : GremlinsCardModel
{
    public Whiz() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeyword(CardKeyword.Exhaust);
       
        WithPower<WizPower>(1, 1);
        WithTips(e =>
            e.IsUpgraded
                ? [HoverTipFactory.FromPower<MakingMagicPlusPower>()]
                : [HoverTipFactory.FromPower<MakingMagicPower>()]);
        WithUpgradingCardTip<Bang>();
    }
    
    protected override async Task PlayEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<WizPower>(ctx, this);
        if (IsUpgraded)
        {
            await CommonActions.ApplySelf<MakingMagicPlusPower>(ctx, this, 1);
        }
        else
        {
            await CommonActions.ApplySelf<MakingMagicPower>(ctx, this, 1);
        }

    }
}