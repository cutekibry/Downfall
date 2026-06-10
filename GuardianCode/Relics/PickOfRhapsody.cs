using BaseLib.Utils;
using Guardian.GuardianCode.Core;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace Guardian.GuardianCode.Relics;

[Pool(typeof(GuardianRelicPool))]
public class PickOfRhapsody() : GuardianRelicModel(RelicRarity.Uncommon)
{
    public override Task AfterCombatEnd(CombatRoom room)
    {
        if (room.RoomType != RoomType.Elite) return Task.CompletedTask;

        var gemReward = GuardianModelDb.GenerateSingleGemReward(
            Owner,
            CardCreationOptions.ForRoom(Owner, room.RoomType));
        if (gemReward == null) return Task.CompletedTask;

        room.AddExtraReward(Owner, gemReward);
        return Task.CompletedTask;
    }
}