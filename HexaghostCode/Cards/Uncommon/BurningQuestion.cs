using BaseLib.Utils;
using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.Extensions;
using Hexaghost.HexaghostCode.Interfaces;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hexaghost.HexaghostCode.Cards.Uncommon;

[Pool(typeof(HexaghostCardPool))]
public class BurningQuestion : HexaghostCardModel, IHasAfterlifeEffect
{
    public BurningQuestion() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        this.WithAfterlife();
        WithPower<StrengthPower>(3, 1);
        WithPower<DexterityPower>(1);
    }

    public async Task AfterlifeEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<DexterityPower>(ctx, this);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<StrengthPower>(ctx, this);
        await AfterlifeEffect(ctx, cardPlay);
    }
}
