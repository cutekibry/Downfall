using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Downfall.DownfallCode.Commands;
using Downfall.DownfallCode.CustomEnums;
using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.CustomEnums;
using Hexaghost.HexaghostCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hexaghost.HexaghostCode.Cards.Uncommon;

[Pool(typeof(HexaghostCardPool))]
public class NightmareVision : HexaghostCardModel
{
    public NightmareVision() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithKeyword(HexaghostKeyword.Retract);
        WithTip(CardKeyword.Ethereal);
        WithKeyword(CardKeyword.Exhaust);
        WithCards(1, 1);
    }

    protected override Artist Artist => Artist.Get<Thelethargicweirdo>();

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var candidates = Owner.GetExhaust().Where(c => c.Keywords.Contains(CardKeyword.Ethereal)).ToList();
        var cards = await DownfallCardCmd.SelectFromCards(ctx, candidates, DownfallCardSelectorPrefs.ToHandSelectionPrompt, this);
        await CardPileCmd.Add(cards, PileType.Hand);
    }
}