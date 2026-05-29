using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using Gremlins.GremlinsCode.Cards.Token;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class Astound : GremlinsCardModel
{
    public Astound() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(5, 2);
        WithUpgradingCardTip<Ward>();
        this.WithTip<WizPower>();
        WithCards(2);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);
        if (Owner.Creature.GetPowerAmount<WizPower>() < 3) return;
        await DownfallCardCmd.GiveCards<Ward>(Owner, PileType.Hand, DynamicVars.Cards.IntValue, upgraded: IsUpgraded);
    }
}