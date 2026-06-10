using BaseLib.Utils;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.Rewards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rewards;

namespace Guardian.GuardianCode.Relics;

[Pool(typeof(GuardianRelicPool))]
public class SackOfGems() : GuardianRelicModel(RelicRarity.Shop)
{
    public override async Task AfterObtained()
    {
        var rewards = new List<Reward>();
        for (var i = 0; i < 5; i++)
        {
            rewards.Add(new GemFinderReward(1, 1, Owner));
        }
        await RewardsCmd.OfferCustom(Owner, rewards);
    }
}