using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using Gremlins.GremlinsCode.Cards.Token;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Common;

[Pool(typeof(GremlinsCardPool))]
public class Tadah : GremlinsCardModel
{
    public Tadah() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithUpgradingCardTip<Ward>();
    }

    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await DownfallCardCmd.GiveCard<Ward>(Owner, PileType.Hand, upgraded: IsUpgraded);
        await GremlinsCmd.SwapToType<WizardGremlin>(ctx, Owner);
    }
}