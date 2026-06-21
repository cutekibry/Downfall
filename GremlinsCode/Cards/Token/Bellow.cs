using BaseLib.Utils;
using Gremlins.GremlinsCode.Core;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Gremlins.GremlinsCode.Cards.Token;

[Pool(typeof(TokenCardPool))]
public class Bellow : GremlinsCardModel
{
    public Bellow() : base(0, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithKeywords(CardKeyword.Ethereal, CardKeyword.Exhaust);
        WithCalculatedVar("Strength", 2, Calc, 1);
        this.WithTip<StrengthPower>();
    }

    private static decimal Calc(CardModel card, Creature? _)
    {
        return card.CombatState?.HittableEnemies.Count(e => !e.Monster?.IntendsToAttack ?? false) ?? 0;
    }

    protected override async Task OnPlayInternal(PlayerChoiceContext ctx, CardPlay cardPlay)
    {
        var strength = ((CalculatedVar)DynamicVars["Strength"]).Calculate(null);
        await CommonActions.ApplySelf<StrengthPower>(ctx, this, strength);
    }
}