using BaseLib.Utils;
using Guardian.GuardianCode.Cards.Abstract;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Guardian.GuardianCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class PackageAncients : GuardianCardModel, IPackageCard
{
    public PackageAncients() : base(0, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithPower<StrengthPower>(1, 1);
        WithPower<PlatingPower>(3, 1);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        await CommonActions.ApplySelf<StrengthPower>(ctx, this);
        await CommonActions.ApplySelf<PlatingPower>(ctx, this);
    }
}