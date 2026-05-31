using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Gremlins.GremlinsCode.Relics;

[Obsolete]
[Pool(typeof(GremlinsRelicPool))]
public class FragmentationGrenade() : GremlinsRelicModel(RelicRarity.Uncommon, false)
{
}