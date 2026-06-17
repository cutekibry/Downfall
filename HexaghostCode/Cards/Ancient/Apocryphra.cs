using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Downfall.DownfallCode.Powers;
using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.Extensions;
using Hexaghost.HexaghostCode.Interfaces;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hexaghost.HexaghostCode.Cards.Ancient;

[Pool(typeof(HexaghostCardPool))]
public class Apocryphra : HexaghostCardModel, IHasAfterlifeEffect
{
    private bool _isInTurnEnd = false;
    private bool _delayedReturnToHand = false;

    public Apocryphra() : base(1, CardType.Attack, CardRarity.Ancient, TargetType.AllEnemies)
    {
        this.WithAfterlife();
        WithDamage(6, 2);
        WithPower<SoulBurnPower>(6, 2);
    }

    protected override Artist Artist => Artist.Get<GoofballMcgee>();

    public override Task BeforeSideTurnEndVeryEarly(PlayerChoiceContext ctx, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner.Creature))
            _isInTurnEnd = true;
        return Task.CompletedTask;
    }
    public override Task AfterSideTurnEndLate(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner.Creature))
        {
            _isInTurnEnd = false;
            if (_delayedReturnToHand)
            {
                _delayedReturnToHand = false;
                return CardPileCmd.Add(this, PileType.Hand);
            }
        }
        return Task.CompletedTask;
    }

    public async Task AfterlifeEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        foreach (var enemy in CombatState!.HittableEnemies)
        {
            await CommonActions.Apply<SoulBurnPower>(ctx, enemy, this);
        }
        if (!_isInTurnEnd)
        {
            await CardPileCmd.Add(this, PileType.Hand);
        }
        else
        {
            _delayedReturnToHand = true;
        }
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        await AfterlifeEffect(ctx, cardPlay);
    }
}