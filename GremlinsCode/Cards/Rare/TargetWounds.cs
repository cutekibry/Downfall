using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Rare;

[Pool(typeof(GremlinsCardPool))]
public class TargetWounds : GremlinsCardModel
{
    public TargetWounds() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<TargetWoundsPower>(3, 2);
    }

    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<TargetWoundsPower>(ctx, this);
    }
}