using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Events;
using Gremlins.GremlinsCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Runs;

namespace Gremlins.GremlinsCode.Relics;

[Pool(typeof(GremlinsRelicPool))]
public class WizardHat : GremlinsRelicModel, IAfterWizConsumed
{
    public WizardHat() : base(RelicRarity.Uncommon)
    {
        WithTip<WizPower>();
    }
    
    public Task AfterWizConsumed(PlayerChoiceContext ctx, Creature oldOwner)
    {
        if (oldOwner != Owner.Creature) return Task.CompletedTask;
        var power = oldOwner.Powers.Where(e => e.TypeForCurrentAmount == PowerType.Debuff).TakeRandom(1, Owner.RunState.Rng.Niche).FirstOrDefault();
        return PowerCmd.Remove(power);
    }
}