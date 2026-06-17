using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hexaghost.HexaghostCode.Cards.Rare;

[Pool(typeof(HexaghostCardPool))]
public class Doomsday : HexaghostCardModel
{
    public Doomsday() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        this.WithPower<DoomsdayPower>(6, -1, false);
    }

    protected override Artist Artist => Artist.Get<CartesianCanvas>();

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        if (HexaghostCmd.GetIgnitedCount(Owner) >= DynamicVars["DoomsdayPower"].IntValue)
        {
            await CommonActions.ApplySelf<DoomsArrivalPower>(ctx, this, 1);   
        }
        else
        {
            await CommonActions.ApplySelf<DoomsdayPower>(ctx, this);
        }
    }
}