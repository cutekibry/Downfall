using BaseLib.Utils;
using Downfall.DownfallCode.Commands;
using Downfall.DownfallCode.CustomEnums;
using Downfall.DownfallCode.Extensions;
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
        var cards = Owner.GetDraw().Where(e => e.Rarity == CardRarity.Basic).ToList();
        var selected = (await DownfallCardCmd.SelectFromCards(ctx, cards, DownfallCardSelectorPrefs.ToHandSelectionPrompt, this)).FirstOrDefault();
        if (selected == null) return;
        selected.EnergyCost.SetThisTurn(0);
        await CardPileCmd.Add(selected, PileType.Hand);
    }
}