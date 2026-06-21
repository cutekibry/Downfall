using BaseLib.Extensions;
using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class Fury : GremlinsCardModel
{
    private int _lastReduction;

    public Fury() : base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(5, 2);
        this.WithRepeat(3);
        WithEnergy(1);
        WithPower<StrengthPower>(2);
    }

    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay, DynamicVars.Repeat.IntValue).Execute(ctx);
    }

    private void RecalculateCostReduction()
    {
        var strength = Owner.Creature.GetPowerAmount<StrengthPower>();
        var reduction = strength / DynamicVars.Power<StrengthPower>().IntValue;
        var delta = reduction - _lastReduction;
        if (delta == 0) return;
        EnergyCost.AddThisCombat(-delta);
        _lastReduction = reduction;
    }

    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this || IsClone) return Task.CompletedTask;
        RecalculateCostReduction();
        return Task.CompletedTask;
    }

    public override Task AfterPowerAmountChanged(PlayerChoiceContext ctx, PowerModel power, decimal amount,
        Creature? applier, CardModel? cardSource)
    {
        if (power.Owner != Owner.Creature || power is not StrengthPower) return Task.CompletedTask;
        RecalculateCostReduction();
        return Task.CompletedTask;
    }
}