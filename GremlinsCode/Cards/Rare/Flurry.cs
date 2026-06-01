using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Gremlins.GremlinsCode.Cards.Rare;

[Pool(typeof(GremlinsCardPool))]
public class Flurry : GremlinsCardModel
{
    public Flurry() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(3);
        WithCostUpgradeBy(-1);
        WithKeyword(CardKeyword.Exhaust);
        WithCalculatedVar("Repeat", 0, Calc);
    }

    private static decimal Calc(CardModel card, Creature? creature)
    {
        return CombatManager.Instance.History.CardPlaysFinished.Count(e =>
            e.HappenedThisTurn(card.CombatState) && e.Actor == card.Owner.Creature);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var amount = (int)((CalculatedVar)DynamicVars["Repeat"]).Calculate(cardPlay.Target);
        await CommonActions.CardAttack(this, cardPlay, amount).Execute(ctx);
    }
}