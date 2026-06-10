using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Guardian.GuardianCode.Cards.Uncommon;

[Pool(typeof(GuardianCardPool))]
public class Emergency : GuardianCardModel
{
    public Emergency() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithTip(GuardianTip.Stasis);
        WithTip(GuardianTip.Accelerate);
        WithKeyword(CardKeyword.Exhaust, UpgradeType.Remove);
    }

    protected override Artist Artist => Artist.Get<Thelethargicweirdo>();

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var stasisCount = GuardianCmd.GetStasisCount(Owner);
        while (GuardianCmd.GetStasisCount(Owner) == stasisCount)
        {
            await GuardianCmd.Accelerate(ctx, this);
        }
    }
}