using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Common;

[Pool(typeof(GremlinsCardPool))]
public class GremlinArms() : GremlinsCardModel(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await GremlinsCmd.TriggerGremlinBonus(ctx, Owner);
        if (IsUpgraded)
            await GremlinsCmd.TriggerGremlinBonus(ctx, Owner);
        await GremlinsCmd.SwapToSelected(ctx, Owner);
    }
}