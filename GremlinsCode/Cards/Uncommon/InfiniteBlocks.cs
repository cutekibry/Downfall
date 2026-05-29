using BaseLib.Utils;
using Gremlins.GremlinsCode.Cards.Token;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class InfiniteBlocks : GremlinsCardModel
{
    public InfiniteBlocks() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<InfiniteBlocksPower>(1);
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        this.WithTip<Ward>();
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<InfiniteBlocksPower>(ctx, this);
    }
}