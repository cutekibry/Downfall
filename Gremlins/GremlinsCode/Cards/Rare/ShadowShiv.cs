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
    public ShadowShiv() : base(3, CardType.Power, CardRarity.Rare, TargetType.None)
    {
        WithPower<ShadowShivPower>(1);
        WithCostUpgradeBy(-1);
        WithTip(typeof(Shiv));
        WithTip(CardKeyword.Exhaust);
    }
    
    protected override async Task PlayEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<ShadowShivPower>(ctx, this);
    }
}