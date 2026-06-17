using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Hexaghost.HexaghostCode.Core;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Hexaghost.HexaghostCode.Cards.Common;

[Pool(typeof(HexaghostCardPool))]
public class StrikeFromBeyond : HexaghostCardModel
{
    public StrikeFromBeyond() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithTip(CardKeyword.Ethereal);
        WithDamage(4, 3);
        WithTags(CardTag.Strike);
    }

    protected override Artist Artist => Artist.Get<Inmo>();

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        if (!Owner.GetDraw().Any())
        {
            await CardPileCmd.Shuffle(ctx, Owner);
        }
        var card = Owner.GetDraw().FirstOrDefault(c => c.Keywords.Contains(CardKeyword.Ethereal));
        if (card != null)
        {
            await CardPileCmd.Add(card, PileType.Hand);
        }
    }
}