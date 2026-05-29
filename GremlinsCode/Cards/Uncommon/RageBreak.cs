using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class RageBreak : GremlinsCardModel
{
    public RageBreak() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        this.WithTip<PossessStrengthPower>();
        WithKeyword(CardKeyword.Exhaust);
        WithBlock(0, 5);
    }

    public override bool GainsBlock => IsUpgraded;

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var powerAmount = Owner.Creature.GetPowerAmount<StrengthPower>();
        if (powerAmount <= 0) return;
        await PowerCmd.Apply<StrengthPower>(ctx, Owner.Creature, powerAmount,
            Owner.Creature, this);
    }
}