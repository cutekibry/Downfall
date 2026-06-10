using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Guardian.GuardianCode.Cards.Ancient;

[Pool(typeof(GuardianCardPool))]
public class AncientConstruct : GuardianCardModel
{
    public AncientConstruct() : base(3, CardType.Power, CardRarity.Ancient, TargetType.Self)
    {
        this.WithPower<ArtifactPower>(0, 2, false);
        this.WithPower<AncientConstructPower>(1, false);
        this.WithTip(typeof(ArtifactPower), UpgradeType.Add);
    }

    protected override Artist Artist => Artist.Get<AlexMdle>();

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<ArtifactPower>(ctx, this);
        await CommonActions.ApplySelf<AncientConstructPower>(ctx, this);
    }
}