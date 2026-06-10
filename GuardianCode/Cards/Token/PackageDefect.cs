

using BaseLib.Utils;
using Guardian.GuardianCode.Cards.Abstract;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Guardian.GuardianCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class PackageDefect : GuardianCardModel, IPackageCard
{
    public PackageDefect() : base(0, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithVar("StasisSlots", 1);
        this.WithPower<ArtifactPower>(0, 1, false);
        this.WithTip(typeof(ArtifactPower), UpgradeType.Add);
        WithTip(GuardianTip.Stasis);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
    {
        GuardianCmd.AddMaxStasisSlots(Owner, 1);
        if (IsUpgraded)
            await CommonActions.ApplySelf<ArtifactPower>(ctx, this);
    }
}