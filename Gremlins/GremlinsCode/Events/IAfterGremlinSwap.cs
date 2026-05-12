using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Events;

public interface IAfterGremlinSwap
{
    Task AfterGremlinSwap(PlayerChoiceContext ctx, Player player, GremlinSwapType  gremlinSwapType);
}