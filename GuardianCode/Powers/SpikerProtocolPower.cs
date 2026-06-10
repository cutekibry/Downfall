using Guardian.GuardianCode.Core;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Guardian.GuardianCode.Powers;

public class SpikerProtocolPower : GuardianPowerModel
{
    public override async Task AfterSideTurnEnd(PlayerChoiceContext ctx, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner) && GuardianCmd.IsInMode<GuardianDefensiveMode>(Owner.Player!))
        {
            await PowerCmd.Apply<ThornsPower>(ctx, Owner, Amount, Owner, null);
        }
    }
}