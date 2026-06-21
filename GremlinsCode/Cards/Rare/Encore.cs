using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Rare;

[Pool(typeof(GremlinsCardPool))]
public class Encore : GremlinsCardModel
{
    public Encore() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<WizPower>(3);
        WithPower<EncorePower>(4, 2);
    }

    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<WizPower>(ctx, this);
        await CommonActions.ApplySelf<EncorePower>(ctx, this);
    }
}