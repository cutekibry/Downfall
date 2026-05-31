using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Events;

public interface IAfterWizConsumed
{
    Task AfterWizConsumed(PlayerChoiceContext ctx, Creature oldOwner);
}