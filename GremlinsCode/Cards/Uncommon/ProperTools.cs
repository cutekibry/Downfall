using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using Gremlins.GremlinsCode.Cards.Token;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class ProperTools : GremlinsCardModel
{
    public ProperTools() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(5, 3);
        WithCards(3, 1);
        this.WithTip<Shiv>();
        this.WithTip<Ward>();
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        if (cardPlay.Target?.Monster == null) return;
        if (cardPlay.Target.Monster.IntendsToAttack)
            await DownfallCardCmd.GiveCards<Ward>(Owner, PileType.Hand, DynamicVars.Cards.IntValue);
        else
            await DownfallCardCmd.GiveCards<Shiv>(Owner, PileType.Hand, DynamicVars.Cards.IntValue);
    }
}