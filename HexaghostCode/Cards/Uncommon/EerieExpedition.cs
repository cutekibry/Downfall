using BaseLib.Utils;
using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.CustomEnums;
using Hexaghost.HexaghostCode.Extensions;
using Hexaghost.HexaghostCode.Interfaces;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Hexaghost.HexaghostCode.Cards.Multiplayer;

[Pool(typeof(HexaghostCardPool))]
public class EerieExpedition : HexaghostCardModel, IHasAfterlifeEffect
{
    public EerieExpedition() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies)
    {
        this.WithAfterlife();
        WithCostUpgradeBy(-1);
    }

    public async Task AfterlifeEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var cards = ModelDb.CardPool<HexaghostCardPool>().AllCards.Where(c => c.Keywords.Contains(HexaghostKeyword.Afterlife)).ToList();
        var card = CardFactory.GetDistinctForCombat(Owner, cards, 1, Owner.RunState.Rng.CombatCardGeneration)
            .First();
        CardCmd.Upgrade(card);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Draw, Owner, CardPilePosition.Random);
    }

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var cards = ModelDb.CardPool<HexaghostCardPool>().AllCards.Where(c => c.Keywords.Contains(HexaghostKeyword.Afterlife)).ToList();
        var card = CardFactory.GetDistinctForCombat(Owner, cards, 1, Owner.RunState.Rng.CombatCardGeneration)
            .First();
        CardCmd.Upgrade(card);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        await AfterlifeEffect(ctx, cardPlay);
    }
}