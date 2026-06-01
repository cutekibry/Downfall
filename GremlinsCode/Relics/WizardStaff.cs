using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Events;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace Gremlins.GremlinsCode.Relics;

[Pool(typeof(GremlinsRelicPool))]
public class WizardStaff : GremlinsRelicModel, IModifyWizExtraDamage
{
    public WizardStaff() : base(RelicRarity.Rare)
    {
        WithDamage(7);
        WithTip<WizPower>();
    }
    
    public decimal ModifyWizExtraDamage(WizPower wiz, decimal extraDamage)
     => wiz.Owner == Owner.Creature ?  extraDamage + DynamicVars.Damage.BaseValue : extraDamage;
}