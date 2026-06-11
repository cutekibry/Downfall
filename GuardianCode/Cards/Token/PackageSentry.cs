using BaseLib.Utils;
using Guardian.GuardianCode.Cards.Abstract;
using Guardian.GuardianCode.Cards.Uncommon;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Guardian.GuardianCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class PackageSentry : GuardianCardModel, IPackageCard
{
    public PackageSentry() : base(0, CardType.Skill, CardRarity.Token, TargetType.AllEnemies)
    {
        WithTips(c => [HoverTipFactory.FromCard<SentryBlast>(true)]);
        WithTips(c => c.IsUpgraded ? [HoverTipFactory.FromCard<SentryWave>(true)] : []);
        WithTip(GuardianTip.Stasis);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        if (!GuardianCmd.CanPutIntoStasis(Owner)) return;
        var blast = Owner.RunState.CreateCard<SentryBlast>(Owner);
        CardCmd.Upgrade(blast);
        await CardPileCmd.AddGeneratedCardToCombat(blast, PileType.Hand, Owner);
        await GuardianCmd.PutIntoStasis(blast, ctx, this);

        if (IsUpgraded)
        {
            if (!GuardianCmd.CanPutIntoStasis(Owner)) return;
            var wave = Owner.RunState.CreateCard<SentryWave>(Owner);
            CardCmd.Upgrade(wave);
            await CardPileCmd.AddGeneratedCardToCombat(wave, PileType.Hand, Owner);
            await GuardianCmd.PutIntoStasis(wave, ctx, this);
        }
    }
}