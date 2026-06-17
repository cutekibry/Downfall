using Downfall.DownfallCode.Events;
using Downfall.DownfallCode.Powers;
using Hexaghost.HexaghostCode.Core;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hexaghost.HexaghostCode.Powers;

public class ExtraCrispyPower : HexaghostPowerModel, IAfterSoulburnDetonate
{
    public async Task AfterSoulburnDetonate(PlayerChoiceContext ctx, Creature creature)
    {
        if (!creature.IsEnemy) return;
        await PowerCmd.Apply<SoulBurnPower>(ctx, creature, Amount, Owner, null);
    }
}