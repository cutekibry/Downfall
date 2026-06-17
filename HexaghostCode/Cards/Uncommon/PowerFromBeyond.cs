using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.Extensions;
using Hexaghost.HexaghostCode.Interfaces;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hexaghost.HexaghostCode.Cards.Uncommon;

[Pool(typeof(HexaghostCardPool))]
public class PowerFromBeyond : HexaghostCardModel, IHasAfterlifeEffect
{
    public PowerFromBeyond() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        this.WithAfterlife();
        WithEnergy(1, 1);
        this.WithPower<DrawCardsNextTurnPower>(2, false);
        this.WithPower<EnergyNextTurnPower>(1, 1, false);
    }

    protected override Artist Artist => Artist.Get<Thelethargicweirdo>();


    public async Task AfterlifeEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<DrawCardsNextTurnPower>(ctx, this);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await AfterlifeEffect(ctx, cardPlay);
        await CommonActions.ApplySelf<EnergyNextTurnPower>(ctx, this);
    }
}