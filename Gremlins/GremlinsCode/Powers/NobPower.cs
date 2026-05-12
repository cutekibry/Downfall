using Downfall.DownfallCode.Utils.Sound;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Events;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace Gremlins.GremlinsCode.Powers;

public class NobPower() : GremlinsPowerModel(PowerType.Buff, PowerStackType.Single), IShouldGremlinSwap, IAfterGremlinSwap
{
    private static ModSoundEffect SoundEffect => new(
        new ModSoundEntry("res://Gremlins/audio/character_select/STS_VO_GremlinNob_1a_v3.ogg", 5, 0.1f, 1, 10)
    );

    private static readonly LocString GremlinNobDialogue = new("monsters", "GREMLINS-GREMLIN_NOB.banter");

    protected override async Task AfterApplied(PlayerChoiceContext ctx, Creature? applier, CardModel? cardSource)
    {
        var player = Owner.Player;
        if (player == null) return;
        GremlinsCmd.AddGremlin(player, ModelDb.Monster<GremlinNob>(), 20, 20);
        await GremlinsCmd.SwapToType<GremlinNob>(ctx, player);
        await Cmd.Wait(1);
        var a = GremlinsCmd.GetCurrentGremlin(player);
        if (a == null) return;
        TalkCmd.Play(GremlinNobDialogue, a, VfxColor.Red);
        SoundEffect.Play();
    }
    
    

    protected override Task AfterRemoved(PlayerChoiceContext ctx, Creature oldOwner)
    {
        var a = GremlinsCmd.GetCurrentGremlin(Owner.Player);
        if (a is not { Monster: GremlinNob }) return Task.CompletedTask;
        GremlinsCmd.KillGremlin(Owner, a);
        return Task.CompletedTask;
    }

    public bool ShouldGremlinSwap(Player player, Creature gremlin) =>
        player.Creature != Owner || gremlin.Monster is GremlinNob;

    public  async Task AfterGremlinSwap(PlayerChoiceContext ctx, Player player, GremlinSwapType gremlinSwapType)
    {
        if (gremlinSwapType != GremlinSwapType.Death) return;
        await PowerCmd.Remove(this);
    }
}