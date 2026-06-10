using BaseLib.Utils;
using Guardian.GuardianCode.Core;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace Guardian.GuardianCode.Relics;

[Pool(typeof(GuardianRelicPool))]
public class SackOfGems() : GuardianRelicModel(RelicRarity.Shop)
{
    public override async Task AfterObtained()
    {
        var rerollOptions = CardCreationOptions.ForNonCombatWithDefaultOdds([Owner.Character.CardPool]);
        var rewards = new List<Reward>();
        for (var i = 0; i < 5; i++)
        {
            var reward = GuardianModelDb.GenerateSingleGemReward(Owner, rerollOptions);
            if (reward != null) rewards.Add(reward);
        }

        await RewardsCmd.OfferCustom(Owner, rewards);
    }
}
