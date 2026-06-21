using BaseLib.Cards.Variables;
using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class BurlyBlow : GremlinsCardModel
{
    public BurlyBlow() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithCalculatedVar("Repeat", 1, Calc);
        WithDamage(4, 2);
    }

    private static decimal Calc(CardModel arg1, Creature? creature)
    {
        return creature?.GetPowerAmount<WeakPower>() ?? 0;
    }

    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var repeat = (int)((CustomCalculatedVar)DynamicVars["Repeat"]).Calculate(cardPlay.Target);
        await CommonActions.CardAttack(this, cardPlay, repeat).Execute(ctx);
    }
}