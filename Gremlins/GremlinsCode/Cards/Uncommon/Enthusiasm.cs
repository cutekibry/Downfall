using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class Enthusiasm : GremlinsCardModel
{
    public Enthusiasm() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
        WithPower<EnthusiasmPower>(1);
        WithCostUpgradeBy(-1);
    }
    
    protected override async Task PlayEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<EnthusiasmPower>(ctx, this);
    }
}