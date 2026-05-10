using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Gremlins.GremlinsCode.Cards.Uncommon;

[Pool(typeof(GremlinsCardPool))]
public class Rhythm : GremlinsCardModel
{
    public Rhythm() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithCostUpgradeBy(-1);
    }
    
    protected override async Task PlayEffect(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        await GremlinsCmd.SwapToNext(ctx, Owner);
        var cards = Owner.PlayerCombatState?.DrawPile.Cards.Where(e => e.Rarity == CardRarity.Basic).ToList();
        if  (cards == null) return;
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        var card =( await CardSelectCmd.FromSimpleGrid(ctx, cards, Owner, prefs)).ToList();
        card.ForEach(e => e.EnergyCost.SetThisTurn(0));
        await CardPileCmd.Add(card, PileType.Hand);
    }
}