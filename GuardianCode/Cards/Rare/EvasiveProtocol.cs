using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using Guardian.GuardianCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Guardian.GuardianCode.Cards.Rare;

[Pool(typeof(GuardianCardPool))]
public class EvasiveProtocol : GuardianCardModel
{
    public EvasiveProtocol() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        this.WithPower<EvasiveProtocolPower>(5, false);
        WithCostUpgradeBy(-1);
        WithTip(GuardianTip.Brace);
    }

    protected override Artist Artist => Artist.Get<CartesianCanvas>();

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<EvasiveProtocolPower>(ctx, this);
    }
}