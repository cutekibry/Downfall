using BaseLib.Utils;
using Gremlins.GremlinsCode.Cards.Basic;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Gremlins.GremlinsCode.Relics;

[Pool(typeof(GremlinsRelicPool))]
public class TagTeamwork : GremlinsRelicModel
{
    public TagTeamwork() : base(RelicRarity.Shop)
    {
        WithTip<TagTeam>();
    }

    public override async Task AfterCardDrawn(PlayerChoiceContext ctx, CardModel card, bool fromHandDraw)
    {
        if (card.Owner != Owner || card is not TagTeam) return;
        await CardPileCmd.Draw(ctx, Owner);
    }

    public override  async Task AfterObtained()
    {
        List<CardModel> cards = [ModelDb.Card<TagTeam>(), ModelDb.Card<TagTeam>()];
        var runStateCards = cards.Select(e => Owner.RunState.CreateCard(e, Owner));
        var results = await CardPileCmd.Add(runStateCards, PileType.Deck);
        CardCmd.PreviewCardPileAdd(results, 1f);
    }
}