using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Hexaghost.HexaghostCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hexaghost.HexaghostCode.Cards.Basic;

[Pool(typeof(HexaghostCardPool))]
public class Kindle : HexaghostCardModel
{
    public Kindle() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
    }

    protected override Artist Artist => Artist.Get<AlexMdle>();


    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        if (IsUpgraded || !HexaghostCmd.IsIgnited(Owner))
        {
            await HexaghostCmd.Ignite(ctx, Owner);
        }
    }
}