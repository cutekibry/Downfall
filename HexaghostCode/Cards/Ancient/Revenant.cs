using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.Extensions;
using Hexaghost.HexaghostCode.Interfaces;
using Hexaghost.HexaghostCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hexaghost.HexaghostCode.Cards.Ancient;

[Pool(typeof(HexaghostCardPool))]
public class Revenant : HexaghostCardModel, IHasAfterlifeEffect
{
    public Revenant() : base(2, CardType.Power, CardRarity.Ancient, TargetType.Self)
    {
        this.WithAfterlife();
        WithKeyword(CardKeyword.Innate, UpgradeType.Add);
        WithPower<IntensityPower>(1);
        this.WithPower<MoreEnergyPower>(1, false);
        WithEnergy(1);
    }

    protected override Artist Artist => Artist.Get<GoofballMcgee>();

    public async Task AfterlifeEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<IntensityPower>(ctx, this, -DynamicVars["IntensityPower"].BaseValue);
        await CommonActions.ApplySelf<MoreEnergyPower>(ctx, this);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await AfterlifeEffect(ctx, cardPlay);
        await CommonActions.ApplySelf<MoreEnergyPower>(ctx, this);
    }
}