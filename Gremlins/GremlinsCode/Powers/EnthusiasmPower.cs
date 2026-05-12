using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Events;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Powers;

public class EnthusiasmPower : GremlinsPowerModel, IAfterGremlinSwap
{
    public async Task AfterGremlinSwap(PlayerChoiceContext ctx, Player player,  GremlinSwapType  gremlinSwapType)
    {
        if (player.Creature != Owner || gremlinSwapType != GremlinSwapType.Move) return;
        Flash();
        await CardPileCmd.Draw(ctx, Amount, player);
    }
}