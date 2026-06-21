using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Rare;

[Pool(typeof(GremlinsCardPool))]
public class Necromancy : GremlinsCardModel
{
    public Necromancy() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithKeyword(CardKeyword.Exhaust);
        this.WithTip<WizPower>();
        WithHeal(10, 3);
    }

    protected override bool IsPlayable => Owner.Creature.GetPowerAmount<WizPower>() >= 3;
    protected override bool ShouldGlowGoldInternal => Owner.Creature.GetPowerAmount<WizPower>() >= 3;

    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await PowerCmd.Remove<WizPower>(Owner.Creature);
        GremlinsCmd.ResurrectRandomGremlin(Owner, DynamicVars.Heal.IntValue);
    }
}