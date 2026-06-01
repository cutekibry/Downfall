using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Rooms;

namespace Gremlins.GremlinsCode.Relics;

[Pool(typeof(GremlinsRelicPool))]
public class ShortStatue() : GremlinsRelicModel(RelicRarity.Ancient)
{
    private bool _hasTriggered;
    private bool _isReviving;

    public override Task AfterCombatEnd(CombatRoom _)
    {
        _hasTriggered = false;
        return Task.CompletedTask;
    }
    

    public override bool ShouldAllowHitting(Creature creature)
    {
        return !IsOwnedGremlin(creature) || !_isReviving;
    }

    public override bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature)
    {
        return !IsOwnedGremlin(creature) || !_isReviving;
    }

    public override bool ShouldDie(Creature creature)
    {
        return _hasTriggered || !IsOwnedGremlin(creature);
    }

    public override async Task AfterDeath(
        PlayerChoiceContext choiceContext,
        Creature creature,
        bool wasRemovalPrevented,
        float deathAnimLength)
    {
        if (!wasRemovalPrevented || !IsOwnedGremlin(creature)) return;

        _hasTriggered = true;
        _isReviving = true;

        Flash();
        var healAmount = (int)(creature.MaxHp * 0.75f);
        await CreatureCmd.Heal(creature, healAmount);

        _isReviving = false;
    }

    private bool IsOwnedGremlin(Creature creature)
    {
        return creature == Owner.Creature;
    }
}