using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class Dazzle : GremlinsCardModel
{
    public Dazzle() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(9, 4);
        WithPower<StrengthPower>(2);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        if (Owner.Creature.GetPowerAmount<WizPower>() < 3) return;
        await DownfallCmd.Steal<StrengthPower>(ctx, cardPlay, this);
    }
}