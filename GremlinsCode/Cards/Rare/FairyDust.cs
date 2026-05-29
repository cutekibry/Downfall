using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using Gremlins.GremlinsCode.Cards.Token;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Rare;

[Pool(typeof(GremlinsCardPool))]
public class FairyDust : GremlinsCardModel
{
    public FairyDust() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithCostUpgradeBy(-1);
        WithCards(2);
        this.WithTip<Ward>();
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await DownfallCardCmd.GiveCards<Ward>(Owner, PileType.Hand, DynamicVars.Cards.BaseValue);
        await CommonActions.Draw(this, ctx);
    }
}