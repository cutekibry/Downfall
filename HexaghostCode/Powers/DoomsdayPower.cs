using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.Events;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hexaghost.HexaghostCode.Powers;

public class DoomsdayPower() : HexaghostPowerModel(PowerType.Buff, PowerStackType.Counter), IAfterGhostflameIgnited
{
    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;
    public async Task AfterGhostflameIgnited(PlayerChoiceContext ctx, Player player, GhostflameModel flame, int index)
    {
        if (player.Creature != Owner || HexaghostCmd.GetIgnitedCount(player) < Amount) return;
        await PowerCmd.Apply<DoomsArrivalPower>(ctx, Owner, 1, Owner, null);
        await PowerCmd.Remove(this);
    }
}