using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class FeelTheAudience : GremlinsCardModel
{
    public FeelTheAudience() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithDamage(8, 3);
        WithPower<WizPower>(1);
    }

    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var count = CombatState?.HittableEnemies.Count(e => e.Monster?.IntendsToAttack ?? false) ?? 0;
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        if (count <= 0) return;
        await PowerCmd.Apply<WizPower>(ctx, Owner.Creature, count, Owner.Creature, this);
    }
}