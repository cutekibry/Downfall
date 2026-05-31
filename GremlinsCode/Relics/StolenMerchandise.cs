using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace Gremlins.GremlinsCode.Relics;

[Obsolete]
[Pool(typeof(GremlinsRelicPool))]
public class StolenMerchandise() : GremlinsRelicModel(RelicRarity.Shop, false)
{
    
}