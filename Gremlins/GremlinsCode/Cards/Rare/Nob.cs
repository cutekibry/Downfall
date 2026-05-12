using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using Downfall.DownfallCode.Utils.Sound;
using Gremlins.GremlinsCode.Core;
using Gremlins.GremlinsCode.Powers;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace Gremlins.GremlinsCode.Cards.Rare;

[Pool(typeof(GremlinsCardPool))]
public class Nob : GremlinsCardModel
{
    public Nob() : base(4, CardType.Power, CardRarity.Rare, TargetType.None)
    {
        WithTempHp(20, 10);
        WithPower<NobPower>(1);
    }
    
    protected override async Task PlayEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await DownfallCmd.GainTempHp(ctx, this);
        await CommonActions.ApplySelf<NobPower>(ctx, this);
    }
}