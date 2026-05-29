using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Gremlins.GremlinsCode.Cards.Rare;

[Pool(typeof(GremlinsCardPool))]
public class ShadowShiv : GremlinsCardModel
{
    public ShadowShiv() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<ShadowShivPower>(1);
        WithCostUpgradeBy(-1);
        this.WithTip<Shiv>();
        WithTip(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<ShadowShivPower>(ctx, this);
    }
}