using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Gremlins.GremlinsCode.Cards.Rare;

[Pool(typeof(GremlinsCardPool))]
public class BrokenShin : GremlinsCardModel
{
    public BrokenShin() : base(2, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithCalculatedVar("Agony", 0, 1, Calc);
        WithVar("WeakDiv", 4, -1);
        WithKeyword(CardKeyword.Exhaust);
        this.WithTip<AgonyPower>();
        this.WithTip<WeakPower>();
    }

    private static decimal Calc(CardModel card, Creature? creature)
    {
        return Math.Floor(creature?.GetPowerAmount<WeakPower>() / card.DynamicVars["WeakDiv"].BaseValue ?? 0);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        var amount = ((CalculatedVar)DynamicVars["Agony"]).Calculate(cardPlay.Target);
        await PowerCmd.Apply<AgonyPower>(ctx, cardPlay.Target, amount, Owner.Creature, this);
    }
}