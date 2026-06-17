using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.Extensions;
using Hexaghost.HexaghostCode.Interfaces;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Hexaghost.HexaghostCode.Cards.Uncommon;

[Pool(typeof(HexaghostCardPool))]
public class Floatwork : HexaghostCardModel, IHasAfterlifeEffect
{
    public Floatwork() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
    {
        this.WithAfterlife();
        WithPower<DexterityPower>(1);
        WithPower<PlatingPower>(3, 1);
    }

    protected override Artist Artist => Artist.Get<Thelethargicweirdo>();

    public override async Task BeforeSideTurnEndVeryEarly(PlayerChoiceContext ctx, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner.Creature) && Owner.GetHand().Contains(this))
        {
            await CommonActions.ApplySelf<PlatingPower>(ctx, this);
        }
    }

    public Task AfterlifeEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        return Task.CompletedTask;
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<DexterityPower>(ctx, this);
        await CommonActions.ApplySelf<PlatingPower>(ctx, this);
    }
}