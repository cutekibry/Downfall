using BaseLib.Utils;
using Guardian.GuardianCode.Cards.Abstract;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Guardian.GuardianCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class PackageOrbwalker : GuardianCardModel, IPackageCard
{
    public PackageOrbwalker() : base(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
    {
        WithDamage(16, 5);
        this.WithAccelerate(1);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.CardAttack(this, play).Execute(ctx);
        await GuardianCmd.Accelerate(ctx, this);
    }
}