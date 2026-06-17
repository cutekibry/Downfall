using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Downfall.DownfallCode.Powers;
using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hexaghost.HexaghostCode.Cards.Uncommon;

[Pool(typeof(HexaghostCardPool))]
public class ExtraCrispy : HexaghostCardModel
{
    public ExtraCrispy() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        WithPower<ExtraCrispyPower>(12, 4);
        WithTip(typeof(SoulBurnPower));
    }

    protected override Artist Artist => Artist.Get<CartesianCanvas>();

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<ExtraCrispyPower>(ctx, this);
    }
}