using BaseLib.Utils;
using Gremlins.GremlinsCode.Cards.Token;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class Polish : GremlinsCardModel
{
    public Polish() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<AccuracyPower>(2, 1);
        WithPower<PolishPower>(2, 1);
        this.WithTip<Shiv>();
        this.WithTip<Ward>();
        WithTip(StaticHoverTip.Block);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<AccuracyPower>(ctx, this);
        await CommonActions.ApplySelf<PolishPower>(ctx, this);
    }
}