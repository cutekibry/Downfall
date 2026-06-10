

using BaseLib.Utils;
using Guardian.GuardianCode.Cards.Abstract;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Guardian.GuardianCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class PackageShapes : GuardianCardModel, IPackageCard
{
    public PackageShapes() : base(0, CardType.Attack, CardRarity.Token, TargetType.AllEnemies)
    {
        WithDamage(4, 2);
        WithPower<ThornsPower>(3, 1);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        await CardPileCmd.Draw(ctx, Owner);
        await CommonActions.ApplySelf<ThornsPower>(ctx, this);
    }
}