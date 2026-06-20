using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Rare;

[Pool(typeof(GremlinsCardPool))]
public class Unforgiving : GremlinsCardModel
{
    public Unforgiving() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<UnforgivingPower>(1);
        WithCostUpgradeBy(-1);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<UnforgivingPower>(ctx, this);
    }
}