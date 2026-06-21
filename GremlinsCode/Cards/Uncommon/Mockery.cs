using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class Mockery : GremlinsCardModel
{
    public Mockery() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithPower<WeakPower>(1, 1);
        WithBlock(9, 3);
    }

    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var power = (await CommonActions.Apply<WeakPower>(ctx, this, cardPlay)).ToList().FirstOrDefault();
        if (power == null || power.Amount < 3) return;
        await CommonActions.CardBlock(this, cardPlay);
    }
}