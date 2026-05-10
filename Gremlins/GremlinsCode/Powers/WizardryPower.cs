using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Powers;

public class WizardryPower : GremlinsPowerModel
{
    protected override async Task AfterSideTurnStart(PlayerChoiceContext ctx, CombatSide side, ICombatState combatState)
    {
        if (side != Owner.Side) return;
        Flash();
        await PowerCmd.Apply<WizPower>(ctx, Owner,  Amount, Owner,  null);
    }
}