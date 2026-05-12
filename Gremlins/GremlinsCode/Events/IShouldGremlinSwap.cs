using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Gremlins.GremlinsCode.Events;

public interface IShouldGremlinSwap
{
    bool ShouldGremlinSwap(Player player, Creature gremlin);
}