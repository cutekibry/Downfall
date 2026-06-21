using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class Revel : GremlinsCardModel
{
    public Revel() : base(3, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithEnergyTip();
        WithCostUpgradeBy(-1);
        WithEnergy(1);
    }

    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var amount = GremlinsCmd.GetLivingGremlinCount(Owner) * DynamicVars.Energy.IntValue;
        await PlayerCmd.GainEnergy(amount, Owner);
    }
}