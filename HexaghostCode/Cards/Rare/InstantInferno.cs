using BaseLib.Utils;
using Downfall.DownfallCode.Powers;
using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.CustomEnums;
using Hexaghost.HexaghostCode.Ghostflames;
using Hexaghost.HexaghostCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Hexaghost.HexaghostCode.Cards.Rare;

[Pool(typeof(HexaghostCardPool))]
public class InstantInferno : HexaghostCardModel
{
    public InstantInferno() : base(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithPower<SoulBurnPower>(12, 9);
        WithKeyword(CardKeyword.Retain);
        WithKeyword(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.Apply<SoulBurnPower>(ctx, this, cardPlay);
        if (HexaghostCmd.GetFlameOfType<InfernoGhostflame>(Owner) != null)
        {
            HexaghostCmd.SetCurrentGhostflame(Owner, HexaghostModelDb.Ghostflame<InfernoGhostflame>());
            await HexaghostCmd.Ignite(ctx, Owner);
        }
    }
}