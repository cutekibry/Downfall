using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hexaghost.HexaghostCode.Cards.Rare;

[Pool(typeof(HexaghostCardPool))]
public class Poltergeist : HexaghostCardModel
{
    public Poltergeist() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        this.WithPower<PoltergeistPower>(6, 2, false);
        WithTip(CardKeyword.Ethereal);
    }

    protected override Artist Artist => Artist.Get<Inmo>();

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<PoltergeistPower>(ctx, this);
    }
}