using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using Gremlins.GremlinsCode.Cards.Uncommon;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Gremlins.GremlinsCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class Bang : GremlinsCardModel
{
    public Bang() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithKeyword(CardKeyword.Exhaust);
        WithUpgradingCardTip<Whiz>();
        WithDamage(0);
        WithRepeat(3, 1);
    }

    protected override async Task PlayEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        await DownfallCardCmd.GiveCard<Whiz>(Owner, PileType.Discard, upgraded: IsUpgraded);
    }
}