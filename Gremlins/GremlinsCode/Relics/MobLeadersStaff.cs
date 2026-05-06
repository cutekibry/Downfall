using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;

namespace Gremlins.GremlinsCode.Relics;

[Pool(typeof(GremlinsRelicPool))]
public class MobLeadersStaff() : GremlinsRelicModel(RelicRarity.Starter)
{
    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<MobLeadersCrown>();
    }
    // TODO
}