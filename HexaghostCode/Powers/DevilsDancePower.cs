using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.Events;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Hexaghost.HexaghostCode.Powers;

public class DevilsDancePower : HexaghostPowerModel, IWheelMoved
{
    public Task AfterWheelAdvance(PlayerChoiceContext ctx, Player player, AbstractModel? source, GhostflameModel ghostflame, int ghostflameIndex, bool silent)
    {
        return Task.CompletedTask;
    }

    public async Task AfterWheelRetract(PlayerChoiceContext ctx, Player player, AbstractModel? source,
        GhostflameModel ghostflame,
        int ghostflameIndex, bool silent)
    {
        if (silent) return;
        await CardPileCmd.Draw(ctx, Amount, player);
    }
}