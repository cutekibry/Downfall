using Guardian.GuardianCode.Core;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Guardian.GuardianCode.Powers;

public class EvasiveProtocolPower : GuardianPowerModel
{
    public override async Task BeforeHandDrawLate(Player player, PlayerChoiceContext ctx, ICombatState combatState)
    {
        if (player.Creature == Owner)
        {
            await GuardianCmd.Brace(ctx, player, Amount);
        }
    }
}