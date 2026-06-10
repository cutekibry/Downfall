using System.Threading.Tasks;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace Guardian.GuardianCode.Powers;

public class GemFinderPower : GuardianPowerModel
{
    public GemFinderPower()
    {
        WithTip(GuardianKeyword.Gem);
    }
    public override Task AfterCombatEnd(CombatRoom room)
    {
        var player = Owner.Player;
        if (player == null) return Task.CompletedTask;

        var rerollOptions = CardCreationOptions.ForRoom(player, room.RoomType);
        for (int i = 0; i < Amount; i++)
        {
            var gemReward = GuardianModelDb.GenerateSingleGemReward(player, rerollOptions);
            if (gemReward != null) room.AddExtraReward(player, gemReward);
        }
        return Task.CompletedTask;
    }
}
