using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class CounterStrike : GremlinsCardModel
{
    public CounterStrike() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithTags(CardTag.Strike);
        WithDamage(8, 2);
        WithRepeat(2, 1);
    }
    
    protected override async Task PlayEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        if (!(cardPlay.Target?.Monster?.IntendsToAttack ?? true)) return;
        var repeat = DynamicVars.Repeat.IntValue;
        for (var i = 0; i < repeat; i++)
        {
            await GremlinsCmd.TriggerGremlinBonus(ctx, Owner);
        }
      
    }
}