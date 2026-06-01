using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

namespace Gremlins.GremlinsCode.Relics;

[Pool(typeof(GremlinsRelicPool))]
public class MobLeadersCrown : GremlinsRelicModel
{
    public MobLeadersCrown() : base(RelicRarity.Starter)
    {
        WithEnergy(1);
        WithCards(1);
    }
    private bool _usedThisCombat;

    private bool UsedThisCombat
    {
        get => _usedThisCombat;
        set
        {
            if (_usedThisCombat == value)
                return;
            AssertMutable();
            _usedThisCombat = value;
        }
    }

    public override async Task AfterShuffle(PlayerChoiceContext ctx, Player shuffler)
    {
        if (shuffler != Owner || UsedThisCombat) return;
        Flash();
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
        await CardPileCmd.Draw(ctx, Owner);
        await GremlinsCmd.SwapToNext(ctx, Owner);
        UsedThisCombat = true;
    }
    
    public override Task AfterCombatEnd(CombatRoom _)
    {
        UsedThisCombat = false;
        return Task.CompletedTask;
    }
}