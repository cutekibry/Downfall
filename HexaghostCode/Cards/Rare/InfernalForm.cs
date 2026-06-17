using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hexaghost.HexaghostCode.Cards.Rare;

[Pool(typeof(HexaghostCardPool))]
public class InfernalForm : HexaghostCardModel
{
    public InfernalForm() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithPower<IntensityPower>(2);
        this.WithPower<InfernalFormPower>(2, false);
    }

    protected override Artist Artist => Artist.Get<GoofballMcgee>();

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        if (IsUpgraded) await CommonActions.ApplySelf<IntensityPower>(ctx, this);
        await CommonActions.ApplySelf<InfernalFormPower>(ctx, this);
    }
}