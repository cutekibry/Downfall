using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Rare;

[Pool(typeof(GremlinsCardPool))]
public class ShowStopper : GremlinsCardModel
{
    public ShowStopper() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithDamage(3);
        this.WithRepeat(5, 1);
        this.WithTip<WizPower>();
    }

    protected override bool IsPlayable => Owner.Creature.GetPowerAmount<WizPower>() == 7;
    protected override bool ShouldGlowGoldInternal => Owner.Creature.GetPowerAmount<WizPower>() == 7;

    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay, DynamicVars.Repeat.IntValue).Execute(ctx);
    }
}