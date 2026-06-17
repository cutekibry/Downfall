using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Hexaghost.HexaghostCode.Cards.Uncommon;
using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.CustomEnums;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Hexaghost.HexaghostCode.Cards.Basic;

[Pool(typeof(HexaghostCardPool))]
public class Float : HexaghostCardModel
{
    public Float() : base(0, CardType.Skill, CardRarity.Basic, TargetType.Self)
    {
        WithCards(1);
        WithKeyword(HexaghostKeyword.Advance, UpgradeType.Remove);
        this.WithTip(HexaghostKeyword.Advance, UpgradeType.Add);
        this.WithTip(HexaghostKeyword.Retract, UpgradeType.Add);
        this.WithTip(HexaghostKeyword.Still, UpgradeType.Add);
        WithVar("CardsPlayed", 3, 2);
    }

    protected override Artist Artist => Artist.Get<AlexMdle>();

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(ctx, Owner);
        if (IsUpgraded)
        {
            await ChoiceCard.SelectAction<FloatChoice>(ctx, this);
        }
    }
}

public class FloatChoice : ChoiceCard
{
    public override string CustomPortraitPath => ModelDb.Card<Float>().CustomPortraitPath;
}