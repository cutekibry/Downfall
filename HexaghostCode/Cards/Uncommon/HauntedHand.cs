using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.Extensions;
using Hexaghost.HexaghostCode.Interfaces;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Hexaghost.HexaghostCode.Cards.Uncommon;

[Pool(typeof(HexaghostCardPool))]
public class HauntedHand : HexaghostCardModel, IHasAfterlifeEffect
{
    private bool _isInTurnEnd = false;
    private bool _delayedDraw = false;

    public HauntedHand() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        this.WithAfterlife();
        WithCards(1, 1);
        WithTip(CardKeyword.Ethereal);
    }

    protected override Artist Artist => Artist.Get<Opal>();

    public override Task BeforeSideTurnEndVeryEarly(PlayerChoiceContext ctx, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner.Creature))
            _isInTurnEnd = true;
        return Task.CompletedTask;
    }
    public override async Task AfterSideTurnEndLate(PlayerChoiceContext ctx, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner.Creature))
        {
            _isInTurnEnd = false;
            if (_delayedDraw)
            {
                _delayedDraw = false;
                await AfterlifeEffect(ctx, null!);
            }
        }
    }
    
    public async Task AfterlifeEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        if (_isInTurnEnd)
        {
            _delayedDraw = true;
            return;
        }
        while (Owner.GetHand().Count < 10)
        {
            var drawn = await CardPileCmd.Draw(ctx, Owner);
            if (drawn == null || !drawn.Keywords.Contains(CardKeyword.Ethereal)) return;
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(ctx, DynamicVars.Cards.IntValue, Owner);
        await AfterlifeEffect(ctx, cardPlay);
    }
}