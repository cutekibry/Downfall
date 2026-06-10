using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Guardian.GuardianCode.Core;
using Guardian.GuardianCode.CustomEnums;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace Guardian.GuardianCode.Cards.Rare;

[Pool(typeof(GuardianCardPool))]
public class GigaBeam : GuardianCardModel
{
    public GigaBeam() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
        WithDamage(28, 6);
        WithTip(GuardianTip.DefensiveMode);
    }

    protected override Artist Artist => Artist.Get<CartesianCanvas>();

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(CombatState!)
            .WithAttackerAnim("Cast", 0.5f)
            .BeforeDamage(BeforeDamageAction)
            .Execute(ctx);
        if (GuardianCmd.IsInMode<GuardianDefensiveMode>(Owner))
            await GuardianCmd.LeaveDefensiveMode(ctx, Owner);
    }

    private async Task BeforeDamageAction()
    {
        var enemies = CombatState?.Enemies.Where(e => e.IsAlive).ToList();

        if (enemies != null)
        {
            var vfx = NHyperbeamVfx.Create(Owner.Creature, enemies.Last());
            if (vfx != null) NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(vfx);
        }

        await Cmd.Wait(0.5f);
        if (enemies != null)
            foreach (var impact in enemies
                         .Select(enemy => NHyperbeamImpactVfx.Create(Owner.Creature, enemy))
                         .OfType<NHyperbeamImpactVfx>())
                NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(impact);
    }
}