using System.Runtime.CompilerServices;
using BaseLib.Utils;
using Downfall.DownfallCode.Artists;
using Hexaghost.HexaghostCode.Core;
using Hexaghost.HexaghostCode.CustomEnums;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Hexaghost.HexaghostCode.Cards.Uncommon;

[Pool(typeof(HexaghostCardPool))]
public class FlareFlick : HexaghostCardModel
{
    public FlareFlick() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithKeyword(HexaghostKeyword.Advance, UpgradeType.Remove);
        WithDamage(14, 4);
        this.WithTip(HexaghostKeyword.Advance, UpgradeType.Add);
        this.WithTip(HexaghostKeyword.Retract, UpgradeType.Add);
        this.WithTip(HexaghostKeyword.Still, UpgradeType.Add);
    }

    protected override Artist Artist => Artist.Get<CartesianCanvas>();

    protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        await CommonActions.CardAttack(this, cardPlay).Execute(ctx);
        await HexaghostCmd.Ignite(ctx, Owner);
        if (IsUpgraded)
        {
            await ChoiceCard.SelectAction<FlareFlickChoice>(ctx, this);
        }
    }
}


[Pool(typeof(TokenCardPool))]
public abstract class ChoiceCard : HexaghostCardModel
{
    public ChoiceCard() : base(-1, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithTips(c => c is ChoiceCard { Keyword: var keyword } ? [HoverTipFactory.FromKeyword(keyword)] : []);
    }

    public override CardPoolModel VisualCardPool => ModelDb.CardPool<HexaghostCardPool>();
    public CardKeyword Keyword = CardKeyword.Exhaust;

    protected override void AddExtraArgsToDescription(LocString description)
    {
        description.Add("Keyword", Keyword.GetTitle());
    }

    public static async Task SelectAction<T>(PlayerChoiceContext ctx, CardModel card)
        where T : ChoiceCard
    {
        var owner = card.Owner;
        var choices = new[] { HexaghostKeyword.Retract, HexaghostKeyword.Advance, HexaghostKeyword.Still }
            .Select(f =>
            {
                var choice = owner.Creature.CombatState!.CreateCard<T>(owner);
                choice.Keyword = f;
                return choice;
            })
            .ToList();
        var chosen = await CardSelectCmd.FromChooseACardScreen(ctx, choices, owner);
        if (chosen is not T { Keyword: var keyword }) return;
        if (keyword == HexaghostKeyword.Advance)
            await HexaghostCmd.Advance(ctx, owner, card);
        else if (keyword == HexaghostKeyword.Retract)
            await HexaghostCmd.Retract(ctx, owner, card);
    }
}

public class FlareFlickChoice : ChoiceCard
{
    public override string CustomPortraitPath => ModelDb.Card<FlareFlick>().CustomPortraitPath;
}