using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using Guardian.GuardianCode.Interfaces;
using Guardian.GuardianCode.Rewards;
using MegaCrit.Sts2.Core.Rooms;

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
        var gemReward = new GemFinderReward(1, 1, player);
        room.AddExtraReward(player, gemReward);
        return Task.CompletedTask;
    }
}