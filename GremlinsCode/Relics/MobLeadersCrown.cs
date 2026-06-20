using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

namespace Gremlins.GremlinsCode.Relics;

[Pool(typeof(GremlinsRelicPool))]
public class MobLeadersCrown : GremlinsRelicModel
{
    private bool _triggeredThisTurn;

    public MobLeadersCrown() : base(RelicRarity.Starter)
    {
        WithEnergy(1);
        WithCards(1);
    }

    public override async Task AfterShuffle(PlayerChoiceContext ctx, Player shuffler)
    {
        if (shuffler != Owner || _triggeredThisTurn) return;
        Flash();
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        await CardPileCmd.Draw(ctx, Owner);
        await GremlinsCmd.SwapToNext(ctx, Owner);
        _triggeredThisTurn = true;
    }

    public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
	{
		if (!participants.Contains(base.Owner.Creature))
		{
			return Task.CompletedTask;
		}
		_triggeredThisTurn = false;
		return Task.CompletedTask;
	}
}